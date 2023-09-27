using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCharacter : MonoBehaviour
{
    public float movementSpeed = 2.0f;

    // public int maxPuppyNumber = 10;
    public Puppy[] puppies = new Puppy[10];
    public int numPuppiesObtained = 0;

    public Transform arm;

    // public float maxRotationAngle = 1;
    public float minMovementRequired;
    private float lerp_t = 5f;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var p in GameObject.FindGameObjectsWithTag("Puppy")) {
            Puppy puppy = p.GetComponent<Puppy>();
            if(!puppy.isStray) {
                AddPuppy(puppy);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (GetAveragePuppyPosition() - transform.position).normalized;
        MoveInDirection(direction, movementSpeed);
    }

    public void AddPuppy(Puppy puppy) {
        if(puppies.Length == numPuppiesObtained) {
            Debug.Log("You win");
        } else 
        {
            puppies[numPuppiesObtained] = puppy;
            puppies[numPuppiesObtained].StrayNoMore(numPuppiesObtained, transform, arm);
            numPuppiesObtained += 1;
        }
    }

    public Vector3 GetAveragePuppyPosition() {
        var positionSum = Vector3.zero;
        foreach(var p in puppies) {
            if(p != null) {
                positionSum += p.transform.position;
            }
        }

        return positionSum / numPuppiesObtained;
    }

    /*
    public void MoveTowards(Vector3 targetPosition) {
        transform.position = targetPosition;
    }
    */

    void MoveInDirection(Vector3 direction, float speed) {
        
        // find location where it wants to move
        var whereToGo = transform.position + direction * speed * Time.deltaTime;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(whereToGo, out hit, 100f, NavMesh.AllAreas)) {
            // if((hit.position - transform.position).magnitude > minMovementRequired) {
            
            if((hit.position-transform.position).magnitude > minMovementRequired) {
                // rotate
                var save = transform.eulerAngles;
                transform.LookAt(hit.position);
                float newRotation = Mathf.LerpAngle(save.y, transform.eulerAngles.y, lerp_t * Time.deltaTime);
                
                transform.eulerAngles = new Vector3(0, newRotation, 0);
            }
            // Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            // transform.rotation = rotation;

            // move
            transform.position = hit.position;
            // }
            
        } else {
            // stop
            Debug.Log("Player edge reached");
        }
    }
}

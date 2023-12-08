using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Necessary references")]
    public Transform arm;

    [Header("Tweakable")]
    //  public int maxPuppyNumber = 10;
    public Puppy[] puppies;                      // how many puppies the player can connect (getting all should mean victory, should be changed in scene) [edit only the length of the array]
    private int numPuppiesNeededToRetrieve;      // number of puppies needed to retrieve (calculates based on max number and starting number)
    private int numPuppiesObtained;  // 0        // number of puppies currently connected      
    private int startingNumber;      // 0        // number of puppies at the beginning of the level

    // public float maxRotationAngle = 1;
    public float movementSpeed; // 2.5f         // how fast the character moves 
    public float minMovementRequired;           // minimum movement distance required to move
    private float lerp_t = 5f; // 5f

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var p in GameObject.FindGameObjectsWithTag("Puppy")) {
            Puppy puppy = p.GetComponent<Puppy>();
            if(!puppy.IsStray()) {
                AddPuppy(puppy);
            }
        }
        startingNumber = numPuppiesObtained;
        numPuppiesNeededToRetrieve = puppies.Length - numPuppiesObtained + 1;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 direction = (GetAveragePuppyPosition() - transform.position).normalized;
        MoveInDirection(direction, movementSpeed);
        */

        Movement();
    }

    public void AddPuppy(Puppy puppy) {
        // Debug.Log(numPuppiesObtained);
        if(puppies.Length == numPuppiesObtained) {
            Debug.Log("You win");
        } else 
        {
            puppies[numPuppiesObtained] = puppy;
            puppies[numPuppiesObtained].StrayNoMore(numPuppiesObtained, transform, arm);
            numPuppiesObtained += 1;
        }

        if(puppies.Length == numPuppiesObtained) {
            // Debug.Log("WIN CONDITION MET, PLAYER SIDE");
            GameManager gm = FindObjectOfType<GameManager>();
            gm.WinConditionMet();
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

    public void Movement()
    {
        // Calculate forces from dogs
        Vector3 puppyForce = Vector3.zero;
        foreach(Puppy puppy in puppies)
        {
            if(puppy != null && puppy.isFree)
            {
                puppyForce += (puppy.transform.position - transform.position);
            }
        }
        // puppyForce.Normalize();
        // puppyForce *= movementSpeed * Time.deltaTime;

        // Check max distances
        float moveMagnitude = movementSpeed * Time.deltaTime;
        bool shouldMove = moveMagnitude >= minMovementRequired;
        Vector3 targetLocation = transform.position + puppyForce.normalized * moveMagnitude;
        if (shouldMove)
        {
            foreach (Puppy puppy in puppies)
            {
                if (puppy != null && Vector3.Distance(targetLocation, puppy.transform.position) > puppy.maxDistance)
                {
                    shouldMove = false;
                    break;
                }
            }
        }

        // Move
        if (shouldMove)
        {
            // rotate
            var save = transform.eulerAngles;
            transform.LookAt(transform.position + puppyForce.normalized);
            float newRotation = Mathf.LerpAngle(save.y, transform.eulerAngles.y, lerp_t * Time.deltaTime);

            transform.eulerAngles = new Vector3(0, newRotation, 0);

            // Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            // transform.rotation = rotation;

            // move
            transform.position = targetLocation;
        }
    }

    /*
    public void MoveTowards(Vector3 targetPosition) {
        transform.position = targetPosition;
    }
    */

    /*
     * DEPRICATED
     */
    void MoveInDirection(Vector3 direction, float speed) {
        var whereToGo = transform.position + direction * speed * Time.deltaTime;

        if ((whereToGo - transform.position).magnitude > minMovementRequired) {
            // rotate
            var save = transform.eulerAngles;
            transform.LookAt(whereToGo);
            float newRotation = Mathf.LerpAngle(save.y, transform.eulerAngles.y, lerp_t * Time.deltaTime);

            transform.eulerAngles = new Vector3(0, newRotation, 0);

            // Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            // transform.rotation = rotation;

            // move
            transform.position = whereToGo;
        }

        /*
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
        */
    }

    public int GetNumPuppiesObtained()
    {
        return numPuppiesObtained;
    }


    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Obstacle")) {
            Debug.Log("DEAD");
            GameManager gm = FindObjectOfType<GameManager>();
            gm.LoseConditionMet();
        }
    }
}

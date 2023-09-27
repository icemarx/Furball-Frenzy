using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Puppy : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public float moveAwaySpeed = 1.5f; // Adjust this to control the puppy's movement speed
    public float moveToPlayerSpeed = 3.0f;
    public float closeToPlayerDistance = 0.1f; // distance to player that is good enough

    public bool isFree = true;

    public float maxDistance;

    public int puppyIndex = -1;


    void Update() {
        if (player != null) {
            Movement();
        }
    }


    void Movement() {
        if(isFree) {
            // Calculate the direction from the puppy to the player
            Vector3 awayFromPlayer = transform.position - player.position;

            if (awayFromPlayer.magnitude <= maxDistance) {
                // other rules
                // Vector3 awayFromPuppies = transform.position - AverageOtherPuppies();
                Vector3 circleDir = CircleDirection();
                Vector3 randomness = Random.insideUnitSphere;
                randomness.y = 0;

                // join and normalize
                // Vector3 moveDirection = awayFromPlayer + awayFromPuppies + randomness;
                Vector3 moveDirection = awayFromPlayer + circleDir + randomness;
                moveDirection.Normalize();

                // move
                MoveInDirection(moveDirection, moveAwaySpeed);
            } else {
                // stop
            }
        } else {
            Vector3 toPlayer = player.position - transform.position;
            MoveInDirection(toPlayer.normalized, moveToPlayerSpeed);

            if (toPlayer.magnitude <= closeToPlayerDistance) {
                isFree = true;
            }
        }
    }

    void MoveInDirection(Vector3 direction, float speed) {
        // find location where it wants to move
        var whereToGo = transform.position + direction * speed * Time.deltaTime;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(whereToGo, out hit, 100f, NavMesh.AllAreas)) {
            // move
            transform.LookAt(hit.position);
            transform.position = hit.position;
        } else {
            // stop
            Debug.Log("Puppy edge reached");
        }
    }


    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Player")) {
            isFree = true;
        }
    }

    public Vector3 AverageOtherPuppies() {
        Vector3 avg = Vector3.zero;
        int count = 0;
        foreach(var puppy in player.GetComponent<PlayerCharacter>().puppies) {
            if(puppy && puppy != this.gameObject) {
                avg += puppy.transform.position;
                count += 1;
            }
        }

        return avg / count;
    }

    public Vector3 CircleDirection() {
        float angle = 360.0f * puppyIndex / player.GetComponent<PlayerCharacter>().numPuppiesObtained;
        return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
    }


    private void OnMouseDown() {
        Debug.Log(this.name);
    }
}

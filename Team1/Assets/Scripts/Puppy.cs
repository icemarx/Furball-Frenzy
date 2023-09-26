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


    void Update() {
        if (player != null) {
            Movement();
        }
    }


    void Movement() {
        if(isFree) {
            // Calculate the direction from the puppy to the player
            Vector3 moveDirection = transform.position - player.position;

            Vector3 randomness = Random.insideUnitSphere;
            randomness.y = 0;
            moveDirection += randomness;

            // Normalize the direction vector to make the movement consistent
            moveDirection.Normalize();

            MoveInDirection(moveDirection, moveAwaySpeed);            
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


    private void OnMouseDown() {
        Debug.Log(this.name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Puppy : MonoBehaviour
{

    public Transform player; // Reference to the player's Transform
    public float movementSpeed = 3.0f; // Adjust this to control the puppy's movement speed

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


            // find location where it wants to move
            var whereToGo = transform.position + moveDirection * movementSpeed * Time.deltaTime;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(whereToGo, out hit, 100f, NavMesh.AllAreas)) {
                // move
                transform.LookAt(hit.position);
                transform.position = hit.position;
            } else {
                // stop
                Debug.Log("Puppy edge reached");
            }



        } else {
            transform.Translate(player.position);
            isFree = true;
        }
    }
}

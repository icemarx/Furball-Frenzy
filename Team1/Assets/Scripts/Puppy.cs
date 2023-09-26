using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puppy : MonoBehaviour
{

    public Transform player; // Reference to the player's Transform
    public float movementSpeed = 3.0f; // Adjust this to control the puppy's movement speed

    void Update() {
        if (player != null) {
            Movement();
        }
    }


    void Movement() {
        // Calculate the direction from the puppy to the player
        Vector3 moveDirection = transform.position - player.position;

        // Normalize the direction vector to make the movement consistent
        moveDirection.Normalize();

        // Move the puppy in the opposite direction of the player
        transform.Translate(moveDirection * movementSpeed * Time.deltaTime);
    }
}

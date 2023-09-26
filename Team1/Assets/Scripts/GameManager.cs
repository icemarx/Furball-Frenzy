using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LayerMask collisionLayer; // Layer mask for collision detection7
    public float detectionRadius;

    public GameObject spherePrefab;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
           {
            // Raycast from the mouse position into the scene
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionLayer)) {
                /*
                // Create a new sphere at the hit point
                GameObject newSphere = Instantiate(spherePrefab, hit.point, Quaternion.identity);
                newSphere.transform.localScale = new Vector3(1, 1, 1) * detectionRadius;
                */

                // Check for collisions with other objects (if needed)
                Collider[] colliders = Physics.OverlapSphere(hit.point, detectionRadius);

                // Handle collisions
                foreach (Collider collider in colliders) {
                    if (collider.gameObject.CompareTag("Puppy")) {
                        collider.gameObject.GetComponent<Puppy>().isFree = false;
                        break;
                    } else if(collider.gameObject.CompareTag("Rope")) {
                        collider.gameObject.GetComponentInParent<Puppy>().isFree = false;
                        break;
                    }
                }
            }
        }
    }
}

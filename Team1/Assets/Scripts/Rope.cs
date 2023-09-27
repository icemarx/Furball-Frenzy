using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Transform puppy;
    public Transform arm;
    

    public void LateUpdate() {
        UpdateRope();
    }

    public void UpdateRope() {
        if (arm) {
            // rotate
            transform.LookAt(arm);

            // scale
            float distanceToPlayer = (arm.position - transform.position).magnitude;
            Vector3 newScale = transform.localScale;
            newScale.z = distanceToPlayer / 2;
            transform.localScale = newScale;
        }
    }
}

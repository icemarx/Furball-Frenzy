using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform root;

    public float minMovementRequired;
    public float interpolationStep;

    // Update is called once per frame
    void LateUpdate()
    {
        if(player && Vector3.Distance(root.position, player.position) > minMovementRequired) {
            // root.position = player.transform.position;
            root.position = Vector3.Lerp(root.position, player.position, interpolationStep);
        }
    }
}

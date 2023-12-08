using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Necessary references")]
    public Transform player;

    [Header("Tweakable")]
    public float minMovementRequired;   // 0.1f     // minimum movement required for camera to move
    public float interpolationStep;     // 0.05f    // how fast the camera moves [range: 0-1]


    // Update is called once per frame
    void LateUpdate()
    {
        if(player && Vector3.Distance(transform.position, player.position) > minMovementRequired) {
            // root.position = player.transform.position;
            transform.position = Vector3.Lerp(transform.position, player.position, interpolationStep);
        }
    }
}

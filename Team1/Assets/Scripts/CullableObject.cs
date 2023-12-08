using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullableObject : MonoBehaviour
{
    [Header("Necessary references")]
    private MeshRenderer mesh;
    private Camera mainCamera;

    [Header("Tweakable")]
    public float hideDistance;  // 10f      // distance from camera at which the object will stop rendering
    public float apearDistance; // 13.5f    // distance from camera at which the object will render


    private void Start() {
        mesh = gameObject.GetComponent<MeshRenderer>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(mesh.enabled && Vector3.Distance(mainCamera.transform.position, this.transform.position) < hideDistance) {
            mesh.enabled = false;
        } else if(Vector3.Distance(mainCamera.transform.position, this.transform.position) > apearDistance) {
            mesh.enabled = true;
        }
    }
}

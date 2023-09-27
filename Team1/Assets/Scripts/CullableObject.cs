using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullableObject : MonoBehaviour
{
    public float hideDistance = 1;
    public float apearDistance = 1.5f;

    private MeshRenderer mesh;
    private Camera mainCamera;

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

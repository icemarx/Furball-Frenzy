using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform player;
    public Transform root;

    // Update is called once per frame
    void Update()
    {
        if(player) {
            root.position = player.transform.position;
        }
    }
}

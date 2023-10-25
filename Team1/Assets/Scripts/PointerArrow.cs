using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerArrow : MonoBehaviour
{
    public Transform anchor;
    public float timeActive;
    public float timeInactive;
    public float timer = 0f;
    public Puppy target = null;

    public bool arrowActive;
    public MeshRenderer[] children;

    

    // Start is called before the first frame update
    void Start()
    {
        children = GetComponentsInChildren<MeshRenderer>();

        if(arrowActive)
        {
            SwitchToActive();
        } else
        {
            SwitchToInactive();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // check timer
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            // switch
            if(arrowActive)
            {
                SwitchToInactive();
            } else
            {
                SwitchToActive();
            }
        }

        if(arrowActive)
        {
            transform.position = anchor.position;

            // check target
            if(target == null || !target.IsStray())
            {
                target = ChoseClosestStray();
                if(target == null)
                {
                    SwitchToInactive();
                }
            }

            if (target != null)
            {
                // rotate
                Vector3 pos = target.transform.position;
                pos.y = transform.position.y;
                transform.LookAt(pos);
            }
        }

    }

    public void SwitchToInactive()
    {
        arrowActive = false;
        timer = timeInactive;
        target = null;

        // Hide childs
        foreach(var child in children)
        {
            child.enabled = false;
        }
    }

    public void SwitchToActive()
    {
        arrowActive = true;
        timer = timeActive;
        target = ChoseClosestStray();

        // Display children
        foreach (var child in children)
        {
            child.enabled = true;
        }
    }

    public Puppy ChoseClosestStray()
    {
        Puppy[] puppies = FindObjectsByType<Puppy>(FindObjectsSortMode.None);
        Puppy closest = null;

        foreach(Puppy puppy in puppies)
        {
            if(puppy.IsStray())
            {
                if(closest == null)
                {
                    closest = puppy;
                } else if(Vector3.Distance(this.transform.position, puppy.transform.position) < Vector3.Distance(this.transform.position, closest.transform.position))
                {
                    closest = puppy;
                }
            }
        }

        return closest;
    }
}

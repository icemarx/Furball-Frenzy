using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerArrow : MonoBehaviour
{
    [Header("Necessary references")]
    public Transform anchor;
    public bool arrowActive;
    private Puppy target = null;
    private MeshRenderer[] children;

    [Header("Tweakable")]
    public float timeActive;    // 5f       // time when the arrow is visible and active
    public float timeInactive;  // 10f      // time when the arrow is invisible and inactive
    private float timer = 0f;    // 0f      // keeps track of time

    

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    // public int maxPuppyNumber = 10;
    public GameObject[] puppies = new GameObject[10];
    public int numPuppiesObtained = 0;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var p in GameObject.FindGameObjectsWithTag("Puppy")) {
            AddPuppy(p);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var middlePoint = GetAveragePuppyPosition();
        MoveTowards(middlePoint);
    }

    public void AddPuppy(GameObject puppy) {
        if(puppies.Length == numPuppiesObtained) {
            Debug.Log("You win");
        } else 
        {
            puppies[numPuppiesObtained] = puppy;
            numPuppiesObtained += 1;
        }
    }

    public Vector3 GetAveragePuppyPosition() {
        var positionSum = Vector3.zero;
        foreach(var p in puppies) {
            if(p != null) {
                positionSum += p.transform.position;
            }
        }

        return positionSum / numPuppiesObtained;
    }

    public void MoveTowards(Vector3 targetPosition) {
        transform.position = targetPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Necessary references")]
    public Puppy puppy;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            RecievePuppy(collision.gameObject.GetComponent<PlayerCharacter>());
            RemoveSelf();
        }
    }

    private void RecievePuppy(PlayerCharacter player) {
        puppy.transform.parent = this.transform.parent;
        player.AddPuppy(puppy);
    }

    private void RemoveSelf() {
        Destroy(this.gameObject);
    }
}

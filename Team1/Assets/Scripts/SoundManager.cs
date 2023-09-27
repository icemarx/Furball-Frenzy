using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip mainTheme; // Reference to the main theme audio clip
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start() {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Set the main theme clip to loop
        audioSource.clip = mainTheme;
        audioSource.loop = true;

        // Play the main theme
        audioSource.Play();
    }
}

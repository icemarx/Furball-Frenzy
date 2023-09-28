using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip mainTheme; // Reference to the main theme audio clip
    public AudioClip tutorialTheme;
    public AudioClip winTheme;
    public AudioClip loseTheme;
    private AudioSource audioSource; // Reference to the AudioSource component

    public AudioClip[] happyBarks;
    public AudioClip[] sadBarks;

    void Awake() {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        /*
        // Set the main theme clip to loop
        audioSource.clip = mainTheme;
        audioSource.loop = true;

        // Play the main theme
        audioSource.Play();
        */
    }

    public void ChooseTrack(int state) {
        audioSource.Stop();
        switch (state) {
            case GameManager.ACTIVE_GAME_STATE:
                audioSource.clip = mainTheme;
                audioSource.loop = true;
                audioSource.Play();
                break;
            case GameManager.TUTORIAL_STATE:
                audioSource.clip = tutorialTheme;
                audioSource.loop = true;
                audioSource.Play();
                break;
            case GameManager.WIN_STATE:
                audioSource.clip = winTheme;
                audioSource.loop = false;
                audioSource.Play();

                // TODO: play happy bark after
                break;
            case GameManager.LOSE_STATE:
                audioSource.clip = loseTheme;
                audioSource.loop = false;
                audioSource.Play();

                // TODO: play sad bark after
                break;
        }
    }
}

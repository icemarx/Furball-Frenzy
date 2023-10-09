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

    private bool waitForNext = false;
    private AudioClip nextAudio = null;

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

    private void Update()
    {
        if(waitForNext && !audioSource.isPlaying && nextAudio != null)
        {
            audioSource.clip = nextAudio;
            audioSource.Play();
            nextAudio = null;
            waitForNext = false;
        }
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

                // play happy bark after
                nextAudio = RandomAudio(happyBarks);
                waitForNext = true;
                break;
            case GameManager.LOSE_STATE:
                audioSource.clip = loseTheme;
                audioSource.loop = false;
                audioSource.Play();

                //  play sad bark after
                nextAudio = RandomAudio(sadBarks);
                waitForNext = true;
                break;
        }
    }

    private AudioClip RandomAudio(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }
}

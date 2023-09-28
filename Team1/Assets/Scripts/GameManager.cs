using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public const int TUTORIAL_STATE = 0;
    public const int ACTIVE_GAME_STATE = 1;
    public const int WIN_STATE = 2;
    public const int LOSE_STATE = 3;
    public int gameState = ACTIVE_GAME_STATE;

    public PlayerCharacter player;

    public LayerMask collisionLayer; // Layer mask for collision detection7
    public float detectionRadius;

    public GameObject spherePrefab;

    [Header("UI elements")]
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject tutorialScreen;
    public GameObject gameScreen;
    public Button playButton;
    public Button restartLevelButton;
    public Button nextLevelButton;
    public Button tryAgainButton;
    public TMP_Text textNumPuppies;

    [Header("Audio")]
    public SoundManager sm;

    public void Awake() {
        playButton.onClick.AddListener(StartGame);
        restartLevelButton.onClick.AddListener(RestartLevel);
        nextLevelButton.onClick.AddListener(NextLevel);
        tryAgainButton.onClick.AddListener(RestartLevel);
    }

    public void Start() {
        HandleGameStateChange(gameState);
    }


    // Update is called once per frame
    void Update()
    {
        if(gameState == ACTIVE_GAME_STATE) {
            // UI update
            UpdateCounter();

            // INPUT
            if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
               {
                // Raycast from the mouse position into the scene
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionLayer)) {
                    /*
                    // Create a new sphere at the hit point
                    GameObject newSphere = Instantiate(spherePrefab, hit.point, Quaternion.identity);
                    newSphere.transform.localScale = new Vector3(1, 1, 1) * detectionRadius;
                    */

                    // Check for collisions with other objects (if needed)
                    Collider[] colliders = Physics.OverlapSphere(hit.point, detectionRadius);

                    // Handle collisions
                    foreach (Collider collider in colliders) {
                        if (collider.gameObject.CompareTag("Puppy")) {
                            collider.gameObject.GetComponent<Puppy>().isFree = false;
                            break;
                        } else if (collider.gameObject.CompareTag("Rope")) {
                            collider.gameObject.GetComponentInParent<Puppy>().isFree = false;
                            break;
                        }
                    }
                }
            }
        }
    }


    // GAME STATE ================================================================================================
    public void HandleGameStateChange(int newState) {
        switch(newState) {
            case TUTORIAL_STATE:
                tutorialScreen.SetActive(true);
                winScreen.SetActive(false);
                loseScreen.SetActive(false);
                gameScreen.SetActive(false);
                sm.ChooseTrack(newState);
                break;

            case ACTIVE_GAME_STATE:
                tutorialScreen.SetActive(false);
                winScreen.SetActive(false);
                loseScreen.SetActive(false);
                gameScreen.SetActive(true);
                UpdateCounter();
                sm.ChooseTrack(newState);
                break;

            case WIN_STATE:
                tutorialScreen.SetActive(false);
                winScreen.SetActive(true);
                loseScreen.SetActive(false);
                gameScreen.SetActive(true);
                sm.ChooseTrack(newState);
                break;

            case LOSE_STATE:
                tutorialScreen.SetActive(false);
                winScreen.SetActive(false);
                loseScreen.SetActive(true);
                gameScreen.SetActive(true);
                sm.ChooseTrack(newState);
                break;
                
        }
        gameState = newState;
    }


    // UI ========================================================================================================
    public void StartGame() {
        // HandleGameStateChange(ACTIVE_GAME_STATE);
        SceneManager.LoadScene("Level 1");
    }

    public void RestartLevel() {
        Debug.Log("Restart Level");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Level 1");
    }


    public void NextLevel() {
        Debug.Log("Next Level");
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCount);
    }

    public void UpdateCounter() {
        // get number from player and set it as text
        var counter = player.numPuppiesObtained - player.startingNumber;
        textNumPuppies.SetText("" + counter);
    }

    public void WinConditionMet() {
        UpdateCounter();
        // Debug.Log("Win Condition met, GM side");

        HandleGameStateChange(WIN_STATE);
    }

    public void LoseConditionMet() {
        HandleGameStateChange(LOSE_STATE);
    }
}

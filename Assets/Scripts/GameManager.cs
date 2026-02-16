using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Lives")]
    public int startingLives = 3;
    private int currentLives;

    [Header("References")]
    public Transform player;        
    public Transform respawnPoint;  
    public Text lifeText;           

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // Listen for scene load events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called each time a new scene loads
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the player in the new scene
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Find respawn point in the new scene
        GameObject respawnObj = GameObject.Find("RespawnPoint");
        if (respawnObj != null)
            respawnPoint = respawnObj.transform;

        // Find LifeText UI in the new scene
        GameObject lifeTextObj = GameObject.Find("LifeText");
        if (lifeTextObj != null)
            lifeText = lifeTextObj.GetComponent<Text>();

        // Update UI
        UpdateLifeUI();
    }

    void Start()
    {
        currentLives = startingLives;
        UpdateLifeUI();
    }

    // player damange
    public void LoseLife()
    {
        currentLives--;
        UpdateLifeUI();
        Debug.Log("Player lost a life! Remaining: " + currentLives);

        if (currentLives > 0)
        {
            RespawnPlayer();
        }
        else
        {
            GameOver();
        }
    }

    void UpdateLifeUI()
    {
        if (lifeText != null)
            lifeText.text = "x" + currentLives;
    }

    //  Respawn player
    void RespawnPlayer()
    {
        if (player != null && respawnPoint != null)
        {
            Debug.Log("Respawning player...");
            player.position = respawnPoint.position;

            Rigidbody2D rb2D = player.GetComponent<Rigidbody2D>();
            if (rb2D != null)
                rb2D.linearVelocity = Vector2.zero;

            Rigidbody rb3D = player.GetComponent<Rigidbody>();
            if (rb3D != null)
            {
                rb3D.linearVelocity = Vector3.zero;
                rb3D.angularVelocity = Vector3.zero;
            }
        }
    }

    //  GAME OVER → End Scene
    void GameOver()
    {
        Debug.Log("Game Over! Loading GameOver scene...");
        SceneManager.LoadScene("GameOver");
    }

    //  BOSS DEFEATED → Winner Scene
    public void BossDefeated()
    {
        Debug.Log("Boss defeated! Loading WinnerScene...");
        SceneManager.LoadScene("Winner");
    }

    //  Replay button
    public void ReplayGame()
    {
        // Reset lives first
        currentLives = startingLives;

        // Load main game scene
        SceneManager.LoadScene("GameScene-ALU");
    }

    // Main Menu button
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("main menu");
    }

    //  Quit button
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

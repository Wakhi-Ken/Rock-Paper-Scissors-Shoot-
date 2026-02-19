using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuCanvas;

    [Header("Settings")]
    public KeyCode pauseKey = KeyCode.Escape;
    public bool debugMode = true;

    // Store original cursor settings to restore them
    private bool wasCursorVisible;
    private CursorLockMode wasCursorLockState;

    void Awake()
    {
        if (PauseMenuCanvas == null)
        {
            Debug.LogError("PauseMenuCanvas is not assigned in the inspector!");
            return;
        }

        PauseMenuCanvas.SetActive(false);
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Store initial cursor settings
        StoreCursorSettings();
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    void StoreCursorSettings()
    {
        wasCursorVisible = Cursor.visible;
        wasCursorLockState = Cursor.lockState;
    }

    void TogglePause()
    {
        if (GameIsPaused)
        {
            Play();
        }
        else
        {
            Stop();
        }
    }

    void Stop()
    {
        if (PauseMenuCanvas == null) return;

        if (debugMode)
            Debug.Log("Pausing game...");

        // Store current cursor settings before changing them
        StoreCursorSettings();

        PauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        // Unlock cursor for menu interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Play()
    {
        if (PauseMenuCanvas == null) return;

        if (debugMode)
            Debug.Log("Unpausing game...");

        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Restore original cursor settings
        RestoreCursorSettings();
    }

    void RestoreCursorSettings()
    {
        Cursor.visible = wasCursorVisible;
        Cursor.lockState = wasCursorLockState;

        if (debugMode)
            Debug.Log($"Cursor restored - Visible: {wasCursorVisible}, Lock State: {wasCursorLockState}");
    }

    public void MainMenu()
    {
        if (debugMode)
            Debug.Log("Loading Main Menu...");

        Time.timeScale = 1f;
        GameIsPaused = false;

        SceneManager.LoadScene("MainMenu");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneswitcher : MonoBehaviour
{
    [Header("Scene Names")]
    public string gameSceneName = "GameWorld"; // Change this to your actual game scene name
    public string mainMenuSceneName = "MainMenu"; // Change this to your actual main menu scene name

    // For Replay button - loads the game scene
    public void ReplayGame()
    {
        Debug.Log("Replaying game...");
        
        // Option 1: Use GameManager if it exists
        if (GameManager.instance != null)
        {
            GameManager.instance.ReplayGame();
        }
        else
        {
            // Option 2: Direct scene loading
            Time.timeScale = 1f; // Ensure time is running
            SceneManager.LoadScene(gameSceneName);
        }
    }

    // For Main Menu button
    public void GoToMainMenu()
    {
        Debug.Log("Going to Main Menu...");
        
        if (GameManager.instance != null)
        {
            GameManager.instance.GoToMainMenu();
        }
        else
        {
            Time.timeScale = 1f; // Ensure time is running
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    // For Quit button
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        
        if (GameManager.instance != null)
        {
            GameManager.instance.QuitGame();
        }
        else
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
    
    // Additional helper method for loading any scene by name
    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is empty!");
        }
    }
    
    // Helper method to reload current scene
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentScene.name);
    }
}
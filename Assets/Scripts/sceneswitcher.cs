using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneswitcher : MonoBehaviour
{
    public void ReplayGame()
    {
        if (GameManager.instance != null)
            GameManager.instance.ReplayGame();
    }

    public void GoToMainMenu()
    {
        if (GameManager.instance != null)
            GameManager.instance.GoToMainMenu();
    }

    public void QuitGame()
    {
        if (GameManager.instance != null)
            GameManager.instance.QuitGame();
    }
}

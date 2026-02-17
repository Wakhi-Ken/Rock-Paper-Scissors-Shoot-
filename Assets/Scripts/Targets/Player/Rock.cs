using UnityEngine;

public class Rock : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("ROCK CLICKED");
        RPSGameManager.instance.SetPlayerChoice(1);
    }
}

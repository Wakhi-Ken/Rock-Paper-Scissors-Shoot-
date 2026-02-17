using UnityEngine;

public class Scissors : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("SCISSORS CLICKED");
        RPSGameManager.instance.SetPlayerChoice(3);
    }
}

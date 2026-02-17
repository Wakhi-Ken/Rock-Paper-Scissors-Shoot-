using UnityEngine;

public class Paper : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("PAPER CLICKED");
        RPSGameManager.instance.SetPlayerChoice(2);
    }
}

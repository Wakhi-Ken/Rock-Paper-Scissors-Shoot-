using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RPSGameManager : MonoBehaviour
{
    public static RPSGameManager instance;

    // ENUM for choices
    public enum Choice { None, Rock, Paper, Scissors }

    private Choice playerChoice = Choice.None;
    private Choice botChoice = Choice.None;

    private bool hasShot = false;   // Anti-cheat lock

    [Header("UI")]
    public TMP_Text playerChoiceText;
    public TMP_Text botChoiceText;
    public TMP_Text resultText;
    public Button replayButton;

    [Header("Bot Scripts")]
    public RockBot rockBot;
    public PaperBot paperBot;
    public ScissorsBot scissorsBot;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ResetRound();
    }

    // PLAYER selects Rock / Paper / Scissors
    public void SetPlayerChoice(int choiceIndex)
    {
        if (hasShot) return;   // Anti-cheat lock

        playerChoice = (Choice)choiceIndex;
        playerChoiceText.text = "Player: " + playerChoice.ToString();
    }

    // SHOOT button pressed
    public void Shoot()
    {
        if (playerChoice == Choice.None) return;

        hasShot = true;

        GenerateBotChoice();
        CompareChoices();

        replayButton.gameObject.SetActive(true);
    }

    // RANDOM BOT CHOICE + SHOW BOT SPRITE
    void GenerateBotChoice()
    {
        int random = Random.Range(1, 4);
        botChoice = (Choice)random;
        botChoiceText.text = "Bot: " + botChoice.ToString();

        // Hide all bot visuals first
        rockBot.Hide();
        paperBot.Hide();
        scissorsBot.Hide();

        // Show chosen bot
        switch (botChoice)
        {
            case Choice.Rock:
                rockBot.Show();
                break;

            case Choice.Paper:
                paperBot.Show();
                break;

            case Choice.Scissors:
                scissorsBot.Show();
                break;
        }
    }

    // ROCK PAPER SCISSORS RULES
    void CompareChoices()
    {
        string result = "";

        switch (playerChoice)
        {
            case Choice.Rock:
                if (botChoice == Choice.Scissors) result = "YOU WIN!";
                else if (botChoice == Choice.Paper) result = "YOU LOSE!";
                else result = "DRAW!";
                break;

            case Choice.Paper:
                if (botChoice == Choice.Rock) result = "YOU WIN!";
                else if (botChoice == Choice.Scissors) result = "YOU LOSE!";
                else result = "DRAW!";
                break;

            case Choice.Scissors:
                if (botChoice == Choice.Paper) result = "YOU WIN!";
                else if (botChoice == Choice.Rock) result = "YOU LOSE!";
                else result = "DRAW!";
                break;
        }

        resultText.text = result;
    }

    // REPLAY BUTTON
    public void ResetRound()
    {
        playerChoice = Choice.None;
        botChoice = Choice.None;
        hasShot = false;

        playerChoiceText.text = "Player: ?";
        botChoiceText.text = "Bot: ?";
        resultText.text = "Choose and Press SHOOT";

        replayButton.gameObject.SetActive(false);

        // Hide bot visuals
        rockBot.Hide();
        paperBot.Hide();
        scissorsBot.Hide();
    }
}

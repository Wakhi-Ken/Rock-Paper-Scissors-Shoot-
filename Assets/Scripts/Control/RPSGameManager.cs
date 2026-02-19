using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RPSGameManager : MonoBehaviour
{
    public static RPSGameManager instance;

    public enum Choice { None, Rock, Paper, Scissors }

    private Choice playerChoice = Choice.None;
    private Choice botChoice = Choice.None;

    private bool hasShot = false;
    private int playerWins = 0;
    private int botWins = 0;

    [Header("UI")]
    public TMP_Text playerChoiceText;
    public TMP_Text botChoiceText;
    public TMP_Text resultText;
    public TMP_Text scoreText;

    [Header("Bot Scripts")]
    public RockBot rockBot;
    public PaperBot paperBot;
    public ScissorsBot scissorsBot;

    [Header("Cursor Settings")]
    public bool keepCursorVisible = true;
    public CursorLockMode cursorLockMode = CursorLockMode.None;

    // Store the last few bot choices to avoid patterns
    private Choice[] lastBotChoices = new Choice[3];
    private int choiceIndex = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Configure cursor to always be visible
        ConfigureCursor();
        ResetRound();
    }

    void OnEnable()
    {
        // Ensure cursor settings are applied when script is enabled
        ConfigureCursor();
    }

    void ConfigureCursor()
    {
        // Set cursor to always be visible and unlocked
        Cursor.visible = keepCursorVisible;
        Cursor.lockState = cursorLockMode;

        Debug.Log("Cursor configured - Visible: " + Cursor.visible + ", Lock State: " + Cursor.lockState);
    }

    public void SetPlayerChoice(int choiceIndex)
    {
        if (hasShot) return;

        playerChoice = (Choice)choiceIndex;
        playerChoiceText.text = "Player: " + playerChoice.ToString();
    }

    public void Shoot()
    {
        if (playerChoice == Choice.None) return;

        hasShot = true;
        GenerateBotChoice();
        CompareChoices();
    }

    void GenerateBotChoice()
    {
        // Make bot choice more random and less predictable
        botChoice = GetRandomBotChoice();

        // Update text and show sprite together
        UpdateBotDisplay();
    }

    Choice GetRandomBotChoice()
    {
        // 70% chance for random choice, 30% chance for strategic choice
        if (Random.value < 0.7f)
        {
            // Pure random choice (1-3)
            return (Choice)Random.Range(1, 4);
        }
        else
        {
            // Strategic choice - sometimes try to counter player's possible choices
            // But make it less obvious by not always countering
            return GetStrategicChoice();
        }
    }

    Choice GetStrategicChoice()
    {
        // Don't always counter - add some randomness to strategy
        if (playerChoice != Choice.None)
        {
            float randomFactor = Random.value;

            // 40% chance to choose the counter, 60% chance for random
            if (randomFactor < 0.4f)
            {
                // Choose the move that would beat the player's choice
                switch (playerChoice)
                {
                    case Choice.Rock:
                        return Choice.Paper; // Paper beats Rock
                    case Choice.Paper:
                        return Choice.Scissors; // Scissors beat Paper
                    case Choice.Scissors:
                        return Choice.Rock; // Rock beats Scissors
                }
            }
        }

        // If no strategy applied, return random
        return (Choice)Random.Range(1, 4);
    }

    void UpdateBotDisplay()
    {
        // Update text
        botChoiceText.text = "Bot: " + botChoice.ToString();

        // Hide all bot visuals first
        rockBot.Hide();
        paperBot.Hide();
        scissorsBot.Hide();

        // Add small delay before showing (optional - for dramatic effect)
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

    void CompareChoices()
    {
        string result = "";

        switch (playerChoice)
        {
            case Choice.Rock:
                if (botChoice == Choice.Scissors)
                {
                    result = "YOU WIN!";
                    playerWins++;
                }
                else if (botChoice == Choice.Paper)
                {
                    result = "YOU LOSE!";
                    botWins++;
                }
                else result = "DRAW!";
                break;

            case Choice.Paper:
                if (botChoice == Choice.Rock)
                {
                    result = "YOU WIN!";
                    playerWins++;
                }
                else if (botChoice == Choice.Scissors)
                {
                    result = "YOU LOSE!";
                    botWins++;
                }
                else result = "DRAW!";
                break;

            case Choice.Scissors:
                if (botChoice == Choice.Paper)
                {
                    result = "YOU WIN!";
                    playerWins++;
                }
                else if (botChoice == Choice.Rock)
                {
                    result = "YOU LOSE!";
                    botWins++;
                }
                else result = "DRAW!";
                break;
        }

        scoreText.text = "Score - Player: " + playerWins + " | Bot: " + botWins;
        resultText.text = result;

        // Store this bot choice for pattern avoidance
        lastBotChoices[choiceIndex % lastBotChoices.Length] = botChoice;
        choiceIndex++;

        CheckForWinner();
    }

    void CheckForWinner()
    {
        if (playerWins >= 3)
        {
            GoToScene("GameWin");
        }
        else if (botWins >= 3)
        {
            GoToScene("GameOver");
        }
        else
        {
            // Add delay before resetting so player can see the result
            Invoke("ResetRound", 1.5f);
        }
    }

    void GoToScene(string sceneName)
    {
        // Ensure cursor settings persist when loading new scene
        ConfigureCursor();
        SceneManager.LoadScene(sceneName);
    }

    public void ResetRound()
    {
        // Cancel any pending invoke
        CancelInvoke();

        playerChoice = Choice.None;
        botChoice = Choice.None;
        hasShot = false;

        playerChoiceText.text = "Player: ";
        botChoiceText.text = "Bot: ";
        resultText.text = "Choose and Press SHOOT";
        scoreText.text = "Score - Player: " + playerWins + " | Bot: " + botWins;

        rockBot.Hide();
        paperBot.Hide();
        scissorsBot.Hide();

        // Reapply cursor settings when round resets
        ConfigureCursor();
    }

    // Optional: Method to reset the entire game
    public void ResetGame()
    {
        playerWins = 0;
        botWins = 0;
        choiceIndex = 0;
        System.Array.Clear(lastBotChoices, 0, lastBotChoices.Length);
        ResetRound();
    }

    // Called when the application loses focus
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            // Reapply cursor settings when application regains focus
            ConfigureCursor();
        }
    }

    // Called when the application pauses (mobile) or loses focus
    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            // Reapply cursor settings when application resumes
            ConfigureCursor();
        }
    }
}
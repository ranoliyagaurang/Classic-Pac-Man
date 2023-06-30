using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Ready, Running, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Manager;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Canvas gameOver;
    [SerializeField] GameObject playBt;

    [Header("GameStatus")]
    [SerializeField] GameState gameState;

    public delegate void GameStateChanged(GameState state);
    public static event GameStateChanged StateChanged;

    private int score;

    private void Awake()
    {
        Manager = this;

        scoreText.text = "Score : " + score;
    }

    public void UpdateScore()
    {
        score++;

        scoreText.text = "Score : " + score;

        AudioManager.Manager.CollectSound();
    }

    public void GameOver()
    {
        gameState = GameState.GameOver;

        StateChanged(gameState);

        gameOver.enabled = true;

        AudioManager.Manager.PlayMusic(false);

        AudioManager.Manager.GameOverSound();
    }

    public void PlayAgainClicked()
    {
        SceneManager.LoadScene(0);

        AudioManager.Manager.ClickSound();
    }

    public void PlayClicked()
    {
        playBt.SetActive(false);

        gameState = GameState.Running;

        StateChanged(gameState);

        AudioManager.Manager.ClickSound();
    }
}
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {  get; private set; }
    public float CurrentScore = 0;
    public float Multiplayer = 1;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        GlobalGameManager.OnGameOver += TrySetHighScore;
        GlobalGameManager.OnGameRestarted += GameRestart;
    }

    private void OnDisable()
    {
        GlobalGameManager.OnGameOver -= TrySetHighScore;
        GlobalGameManager.OnGameRestarted -= GameRestart;
    }
    public void AddScore(int score)
    {
        CurrentScore += score * Multiplayer;
        CurrentScore = Mathf.Ceil(CurrentScore);
    }

    private void TrySetHighScore()
    {
        GlobalGameManager.Instance.SetHighScore(CurrentScore);
    }

    private void GameRestart()
    {
        CurrentScore = 0;
    }
}

using TMPro;
using UnityEngine;

public class InGameUIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _moneyText;

    [SerializeField] private RevivePanel _revivePanel;
    [SerializeField] private GameOverPanel _gameOverPanel;
    [SerializeField] private ShopWindow _shopWindow;
    [SerializeField] private PauseMenu _pauseMenu;

    private ScoreManager _score;

    private void OnEnable()
    {
        GlobalGameManager.OnFirstDie += ShowReviveView;
        GlobalGameManager.OnGameOver += ShowGameOverView;
        GlobalGameManager.OnGamePaused += ShowPauseWindow;
        GlobalGameManager.Instance.PlayerData.OnMoneyChanged += ShowMoney;
    }

    private void Start()
    {
        _score = ScoreManager.Instance;
        ShowMoney(GlobalGameManager.Instance.PlayerData.Money);
    }

    private void OnDisable()
    {
        GlobalGameManager.OnFirstDie -= ShowReviveView;
        GlobalGameManager.OnGameOver -= ShowGameOverView;
        GlobalGameManager.OnGamePaused -= ShowPauseWindow;
        GlobalGameManager.Instance.PlayerData.OnMoneyChanged -= ShowMoney;

    }

    private void Update()
    {
        _scoreText.text = _score.CurrentScore.ToString();
    }

    private void ShowReviveView()
    {
        Instantiate(_revivePanel, transform);
    }

    private void ShowGameOverView()
    {
        Instantiate(_gameOverPanel, transform);
    }

    private void ShowPauseWindow()
    {
        Instantiate(_pauseMenu, transform);
    }

    public void GoToShop()
    {
        Instantiate(_shopWindow, transform);
    }

    private void ShowMoney(int money)
    {
        _moneyText.text = money.ToString();
    }
}

using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _shopButton;

    private InGameUIView _uiView;

    private void OnEnable()
    {
        _uiView = FindFirstObjectByType<InGameUIView>();
        _restartButton.onClick.AddListener(RestartGame);
        _shopButton.onClick.AddListener(GoToShop);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(RestartGame);
        _shopButton.onClick.RemoveListener(GoToShop);
    }

    private void RestartGame()
    {
        GlobalGameManager.Instance.SetState(GlobalGameManager.States.Restart);
        Destroy(gameObject);
    }

    private void GoToShop()
    {
        _uiView.GoToShop();
    }
}

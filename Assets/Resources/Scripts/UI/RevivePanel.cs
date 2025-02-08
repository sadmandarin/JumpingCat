using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Agava.YandexGames;

public class RevivePanel : MonoBehaviour
{
    [SerializeField] private Button _reviveButton;
    [SerializeField] private TextMeshProUGUI _reviveTime;

    private int _time = 10;

    private void OnEnable()
    {
        _reviveButton.onClick.AddListener(Revive);

        StartCoroutine(ReviveTimer());
    }

    private void OnDisable()
    {
        _reviveButton.onClick.RemoveListener(Revive);
    }

    private void Revive()
    {
        GlobalGameManager.Instance.Revive();
        Destroy(gameObject);
    }

    private IEnumerator ReviveTimer()
    {
        _reviveTime.text = _time.ToString();
        while (_time > 0)
        {
            yield return new WaitForSeconds(1);

            _time -= 1;
            _reviveTime.text = _time.ToString();
        }
        GlobalGameManager.Instance.SetState(GlobalGameManager.States.Gameover);
        Destroy(gameObject);
    }
}

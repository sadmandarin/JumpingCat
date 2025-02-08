using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuySkinWindow : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private Image _skinIcon;

    private CatSkinItem _skin;

    public void Init(CatSkinItem skin)
    {
        _price.text = skin.Price.ToString();
        _skinIcon.sprite = skin.Icon;
        _skin = skin;
                
        _closeButton.onClick.AddListener(DestroyWindow);
        _buyButton.onClick.AddListener(BuySkin);
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveListener(DestroyWindow);
        _buyButton.onClick.RemoveListener(BuySkin);
    }

    private void DestroyWindow()
    {
        Destroy(gameObject);
    }

    private void BuySkin()
    {
        if (GlobalGameManager.Instance.PlayerData.BuySkin(_skin))
        {
            GlobalGameManager.Instance.SetSkin(_skin);

            Destroy(gameObject);
        }
        else
        {
            //Переход в окно покупки монет, если не хватает на покупку
        }
    }
}

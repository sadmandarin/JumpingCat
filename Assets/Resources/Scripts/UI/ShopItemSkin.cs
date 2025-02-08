using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSkin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private Image _icon;
    [SerializeField] private BuySkinWindow _buySkin;
    [SerializeField] private Image _outline;

    private Button _button;
    private CatSkinItem _skin;

    public void Init(CatSkinItem skin)
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ClickAction);

        GlobalGameManager.OnSkinChanged += ChangeFrameIfCurrent;
        GlobalGameManager.Instance.PlayerData.OnSkinBuyed += ChangeFrameIfBuyed;

        _name.text = skin.Name;
        _price.text = skin.Price.ToString();
        _icon.sprite = skin.Icon;
        _skin = skin;
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(ClickAction);
        GlobalGameManager.OnSkinChanged -= ChangeFrameIfCurrent;
        GlobalGameManager.Instance.PlayerData.OnSkinBuyed -= ChangeFrameIfBuyed;
    }

    private void ClickAction()
    {
        if (GlobalGameManager.Instance.PlayerData.IsSkinBuyed(_skin))
        {
            GlobalGameManager.Instance.SetSkin(_skin);
        }
        else
        {
           var skinWindow = Instantiate(_buySkin, FindFirstObjectByType<Canvas>().transform);
            skinWindow.Init(_skin);
        }
    }

    private void ChangeFrameIfCurrent(CatSkinItem skin)
    {
        if (skin == _skin)
            _outline.enabled = true;
        else
            _outline.enabled = false;
    }

    private void ChangeFrameIfBuyed(CatSkinItem skin)
    {
        if (skin == _skin)
        {
            // Добавить замену фрейма при покупке скина
        }
    }
}

using UnityEngine;

public class ShopWindow : MonoBehaviour
{
    [SerializeField] private StoreConfigSO _store;
    [SerializeField] private RectTransform _content;
    [SerializeField] private ShopItemSkin _shopItem;

    private void OnEnable()
    {
        FillShopMenu();
    }

    private void FillShopMenu()
    {
        foreach (var shopItem in _store.CatSkins)
        {
            var skin = Instantiate(_shopItem, _content);
            skin.Init(shopItem);
        }
    }
}

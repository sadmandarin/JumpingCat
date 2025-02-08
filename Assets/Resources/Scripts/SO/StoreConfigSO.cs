using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoreConfigSO", menuName = "Scriptable Objects/StoreConfigSO", order = 0)]
public class StoreConfigSO : ScriptableObject
{
    public List<CatSkinItem> CatSkins;
}

[System.Serializable]
public class CatSkinItem
{
    public string Name;
    public Sprite Icon;
    public int Price;
    public int PremiumPrice;
}

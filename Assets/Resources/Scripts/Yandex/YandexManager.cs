using Agava.YandexGames;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YandexManager : MonoBehaviour
{
    public static YandexManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        
        InitializeSdk();
    }

    private void InitializeSdk()
    {
        YandexGamesSdk.Initialize(() =>
        {
            Debug.Log("YandexSDK initialized");
            CheckAutorization();
        }
        );
    }

    private void CheckAutorization()
    {
        PlayerAccount.Authorize(() =>
        {
            Debug.Log("Player Authorized");
        });
    }

    public void SaveData(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerAccount.SetCloudSaveData(json, () =>
        {
            Debug.Log("Data saved");
        }, (error) =>
        {
            Debug.Log($"Error saving data{error}");
        });

    }

    public void LoadData(Action<SaveData> OnLoadedData)
    {
        PlayerAccount.GetCloudSaveData((data) =>
        {
            if (!string.IsNullOrEmpty(data))
            {
                SaveData loadedData = JsonUtility.FromJson<SaveData>(data);
                OnLoadedData?.Invoke(loadedData);
            }
            else
                OnLoadedData?.Invoke(null);
        }, (error) =>
        {
            OnLoadedData?.Invoke(null);
        });
    }

    public void ShowRewardedAd(Action OnRewardGranted)
    {
        
    }
}

[System.Serializable]
public class SaveData
{
    public int MaxScore;
    public int Money;
    public CatSkinItem CurrentSkin;
    public List<CatSkinItem> BuyedSkins;
}

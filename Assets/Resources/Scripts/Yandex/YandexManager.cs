using Agava.YandexGames;
using System;
using System.Collections.Generic;
using UnityEngine;

public class YandexManager : MonoBehaviour
{
    public static YandexManager Instance;

    private float _adCooldown = 60;
    private float _lastAdTime = -Mathf.Infinity; 

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
        VideoAd.Show(
            onOpenCallback: () => Debug.Log("Реклама открыта"),
            onRewardedCallback: () =>
            {
                Debug.Log("Награда получена");
                OnRewardGranted?.Invoke();
            },
            onCloseCallback: () => Debug.Log("Реклама закрыта"),
            onErrorCallback: (error) =>
            {
                Debug.Log($"Ошибка при вызове рекламы: {error}");
            }
            );
    }

    public void ShowRewardAtTime()
    {
        if (Time.time - _lastAdTime < _adCooldown)
            return;

        InterstitialAd.Show(
            onOpenCallback: () => Debug.Log("Реклама открыта"),
            onCloseCallback: (wasShown) =>
            {
                Debug.Log($"Реклама была показана {wasShown}");
                _lastAdTime = Time.time;
            },
            onErrorCallback: (errorMessage) =>
            {
                Debug.LogError($"Ошибка при показе полноэкранной рекламы: {errorMessage}");
            },
            onOfflineCallback: () =>
            {
                Debug.LogWarning("Полноэкранная реклама недоступна в офлайн-режиме.");
            });
    }

    public void TryShowAd()
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

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    private States _currentState;

    public static GlobalGameManager Instance { get; private set; }
    public PlayerData PlayerData { get; private set; }

    /// <summary>
    /// Event вызывается, после окончательной смерти
    /// </summary>
    public static event Action OnGameOver;
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;

    /// <summary>
    /// Event вызывается, когда есть возможность второй жизни за монеты/рекламу
    /// </summary>
    public static event Action OnFirstDie;

    /// <summary>
    /// Event вызывается на возрождении
    /// </summary>
    public static event Action OnRevive;
    /// <summary>
    /// Event вызывается при перезапуске игры
    /// </summary>
    public static event Action OnGameRestarted;
    public static event Action<CatSkinItem> OnSkinChanged;

    public enum States
    {
        Restart,
        Paused,
        Continue,
        Gameover
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        //YandexManager.Instance.LoadData((loadedData) =>
        //{
        //    if (loadedData != null) 
        //        PlayerData = new PlayerData(loadedData.MaxScore, loadedData.Money, loadedData.CurrentSkin, loadedData.BuyedSkins);
            
        //    else
        //        PlayerData = new PlayerData();
        //});

        PlayerData = new PlayerData();
    }

    public void SetHighScore(float score)
    {
        if (score > PlayerData.MaxScore)
            PlayerData.MaxScore = score;
    }

    public void SetSkin(CatSkinItem item)
    {
        if (PlayerData.CurrentSkin != item)
        {
            PlayerData.CurrentSkin = item;
            OnSkinChanged?.Invoke(item);
        }
    }

    public void BuySkin(CatSkinItem skin)
    {

    }

    public void SetState(States state)
    {
        _currentState = state;
        switch (_currentState)
        {
            case States.Restart:
                RestartGame();
                break;
            case States.Paused:
                Pause();
                break;
            case States.Continue:
                Continue();
                break;
            case States.Gameover:
                GameOver();
                break;
        }
    }

    private void Pause()
    {
        Time.timeScale = 0;
        OnGamePaused?.Invoke();
    }

    private void Continue()
    {
        Time.timeScale = 1;
        OnGameResumed?.Invoke();
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        OnGameOver?.Invoke();
    }

    public void FirstDead()
    {
        OnFirstDie?.Invoke();
    }

    public void Revive()
    {
        OnRevive?.Invoke();
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        OnGameRestarted?.Invoke();
    }
}

[System.Serializable]
public class PlayerData
{
    public float MaxScore;
    public int Money;
    public CatSkinItem CurrentSkin;
    public List<CatSkinItem> BuyedSkins;
    public float ImmortalTime = 2;

    public event Action<int> OnMoneyChanged;
    public event Action<CatSkinItem> OnSkinBuyed;

    public PlayerData()
    {
        MaxScore = 0;
        Money = 0;
        CurrentSkin = null;
        BuyedSkins = new List<CatSkinItem>();
    }

    public PlayerData(int maxScore, int money, CatSkinItem skin, List<CatSkinItem> skins)
    {
        MaxScore = maxScore;
        Money = money;
        CurrentSkin = skin;
        BuyedSkins = skins;
    }

    public bool BuySkin(CatSkinItem skin)
    {
        if (skin.Price < Money)
        {
            AddMoney(-skin.Price);
            BuyedSkins.Add(skin);
            OnSkinBuyed?.Invoke(skin);
            return true;
        }
        else
            return false;
        
    }

    public void AddMoney(int money)
    {
        Money += money;
        OnMoneyChanged?.Invoke(Money);
    }

    public bool IsSkinBuyed(CatSkinItem skin)
    {
        if (BuyedSkins.Contains(skin))
            return true;
        else
            return false;
    }
}

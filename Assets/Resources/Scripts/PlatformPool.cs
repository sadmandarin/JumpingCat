using System.Collections.Generic;
using UnityEngine;

public class PlatformPool : MonoBehaviour
{
    public static PlatformPool Instance {  get; private set; }

    public List<PlatformType> PlatformsTypes;
    private List<GameObject> _activePlatforms = new();

    private int _poolSize = 10;
    private Dictionary<GameObject, List<GameObject>> _platformPools;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }    
        else
            Destroy(gameObject);

        _platformPools = new Dictionary<GameObject, List<GameObject>>();

        foreach (var platform in PlatformsTypes)
        {
            List<GameObject> pool = new List<GameObject>();

            for (int i = 0; i < _poolSize; i++)
            {
                var poolPlatform = Instantiate(platform.platformPrefab, transform);
                poolPlatform.SetActive(false);
                pool.Add(poolPlatform);
            }
            _platformPools.Add(platform.platformPrefab, pool);

            Debug.Log($"Создан пул для {platform.platformPrefab.name}, размер: {pool.Count}");
        }
    }

    public GameObject GetPlatform(GameObject platformPrefab)
    {
        if (_platformPools.ContainsKey(platformPrefab))
        {
            foreach (var platform in _platformPools[platformPrefab])
            {
                if (!platform.activeInHierarchy)
                {
                    platform.SetActive(true);
                    _activePlatforms.Add(platform);
                    return platform;
                }
            }
            GameObject newPlatform = Instantiate(platformPrefab, transform);
            _platformPools[platformPrefab].Add(newPlatform);
            return newPlatform;
        }

        return null;
    }

    public void RemoveFromActive(GameObject platform)
    {
        _activePlatforms.Remove(platform);
    }
}

[System.Serializable]
public class PlatformType
{
    public GameObject platformPrefab;
    public float spawnWeight;
}
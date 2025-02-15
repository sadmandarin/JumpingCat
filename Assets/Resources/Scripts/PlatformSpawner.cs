using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnHeight; 
    [SerializeField] private float _minX = -4.5f, _maxX = 4.5f; 
    [SerializeField] private float _minYDistance, _maxYDistance;
    [SerializeField] private MoneyItem _moneyPrefab;

    private float lastPlatformY;

    private void Start()
    {
        lastPlatformY = transform.position.y;

        for (int i = 0; i < 5; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        if (Camera.main.transform.position.y + 10 > lastPlatformY)
        {
            SpawnPlatform();
        }
    }

    private void SpawnPlatform()
    {
        float xPos = Random.Range(_minX, _maxX);

        float yDistance = Random.Range(_minYDistance, _maxYDistance);
        lastPlatformY += yDistance;

        var randomPlatform = GetRandomPlatform();

        var platform = PlatformPool.Instance.GetPlatform(randomPlatform);

        if (platform != null)
        {
            platform.transform.position = new Vector3(xPos, lastPlatformY);
        }

        if (Random.value < 0.2 && !platform.TryGetComponent(out BreakablePlatform _))
        {
            SpawnMoneyAbovePlatform(platform.transform);
        }
    }

    private void SpawnMoneyAbovePlatform(Transform platformTransform)
    {
        var spawnPos = platformTransform.position + new Vector3(0, 0.5f, 0);

        var money = Instantiate(_moneyPrefab, spawnPos, Quaternion.identity);
        money.Init(platformTransform);
    }

    private GameObject GetRandomPlatform()
    {
        var platforms = PlatformPool.Instance.PlatformsTypes;
        
        var totalWeight = 0f;

        foreach (var platform in platforms)
        {
            totalWeight += platform.spawnWeight;
        }

        var random = Random.Range(0, totalWeight);

        foreach (var platform in platforms)
        {
            if (random < platform.spawnWeight)
            {
                return platform.platformPrefab;
            }
            random -= platform.spawnWeight;
        }

        return platforms[0].platformPrefab;
    }
}


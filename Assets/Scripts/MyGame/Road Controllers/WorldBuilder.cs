using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class WorldBuilder : SingletonBehaviour<WorldBuilder>
{
    private Transform _lastPlatform = null;

    private bool _isObstacle;
    private bool _isCross;

    private Random _random;
    private void OnEnable()
    {
        RoadEnd.onRoadEnd += CreatePlatform;
        RoadEnd.onRoadEnd += ReturnToPool;
    }

    private void OnDestroy()
    {
        RoadEnd.onRoadEnd -= CreatePlatform;
        RoadEnd.onRoadEnd -= ReturnToPool;
    }

    private void Awake()
    {
        InitializeSingleton();
    }

    public void Init(int seed)
    {
        _random = new Random(seed);
        
        CreateFreePlatform();
        CreateObstaclePlatform();
        CreateObstaclePlatform();
        CreateCrossPlatform();
        for (int i = 0; i < 5; i++)
        {
            CreatePlatform(null);
        }
    }

    public void CreatePlatform(PoolItem nothing)
    {
        if (_isObstacle)
            CreateFreePlatform();
        else if(_isCross)
            CreateObstaclePlatform();
        else
        {
            if (_random.Next(0, 100) <= 50f)
                CreateCrossPlatform();
            else
                CreateObstaclePlatform();
        }
    }

    private void CreateBasePlatform(PoolType platformType)
    {
        Transform endPoint = (_lastPlatform == null) ? transform : _lastPlatform.GetComponent<RoadBlockController>().endPoint;
        Vector3 pos = (_lastPlatform == null) ? transform.position : endPoint.position;

        PoolItem result = PoolManager.Get(platformType);
        
        _lastPlatform = SetSpawnSettings(result, pos, endPoint);
    }

    private void CreateFreePlatform()
    {
        CreateBasePlatform(PoolType.RoadStraight);
        
        _isObstacle = false;
    }

    private void CreateObstaclePlatform()
    {
        CreateBasePlatform(PoolType.RoadStraight);
        
        _lastPlatform.GetComponent<RoadBlockController>().GenerateObstacles(_random.Next());
        
        _isObstacle = true;
        _isCross = false;
    }

    private void CreateCrossPlatform()
    {
        CreateBasePlatform(_random.Next(0, 100) <= 50f ? PoolType.RoadBendLeft : PoolType.RoadBendRight);

        _isCross = true;
        _isObstacle = false;
    }

    private Transform SetSpawnSettings(PoolItem result, Vector3 pos, Transform endPoint)
    {
        Transform resultTransform = result.gameObject.transform;
        
        resultTransform.SetParent(transform);
        resultTransform.position = pos;
        resultTransform.rotation = endPoint.rotation;
        
        result.gameObject.SetActive(true);

        return resultTransform;
    }

    private void ReturnToPool(PoolItem poolItem)
    { 
        PoolManager.Return(poolItem);
    }
}

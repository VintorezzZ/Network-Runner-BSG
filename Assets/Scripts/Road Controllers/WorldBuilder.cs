using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldBuilder : MonoBehaviour
{
    private Transform _lastPlatform = null;

    private bool _isObstacle;
    private bool _isCross;
    
    private void OnEnable()
    {
        Destroyer.onRoadEnds += CreatePlatform;
        Destroyer.onRoadEnds += ReturnToPool;
    }

    private void OnDestroy()
    {
        Destroyer.onRoadEnds -= CreatePlatform;
        Destroyer.onRoadEnds -= ReturnToPool;
    }

    void Start()
    {
        Init();
    }

    public void Init()
    {
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
            if (Random.Range(0, 101) < 50)
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
        
        _lastPlatform.GetComponent<RoadBlockController>().GenerateObstacles();
        
        _isObstacle = true;
        _isCross = false;
    }

    private void CreateCrossPlatform()
    {
        CreateBasePlatform(PoolType.RoadBend);
        
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

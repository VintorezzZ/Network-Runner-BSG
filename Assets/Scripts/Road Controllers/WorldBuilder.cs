using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldBuilder : MonoBehaviour
{
    public Transform platformContainer;

    private Transform _lastPlatform = null;

    private bool _isObstacle;
    private bool _isCross;
    
    private void OnEnable()
    {
        Destroyer.spawnNewRoad += CreatePlatform;
    }

    private void OnDisable()
    {
        Destroyer.spawnNewRoad -= CreatePlatform;
    }

    void Start()
    {
        Init();
    }

    public void Init()
    {
        CreateFreePlatform();

        for (int i = 0; i < 10; i++)
        {
            CreatePlatform();
        }
    }

    public void CreatePlatform()
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
        Transform endPoint = (_lastPlatform == null) ? platformContainer : _lastPlatform.GetComponent<RoadBlockController>().endPoint;
        Vector3 pos = (_lastPlatform == null) ? platformContainer.position : endPoint.position;

        PoolItem result = Pool.Get(platformType);
        SetSpawnSettings(result, pos, endPoint);

        _lastPlatform = result.transform;
        
        result.gameObject.SetActive(true);
    }

    private void CreateFreePlatform()
    {
        CreateBasePlatform(PoolType.RoadStraight);
        
        _isObstacle = false;
    }

    private void CreateObstaclePlatform()
    {
        CreateBasePlatform(PoolType.RoadStraight);
        
        //ObstacleGenerator.GenerateObstacles(_lastPlatform.gameObject);
        
        _isObstacle = true;
        _isCross = false;
    }

    private void CreateCrossPlatform()
    {
        CreateBasePlatform(PoolType.RoadBend);
        
        _isCross = true;
        _isObstacle = false;
    }

    private void SetSpawnSettings(PoolItem result, Vector3 pos, Transform endPoint)
    {
        Transform resultTransform = result.gameObject.transform;
        
        resultTransform.position = pos;
        resultTransform.rotation = endPoint.rotation;
        
        result.gameObject.SetActive(true);
    }
}


internal class ObstacleGenerator
{
    public static void GenerateObstacles(GameObject platform)
    {
       Transform[] obstaclePoints = platform.GetComponent<RoadBlockController>().obstaclePoints;
       
       if (obstaclePoints.Length > 0)
       {
           for (int i = 0; i < obstaclePoints.Length; i++)
           {
               if (Random.Range(0, 101) < 50)
               {
                   obstaclePoints[i].gameObject.SetActive(true);
               }
           }
       }
    }
}

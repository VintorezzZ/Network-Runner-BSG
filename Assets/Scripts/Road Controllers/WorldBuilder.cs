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

    private string RoadStraight = "RoadStraight";
    private string RoadBend = "RoadBend";
    
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

    private void CreateBasePlatform(String platformType)
    {
        Transform endPoint = (_lastPlatform == null) ? platformContainer : _lastPlatform.GetComponent<RoadBlockController>().endPoint;
        Vector3 pos = (_lastPlatform == null) ? platformContainer.position : endPoint.position;

        GameObject result = Pool.singleton.Get(platformType);
        SetSpawnSettings(result, pos, endPoint);
        
        _lastPlatform = result.transform;
    }

    private void CreateFreePlatform()
    {
        CreateBasePlatform(RoadStraight);
        
        _isObstacle = false;
    }

    private void CreateObstaclePlatform()
    {
        CreateBasePlatform(RoadStraight);
        
        ObstacleGenerator.GenerateObstacles(_lastPlatform.gameObject);
        
        _isObstacle = true;
        _isCross = false;
    }

    private void CreateCrossPlatform()
    {
        CreateBasePlatform(RoadBend);
        
        _isCross = true;
        _isObstacle = false;
    }

    private static void SetSpawnSettings(GameObject result, Vector3 pos, Transform endPoint)
    {
        result.transform.position = pos;
        result.transform.rotation = endPoint.rotation;
        result.SetActive(true);
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

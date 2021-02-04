using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadBlockController : MonoBehaviour, IPoolObservable
{
    public Transform endPoint;
    public Transform[] obstaclePoints;
    public List<Obstacle> pooledObstacles;

    private PoolItem poolItem;
    private Destroyer[] destroyers;
    Transform generatedObstacles;

    private void Awake()
    {
        CreateObstaclesContainer();
    }

    private void Start()
    {
       poolItem = GetComponent<PoolItem>();
       destroyers = GetComponentsInChildren<Destroyer>();

       foreach (Destroyer destroyer in destroyers)
       {
           destroyer.parentPoolItem = poolItem;
       }
    }

    private void CreateObstaclesContainer()
    {
        generatedObstacles = new GameObject("GeneratedObstacles").transform;
        generatedObstacles.SetParent(transform);
    }

    public void GenerateObstacles()
    {
        if (obstaclePoints.Length > 0)
        {
            
            for (int i = 0; i < obstaclePoints.Length; i++)
            {
                if (i % 2 == 0)
                {
                    Obstacle roadItem = null;

                    roadItem = GetRoadItem();

                    roadItem.onReturnToPool += RemoveObstacleFromList;

                    roadItem.transform.SetParent(generatedObstacles);
                    roadItem.transform.position = obstaclePoints[i].position;
                    roadItem.transform.rotation = obstaclePoints[i].rotation;
                    
                    roadItem.gameObject.SetActive(true);

                    pooledObstacles.Add(roadItem);
                }
            }
        }
    }

    private static Obstacle GetRoadItem()
    {
        Obstacle roadItem;
        
        if (Random.Range(0, 101) < 30)
            roadItem = PoolManager.Get(PoolType.Bonuses).GetComponent<Obstacle>();
        else
            roadItem = PoolManager.Get(PoolType.Obstacles).GetComponent<Obstacle>();

        return roadItem;
    }

    public void OnReturnToPool()
    {
        ReturnObstaclesToPool();
    }

    public void ReturnObstaclesToPool()
    {
        foreach (var obst in pooledObstacles)
        {
            obst.onReturnToPool -= RemoveObstacleFromList;
            obst.RemoveObstacle();
        }

        pooledObstacles.Clear();
    }

    private void RemoveObstacleFromList(Obstacle obst)
    {
        obst.onReturnToPool -= RemoveObstacleFromList;
        pooledObstacles.Remove(obst);
    }

    public void OnTakeFromPool()
    {

    }
}

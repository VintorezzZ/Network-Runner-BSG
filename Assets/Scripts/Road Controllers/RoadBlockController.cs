using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadBlockController : MonoBehaviour
{
    public Transform endPoint;
    public Transform[] obstaclePoints;
    public List<PoolItem> pooledObstacles;

    private PoolItem poolItem;
    private Destroyer[] destroyers;
    public bool hasObstacles { get; private set; } = false;

    private void Awake()
    { 
       poolItem = GetComponent<PoolItem>();
       destroyers = GetComponentsInChildren<Destroyer>();

       foreach (Destroyer destroyer in destroyers)
       {
           destroyer.parentPoolItem = poolItem;
       }
    }

    public void GenerateObstacles()
    {
        if (obstaclePoints.Length > 0)
        {
            hasObstacles = true;
            
            for (int i = 0; i < obstaclePoints.Length; i++)
            {
                if (i % 2 == 0)
                {
                    PoolItem obst = PoolManager.Get(PoolType.Obstacles);
                    obst.gameObject.SetActive(true);
                    obst.transform.position = obstaclePoints[i].position;
                    obst.transform.rotation = obstaclePoints[i].rotation;
                    
                    pooledObstacles.Add(obst);
                }
            }
        }
    }


    public void ReturnObstaclesToPool()
    {
        foreach (var obst in pooledObstacles)
        {
            PoolManager.Return(obst);
        }

        hasObstacles = false;
    }
}

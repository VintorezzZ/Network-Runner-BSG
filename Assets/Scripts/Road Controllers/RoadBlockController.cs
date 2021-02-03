using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadBlockController : MonoBehaviour, IPoolObservable
{
    public Transform endPoint;
    public Transform[] obstaclePoints;
    public List<PoolItem> pooledObstacles;

    private PoolItem poolItem;
    private Destroyer[] destroyers;
    public bool hasObstacles { get; private set; } = false;

    private void Start()
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
                    PoolItem roadItem = null;

                    if (Random.Range(0, 101) < 30) 
                    {
                        roadItem = PoolManager.Get(PoolType.Bonuses);
                    }
                    else
                    {
                        roadItem = PoolManager.Get(PoolType.Obstacles); 
                    }
                    
                    roadItem.gameObject.SetActive(true);
                    roadItem.transform.position = obstaclePoints[i].position;
                    roadItem.transform.rotation = obstaclePoints[i].rotation;
                    
                    pooledObstacles.Add(roadItem);
                }
            }
        }
    }

    public void ReturnObstaclesToPool()
    {
        foreach (var obst in pooledObstacles)
        {
            if (!obst.isFree)  // somewhere in the world
            {
                PoolManager.Return(obst);
            }
        }

        pooledObstacles.Clear();
        hasObstacles = false;
    }
    

    public void OnReturnToPool()
    {
        if (hasObstacles)
        {
            ReturnObstaclesToPool(); 
        }
    }

    public void OnTakeFromPool()
    {

    }
}

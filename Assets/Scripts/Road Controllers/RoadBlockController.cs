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
    Transform generatedObstacles;
    private void Start()
    {
       generatedObstacles = new GameObject("GeneratedObstacles").transform;
        generatedObstacles.SetParent(transform);
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
                    roadItem.transform.SetParent(generatedObstacles);

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
            PoolManager.Return(obst);
        }

        pooledObstacles.Clear();
    }
    

    public void OnReturnToPool()
    {
        ReturnObstaclesToPool(); 
    }

    public void OnTakeFromPool()
    {

    }
}

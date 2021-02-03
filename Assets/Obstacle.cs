using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IPoolObservable
{
    public event Action<Obstacle> onReturnToPool;

    private PoolItem poolItem;

    private void Start()
    {
        poolItem = GetComponent<PoolItem>();
    }

    public void RemoveObstacle()
    {
        PoolManager.Return(poolItem);
    }
    public void OnReturnToPool()
    {
        onReturnToPool?.Invoke(GetComponent<Obstacle>());  
    }

    public void OnTakeFromPool()
    {
        
    }

   
}

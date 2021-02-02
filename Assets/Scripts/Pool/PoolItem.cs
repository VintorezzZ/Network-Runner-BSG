using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItem : MonoBehaviour
{
    public bool isFree { get; private set; }
    public PoolType PoolType { get; private set; }

    public void Init(PoolType poolType)
    {
        isFree = true;
        PoolType = poolType;
    }

    public void TakeFromPool()
    {
        isFree = false;
    }

    public void ReturnToPool()
    {
        isFree = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pool : MonoBehaviour
{
    public static Pool Instance;

    //public List<PoolContainer> items;
    //public List<GameObject> pooledItems;
    private Dictionary<PoolType, PoolContainer> _pools = new Dictionary<PoolType, PoolContainer>();

    void Awake()
    {
        Instance = this;


        // пройти по всем дочерним контейнерам, добав в дикт и вызыв у него  инит. там он делает что-то, вешает на себя пул итем.
        foreach (PoolContainer poolContainer in GetComponentsInChildren<PoolContainer>())
        {
            _pools.Add(poolContainer.poolType, poolContainer);
            poolContainer.Init();
        }

        //pooledItems = new List<GameObject>();

    }

    public static PoolItem Get(PoolType poolType)
    {
        if (!Instance._pools.ContainsKey(poolType))
        {
            Debug.LogError("Unknown pool name: " + poolType);
            return null;
        }

        return Instance._pools[poolType].TakeFromPool();
    }

    public static void Return(PoolItem item)
    {
        if (!Instance._pools.ContainsKey(item.PoolName))
        {
            Debug.LogError("Unknown pool name: " + item.PoolName);
            return;
        }

        Instance._pools[item.PoolName].ReturnToPool(item);
    }










    // for (int i = 0; i < pooledItems.Count; i++)
    // {
    //     if(!pooledItems[i].activeInHierarchy && pooledItems[i].tag == tag)  //проверять не по иерархии, а по флагу в скрипте
    //     {
    //         return pooledItems[i];
    //     }
    // }
    //
    // for (int i = 0; i < items.Count; i++)
    // {
    //     for (int j = 0; j < items[i].prefabs.Count; j++)
    //     {
    //         if(items[i].prefabs[j].tag == tag && items[i].expandable)
    //         {
    //             GameObject obj = Instantiate(items[i].prefabs[j]);
    //             obj.SetActive(false);
    //             pooledItems.Add(obj);
    //             return obj;
    //         }
    //     }
    // }
    //
    // return null;
//}

public void ReturnToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public IEnumerator ReturnToPool(GameObject gameObject, float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}

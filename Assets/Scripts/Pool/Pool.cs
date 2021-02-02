using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pool : MonoBehaviour
{
    public static Pool Instance;

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
        if (!Instance._pools.ContainsKey(item.PoolType))
        {
            Debug.LogError("Unknown pool name: " + item.PoolType);
            return;
        }

        Instance._pools[item.PoolType].ReturnToPool(item);
    }


    #region expand code

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

    #endregion

    
    public void ReturnToPool(PoolItem poolItem)
    {
        poolItem.gameObject.SetActive(false);
        Return(poolItem);
    }

    public IEnumerator ReturnToPool(PoolItem poolItem, float time)
    {
        yield return new WaitForSeconds(time);
        Return(poolItem);
        poolItem.gameObject.SetActive(false);
    }
}

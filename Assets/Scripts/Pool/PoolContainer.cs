using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolContainer : MonoBehaviour  // container
{
    public PoolType poolType;
    public bool expandable;
    public List<GameObject> prefabs;
    public List<PoolItem> pooledItems;
    public void Init()
    {
        foreach (GameObject prefab in prefabs)
        {
            GameObject obj = Instantiate(prefab, gameObject.transform, true);
            obj.SetActive(false);
            obj.AddComponent<PoolItem>();
            pooledItems.Add(obj.GetComponent<PoolItem>());
        }
    }

    public PoolItem TakeFromPool()
    {
        for (int i = 0; i < pooledItems.Count; i++)
        {
            if(!pooledItems[i].isActive)  //проверять не по иерархии, а по флагу в скрипте
            {
                return pooledItems[i];
            }
        }

        return null;
    }

    public void ReturnToPool(PoolItem item)
    {
        
    }
}



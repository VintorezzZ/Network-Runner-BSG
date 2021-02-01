using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolItem
{
    //public GameObject prefab;
    public List<GameObject> prefabs;
    public int amount;
    public bool expandable;
}

public class Pool : MonoBehaviour 
{
    public static Pool singleton;
    public List<PoolItem> items;
    public List<GameObject> pooledItems;

    void Awake()
    {
        singleton = this;
    }

    // Use this for initialization
    void Start() 
    {
        pooledItems = new List<GameObject>();
        foreach(PoolItem item in items)
        {
            for (int i = 0; i < item.amount; i++)
            {
                for (int j = 0; j < item.prefabs.Count; j++)
                {
                    GameObject obj = Instantiate(item.prefabs[j], gameObject.transform, true);
                    obj.SetActive(false);
                    pooledItems.Add(obj);   
                }
            }
        }
    }
    public GameObject Get(string tag)
    {
        for (int i = 0; i < pooledItems.Count; i++)
        {
            if(!pooledItems[i].activeInHierarchy && pooledItems[i].tag == tag)
            {
                return pooledItems[i];
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            for (int j = 0; j < items[i].prefabs.Count; j++)
            {
                if(items[i].prefabs[j].tag == tag && items[i].expandable)
                {
                    GameObject obj = Instantiate(items[i].prefabs[j]);
                    obj.SetActive(false);
                    pooledItems.Add(obj);
                    return obj;
                }
            }
        }

        return null;
    }

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

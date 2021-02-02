using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolContainer : MonoBehaviour  // container
{
    public PoolType poolType;
    public int amount;
    public bool expandable;
    public List<GameObject> examplePrefabs; // only for set
    public List<PoolItem> pooledItems;
    public void Init()
    {
        for (int i = 0; i < amount; i++)
        {
            for (int j = 0; j < examplePrefabs.Count; j++)
            {
                GameObject obj = Instantiate(examplePrefabs[j], gameObject.transform, true); // создаем объект
                
                SetPooledItemSettings(obj);
            }
        }
    }

    private void SetPooledItemSettings(GameObject item)
    {
        item.SetActive(false); // деактивируем 
        PoolItem objPoolItem = item.AddComponent<PoolItem>(); // добавляем на каждый объект контракт 
        objPoolItem.Init(poolType); // вызываем инициализацию в объекте
        pooledItems.Add(objPoolItem); // добавляем в массив созданных объектов
    }

    public PoolItem TakeFromPool()
    {
        for (int i = 0; i < pooledItems.Count; i++)
        {
            if(pooledItems[i].isFree)  //проверять не по иерархии, а по флагу в скрипте
            {
                pooledItems[i].TakeFromPool();  // меняем флаг у объекта
                return pooledItems[i]; // возврашаем объект
            }
        }

        if (expandable)
        {
            GameObject obj = Instantiate(examplePrefabs[0], gameObject.transform, true); // создаем объект
            
            SetPooledItemSettings(obj);

            PoolItem lastAddedItem = pooledItems[pooledItems.Count - 1];  // находим последний добавленный элемент
            lastAddedItem.TakeFromPool();  // меняем флаг у последнего объекта
            return lastAddedItem; // возврашаем объект
        }
        
        return null;
    }

    public void ReturnToPool(PoolItem item)
    {
        item.gameObject.SetActive(false);
        item.ReturnToPool();
    }
}



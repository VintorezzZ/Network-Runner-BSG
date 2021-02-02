using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolContainer : MonoBehaviour  // container
{
    public PoolType poolType;
    public int amount;
    //public bool expandable;
    public List<GameObject> examplePrefabs; // only for set
    public List<PoolItem> pooledItems;
    public void Init()
    {
        for (int i = 0; i < amount; i++)
        {
            for (int j = 0; j < examplePrefabs.Count; j++)
            {
                GameObject obj = Instantiate(examplePrefabs[j], gameObject.transform, true); // создаем объект
                obj.SetActive(false); // деактивируем 
                PoolItem objPoolItem = obj.AddComponent<PoolItem>(); // добавляем на каждый объект контракт 
                objPoolItem.Init(poolType);  // вызываем инициализацию в объекте
                pooledItems.Add(objPoolItem);  // добавляем в массив созданных объектов
            }
        }
    }

    public PoolItem TakeFromPool()
    {
        for (int i = 0; i < pooledItems.Count; i++)
        {
            //print("pool count: " + pooledItems.Count);
            if(pooledItems[i].isFree)  //проверять не по иерархии, а по флагу в скрипте
            {
                pooledItems[i].TakeFromPool();  // меняем флаг у объекта
                return pooledItems[i]; // возврашаем объект
            }
        }
        //print("not take");
        return null;
    }

    public void ReturnToPool(PoolItem item)
    {
        item.ReturnToPool();
    }
}



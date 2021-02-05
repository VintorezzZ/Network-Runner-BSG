using System;
using System.Collections;
using UnityEngine;

public class Shell : MonoBehaviour, IPoolObservable
{
   private float speed = 0.2f;
   private PoolItem poolItem;

   public float playerVelocity { get; set; }
  
   public void Init()
   {
      poolItem = GetComponent<PoolItem>();
   }

   private void OnEnable()
   {
      StartCoroutine(ReturnToPool(2));
   }

   private void Update()
   {
      transform.Translate(-Vector3.forward * speed, Space.Self);
   }

   private float PlayerVelocity()
   {
      return playerVelocity * 0.1f;
   }


   private IEnumerator ReturnToPool(float time)
   {
      yield return new WaitForSeconds(time);
      PoolManager.Return(poolItem);
   }

   public void OnReturnToPool()
   {
      StopCoroutine(ReturnToPool(2));
   }

   public void OnTakeFromPool()
   {
      //StartCoroutine(ReturnToPool(2));
   }
}

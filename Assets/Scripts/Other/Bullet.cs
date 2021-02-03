using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   private float speed = 0.3f;
   private PoolItem poolItem;

   private void Start()
   {
      poolItem = GetComponent<PoolItem>();

      Invoke(nameof(ReturnToPool), 2);
   }

   private void Update()
   {
      transform.Translate(Vector3.forward * speed, Space.Self);
   }

   private void ReturnToPool()
   {
      PoolManager.Return(poolItem);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Obstacle"))
      {
         CancelInvoke();
         
         PoolManager.Return(other.gameObject.GetComponent<PoolItem>());

         ReturnToPool();
      }
   }
}

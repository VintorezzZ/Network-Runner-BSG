using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolObservable
{
   private float speed = 0.3f;
   private PoolItem poolItem;

   public float playerVelocity { get; set; }
   private void Start()
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
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Obstacle"))
      {
         PoolManager.Return(other.gameObject.GetComponent<PoolItem>());

         PoolManager.Return(poolItem);
      }
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

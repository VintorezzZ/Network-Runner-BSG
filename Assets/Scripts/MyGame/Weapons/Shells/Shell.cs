using System;
using System.Collections;
using UnityEngine;

public class Shell : MonoBehaviour, IPoolObservable
{
   private float speed = 50f;
   private PoolItem poolItem;
   private Rocket rocket;
   private Vector3 targetDirection;
   public float PlayerVelocity { get; set; }

   public void Init(Vector3 targetDir)
   {
      if(!poolItem)
         poolItem = GetComponent<PoolItem>();

      if (!rocket)
         rocket = GetComponentInChildren<Rocket>();

      targetDirection = targetDir;
      rocket.Init(poolItem);
   }

   private void OnEnable()
   {
      StartCoroutine(ReturnToPool(2));
   }

   private void Update()
   {
      transform.localPosition += -targetDirection * speed * Time.deltaTime;
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

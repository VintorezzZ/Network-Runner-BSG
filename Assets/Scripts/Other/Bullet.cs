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
      
      StartCoroutine(Pool.Instance.ReturnToPool(gameObject.GetComponent<PoolItem>(), 2));
   }

   private void Update()
   {
      //transform.localPosition += new Vector3(0, 0, speed);
      transform.Translate(Vector3.forward * speed, Space.Self);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Obstacle"))
      {
         //Pool.Instance.ReturnToPool(other.gameObject.GetComponent<PoolItem>());
         Pool.Instance.ReturnToPool(poolItem);
      }
   }
}

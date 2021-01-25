using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   private float speed = 0.3f;

   private void Start()
   {
      Destroy(gameObject, 2f);
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
         Destroy(other.gameObject);
         Destroy(gameObject);
      }
   }
}

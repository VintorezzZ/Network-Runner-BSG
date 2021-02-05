using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private void OnEnable()
    {
        // start Move
    }

    private void OnDisable()
    {
        // Stop Move
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable iDamageable))
        {
            iDamageable.TakeDamage();
        }
      
        PoolManager.Return(GetComponentInParent<PoolItem>());
    }
}

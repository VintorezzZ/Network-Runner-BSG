using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private PoolItem poolItem;
    private void Start()
    {
        poolItem = GetComponentInParent<PoolItem>();
    }

    private void OnEnable()
    {
        // start Move
    }

    private void OnDisable()
    {
        // Stop Move
    }


    private void OnCollisionEnter(Collision other)
    {
        // print(other.gameObject.name);
        // if (other.gameObject.TryGetComponent(out IDamageable iDamageable))
        // {
        //     print("get");
        //     iDamageable.TakeDamage();
        // }
        
        //Physics.OverlapSphere()
        
        Transform explosionFX = PoolManager.Get(PoolType.ExplosionsFX).transform;

        SetExplosionFXSettings(explosionFX, other);
        
        PoolManager.Return(poolItem);
    }

    private void SetExplosionFXSettings(Transform explosion, Collision collision)
    {
        explosion.position = collision.transform.position;
        explosion.gameObject.SetActive(true);
    }
}

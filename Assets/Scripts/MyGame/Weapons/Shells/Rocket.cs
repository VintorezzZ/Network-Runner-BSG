using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour, IPoolObservable
{
    private PoolItem poolItem;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform targetRotation;
    
    public void Init(PoolItem parentPoolItem)
    {
        if(!poolItem)
            poolItem = parentPoolItem;
    }

    private void Update()
    {
        transform.RotateAround(targetRotation.position, transform.forward, 520 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        Collider[] colliders;
        colliders = Physics.OverlapSphere(other.transform.position, 5);

        foreach (var coll in colliders)
        {
            if (coll.gameObject.TryGetComponent(out IDamageable iDamageable))
            {
                iDamageable.TakeDamage();
            }
        }
        
        Transform explosionFX = PoolManager.Get(PoolType.ExplosionsFX).transform;

        SetExplosionFXSettings(explosionFX, other);
       
        PoolManager.Return(poolItem);
    }

    private void SetExplosionFXSettings(Transform explosion, Collision collision)
    {
        explosion.position = collision.transform.position;
        explosion.gameObject.SetActive(true);
    }


    public void OnReturnToPool()
    {
        trailRenderer.enabled = false;
        trailRenderer.emitting = false;
    }

    public void OnTakeFromPool()
    {
        trailRenderer.enabled = true;
        trailRenderer.emitting = true;
    }
}

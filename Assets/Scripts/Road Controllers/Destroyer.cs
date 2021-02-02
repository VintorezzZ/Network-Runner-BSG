using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public delegate void SpawnNewRoad();
    public static event SpawnNewRoad spawnNewRoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawnNewRoad?.Invoke();
            //Destroy(transform.parent.gameObject, 2f);
            StartCoroutine(Pool.Instance.ReturnToPool(transform.parent.gameObject, 2));
        }
    }
}

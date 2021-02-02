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

            StartCoroutine(Pool.Instance.ReturnToPool(transform.parent.gameObject.GetComponent<PoolItem>(), 2));
        }
    }
}

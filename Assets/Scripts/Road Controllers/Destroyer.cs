using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public delegate void OnRoadEnds(PoolItem poolItem);
    public static event OnRoadEnds onRoadEnds; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke(nameof(SendMessage), 1);
        }
    }

    private void SendMessage()
    {
        onRoadEnds?.Invoke(transform.parent.gameObject.GetComponent<PoolItem>());
    }
}

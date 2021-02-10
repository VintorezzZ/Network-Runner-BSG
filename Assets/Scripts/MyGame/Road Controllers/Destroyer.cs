using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public delegate void OnRoadEnds(PoolItem poolItem);
    public static event OnRoadEnds onRoadEnds;

    public PoolItem parentPoolItem;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().photonView.IsMine)
            {
                Invoke(nameof(ActivateEvent), 1);
            }
        }
    }

    private void ActivateEvent()
    {
        onRoadEnds?.Invoke(parentPoolItem);
    }
}

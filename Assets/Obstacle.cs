using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IPoolObservable
{
    public event System.Action asd;
    public void OnReturnToPool()
    {
        Debug.LogError("OnReturnToPool");
    }

    public void OnTakeFromPool()
    {
        Debug.LogError("OnTakeFromPool");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

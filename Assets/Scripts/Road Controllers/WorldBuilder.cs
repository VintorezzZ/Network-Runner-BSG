using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldBuilder : MonoBehaviour
{
    public GameObject[] freePlatforms;
    public GameObject[] obstaclePlatforms;
    public GameObject[] crossPlatforms;
    public Transform platformContainer;

    private Transform _lastPlatform = null;

    private bool _isObstacle;
    private bool _isCross;

    private void OnEnable()
    {
        Destroyer.spawnNewRoad += CreatePlatform;
    }

    private void OnDisable()
    {
        Destroyer.spawnNewRoad -= CreatePlatform;
    }

    void Start()
    {
        Init();
    }

    public void Init()
    {
        CreateFreePlatform(0);
        //CreateFreePlatform();
        //CreateFreePlatform();

        for (int i = 0; i < 10; i++)
        {
            CreatePlatform();
        }
    }

    public void CreatePlatform()
    {
        if (_isObstacle)
            CreateFreePlatform();
        else if(_isCross)
            CreateObstaclePlatform();
        else
        {
            if (Random.Range(0, 101) < 50)
                CreateCrossPlatform();
            else
                CreateObstaclePlatform();
        }
    }

    private void CreateFreePlatform()
    {
        Transform endPoint = _lastPlatform.GetComponent<RoadBlockController>().endPoint;
        Vector3 pos = (_lastPlatform == null) ? platformContainer.position : endPoint.position;
        
        int index = Random.Range(0, freePlatforms.Length);
        GameObject result = Instantiate(freePlatforms[index], pos, endPoint.rotation, platformContainer);
        _lastPlatform = result.transform;
        _isObstacle = false;
    }
    private void CreateFreePlatform(int index)
    {
        Vector3 pos = (_lastPlatform == null) ? platformContainer.position : _lastPlatform.GetComponent<RoadBlockController>().endPoint.position;
                        
        GameObject result = Instantiate(freePlatforms[index], pos, Quaternion.identity, platformContainer);
        _lastPlatform = result.transform;
        _isObstacle = false;
        _isCross = false;
    }

    private void CreateObstaclePlatform()
    {
        Transform endPoint = _lastPlatform.GetComponent<RoadBlockController>().endPoint;
        Vector3 pos = (_lastPlatform == null) ? platformContainer.position : endPoint.position;

        int index = Random.Range(0, obstaclePlatforms.Length);

        GameObject result = Instantiate(obstaclePlatforms[index], pos, endPoint.rotation , platformContainer);
        
        _lastPlatform = result.transform;
        _isObstacle = true;
        _isCross = false;
    }
    
    private void CreateCrossPlatform()
    {
        Transform endPoint = _lastPlatform.GetComponent<RoadBlockController>().endPoint;
        Vector3 pos = (_lastPlatform == null) ? platformContainer.position : endPoint.position;

        int index = Random.Range(0, crossPlatforms.Length);

        GameObject result = Instantiate(crossPlatforms[index], pos, endPoint.rotation, platformContainer);
        
        _lastPlatform = result.transform;
        _isCross = true;
        _isObstacle = false;
    }
}

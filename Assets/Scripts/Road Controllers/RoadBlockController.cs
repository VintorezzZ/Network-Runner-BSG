using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadBlockController : MonoBehaviour
{
    public Transform endPoint;
    public Transform[] obstaclePoints;
    
    
    
    public static void GenerateObstacles(GameObject platform)
    {
        Transform[] obstaclePoints = platform.GetComponent<RoadBlockController>().obstaclePoints;
       
        if (obstaclePoints.Length > 0)
        {
            for (int i = 0; i < obstaclePoints.Length; i++)
            {
                if (Random.Range(0, 101) < 50)
                {
                    obstaclePoints[i].gameObject.SetActive(true);
                }
            }
        }
    }
}

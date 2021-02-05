using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    private static LineCreator instance;
    
    private static LineRenderer lineRenderer;
    private static Coroutine prevCoroutine;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public static void CreateShotLine(Vector3 startPos, Vector3 endPos, Color color)
    {
        if(prevCoroutine != null)
            instance.StopCoroutine(prevCoroutine);
        
        SetShotLineSettings(startPos, endPos, color);

        print(prevCoroutine);
        prevCoroutine = instance.StartCoroutine(DisableShotLine(0.3f));   // Why It is Null ?? 
    }
    
    private static void SetShotLineSettings(Vector3 startPos, Vector3 endPos, Color color)
    {
        Color startColor = color;
        startColor.a = 0;
        lineRenderer.SetColors(startColor, color);
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        lineRenderer.enabled = true;
    }
    
    private static IEnumerator DisableShotLine(float time)
    {
        yield return new WaitForSeconds(time);
        lineRenderer.enabled = false;
    }

}

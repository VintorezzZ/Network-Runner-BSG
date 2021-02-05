using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BaseWeapon
{
    //private LineRenderer lineRenderer;
    //private Coroutine prevCoroutine;
    public override void Init()
    {
        base.Init();
        //lineRenderer = GetComponent<LineRenderer>();
    }

    public override void Shoot()
    {
        base.Shoot();
        ProcessShoot();
    }

    private void ProcessShoot()
    {
        if (hit.collider)
        {
            LineCreator.CreateShotLine(transform.position, hit.point, Color.red);
        }
        else
        {
            LineCreator.CreateShotLine(transform.position, rayCastPoint.position + rayCastPoint.forward * RAY_DISTANCE, Color.yellow);
        }
    }

    // private void CreateShotLine(Vector3 startPos, Vector3 endPos, Color color)
    // {
    //     if(prevCoroutine != null)
    //         StopCoroutine(prevCoroutine);
    //     
    //     SetShotLineSettings(startPos, endPos, color);
    //
    //     prevCoroutine = StartCoroutine(DisableShotLine(0.3f));
    // }

    // private void SetShotLineSettings(Vector3 startPos, Vector3 endPos, Color color)
    // {
    //     Color startColor = color;
    //     startColor.a = 0;
    //     lineRenderer.enabled = true;
    //     lineRenderer.SetColors(startColor, color);
    //     lineRenderer.SetPosition(0, startPos);
    //     lineRenderer.SetPosition(1, endPos);
    // }

    // private IEnumerator DisableShotLine(float time)
    // {
    //     yield return new WaitForSeconds(time);
    //     lineRenderer.enabled = false;
    // }
}

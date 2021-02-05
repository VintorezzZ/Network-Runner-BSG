using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BaseWeapon
{
    private LineRenderer lineRenderer;
    private bool ShotLineCreated = false;
    public override void Init()
    {
        base.Init();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        StartCoroutine(DisableShotLine(3));
    }

    public override void Shoot()
    {
        base.Shoot();
        ProcessShoot();
    }

    private void ProcessShoot()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(WeaponManager.RayCastPoint.position, transform.forward, out hit, 500f))
        {
            Debug.DrawLine(WeaponManager.RayCastPoint.position, hit.point, Color.black, 2f);
            
            CreateShotLine(transform.position, hit.point, Color.red);
        }
        else
        {
            CreateShotLine(transform.position, transform.forward * -1 * 10f, Color.yellow);
            
            Debug.DrawRay(WeaponManager.RayCastPoint.position, transform.forward * -1 * 10f, Color.magenta, 2f);
        }
    }

    private void CreateShotLine(Vector3 startPos, Vector3 endPos, Color color)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetColors(color, color);
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        
        ShotLineCreated = true;
    }

    private IEnumerator DisableShotLine(float time)
    {
        if (!ShotLineCreated)
            yield break;
        
        yield return new WaitForSeconds(time);
        
        lineRenderer.enabled = false;
        ShotLineCreated = false;
    }
}

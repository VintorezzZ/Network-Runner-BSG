using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BaseWeapon
{
    private LineRenderer lineRenderer;
    private bool ShotLineCreated = false;
    private Coroutine prevCoroutine;
    public override void Init()
    {
        base.Init();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void Shoot()
    {
        base.Shoot();
        ProcessShoot();
    }

    private void ProcessShoot()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(WeaponManager.RayCastPoint.position,WeaponManager.RayCastPoint.forward , out hit, 500f))
        {
            Debug.DrawLine(WeaponManager.RayCastPoint.position, hit.point, Color.black, 2f);
            
            CreateShotLine(transform.position, hit.point, Color.red);
            
            print(hit.collider.name);
        }
        else
        {
            CreateShotLine(transform.position,  WeaponManager.RayCastPoint.position + WeaponManager.RayCastPoint.forward * 10f, Color.yellow);
            
            Debug.DrawRay(WeaponManager.RayCastPoint.position, WeaponManager.RayCastPoint.forward * 10f, Color.magenta, 2f);
        }
    }

    private void CreateShotLine(Vector3 startPos, Vector3 endPos, Color color)
    {
        if(prevCoroutine != null)
            StopCoroutine(prevCoroutine);
        
        lineRenderer.enabled = true;
        lineRenderer.SetColors(color, color);
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        prevCoroutine = StartCoroutine(DisableShotLine(2));
        
    }

    private IEnumerator DisableShotLine(float time)
    {
        yield return new WaitForSeconds(time);
        
        lineRenderer.enabled = false;
    }
}

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
        
        if (Physics.Raycast(WeaponManager.gunHolder.position, transform.forward * -1, out hit, 500f))
        {
            //Debug.DrawLine(WeaponManager.gunHolder.position, hit.point, Color.black, 2f);
            
            CreateShotLine(transform.position, hit.point);
        }
        else
        {
            //CreateShotLine(transform.position, hit.point);
            
            //Debug.DrawRay(WeaponManager.gunHolder.position, transform.forward * -1 * 10f, Color.magenta, 2f);
        }
    }

    private void CreateShotLine(Vector3 startPos, Vector3 endPos)
    {
        if (endPos == Vector3.zero)
            endPos = -transform.forward * 10f;

        lineRenderer.enabled = true;
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

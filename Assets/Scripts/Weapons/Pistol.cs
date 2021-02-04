using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BaseWeapon
{
    private LineRenderer lineRenderer;
    
    public override void Shoot()
    {
        base.Shoot();
        ProcessShoot();
    }

    private void ProcessShoot()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(WeaponManager.gunHolder.position, transform.TransformDirection(Vector3.forward), out hit, 500f))
        {
            
            Debug.DrawRay(WeaponManager.gunHolder.position, transform.TransformDirection(Vector3.forward) * Mathf.Infinity, Color.red);
            
            Debug.DrawRay(WeaponManager.gunHolder.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            
            Debug.Log("Did Hit");
        }
    }
}

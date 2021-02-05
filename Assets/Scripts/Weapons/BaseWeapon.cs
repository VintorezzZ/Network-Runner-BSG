using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    protected RaycastHit hit;
    protected const float RAY_DISTANCE = 30f;
    protected Transform rayCastPoint = WeaponManager.RayCastPoint;
    protected Vector3 targetDirection;
    public virtual void Init()
    {
        
    }
    public virtual void Shoot()
    {
        if (Physics.Raycast(rayCastPoint.position,rayCastPoint.forward , out hit, RAY_DISTANCE))
        {
            //Debug.DrawLine(WeaponManager.RayCastPoint.position, hit.point, Color.black, 2f);
            targetDirection = -(hit.point - transform.position).normalized;
        }
        else
        {
            //Debug.DrawRay(WeaponManager.RayCastPoint.position, WeaponManager.RayCastPoint.forward * 10f, Color.magenta, 2f);
            targetDirection = -rayCastPoint.forward;
        }
    }
    
    
}

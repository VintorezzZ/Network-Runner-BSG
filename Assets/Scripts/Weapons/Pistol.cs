using UnityEngine;

public class Pistol : BaseWeapon
{
    private LineCreator lineCreator;
    public override void Init()
    {
        base.Init();
        lineCreator = GetComponent<LineCreator>();
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
            if (hit.collider.TryGetComponent(out IDamageable iDamageable))
            {
                iDamageable.TakeDamage();
            }
            
            lineCreator.CreateShotLine(transform.position, hit.point, Color.red);
        }
        else
        {
            lineCreator.CreateShotLine(transform.position, rayCastPoint.position + rayCastPoint.forward * RAY_DISTANCE, Color.yellow);
        }
    }
}

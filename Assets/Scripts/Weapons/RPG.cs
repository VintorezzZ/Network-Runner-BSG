using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG : BaseWeapon
{
    public override void Init()
    {
        base.Init();
    }

    public override void Shoot()
    {
        base.Shoot();
        ProcessShoot();
    }

    private void ProcessShoot()
    {
       InstantiateBullet();
    }
    
    
    private void InstantiateBullet()
    {
        Bullet bullet = PoolManager.Get(PoolType.Bullets).GetComponent<Bullet>();
        bullet.playerVelocity = GameManager.instance.playerController.speed;
        
        SetBulletSettings(bullet);
    }
    
    private void SetBulletSettings(Bullet bullet)                        // уточнить 
    {
        bullet.gameObject.SetActive(true);
        //bullet.transform.SetParent(generatedBullets);
        bullet.transform.position = transform.position + -transform.forward;
        bullet.transform.rotation = transform.rotation;
    }
}

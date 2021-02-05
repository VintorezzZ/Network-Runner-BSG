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
        Shell shell = PoolManager.Get(PoolType.Rockets).GetComponent<Shell>();
        shell.playerVelocity = GameManager.instance.playerController.speed;
        
        SetShellSettings(shell);
        
    }
    
    private void SetShellSettings(Shell shell)                        // уточнить 
    {
        //bullet.transform.SetParent(generatedBullets);
        shell.transform.position = transform.position + -transform.forward;
        shell.transform.rotation = transform.rotation;
        
        shell.Init(targetDirection);
        
        shell.gameObject.SetActive(true);
    }
}

using System;
using UnityEngine;

public class WeaponManager
{
    private Transform gunHolder;
    private Transform RayCastPoint;
    
    private BaseWeapon currentWeapon;
    private string previousWeaponName;

    private float timer = 0f;
    private bool timerStarted = false;
    public WeaponManager(Transform gunHolder, Transform RayCastPoint)
    {
        this.gunHolder = gunHolder;
        this.RayCastPoint = RayCastPoint;
    }

    public void Init()
    {
        LoadWeapon("M1911");
    }

    public void OnUpdate()
    {
        DeactivateWeaponByTimer();
    }
    public BaseWeapon LoadWeapon(string weaponName)
    {
        string path = "Weapons/" + weaponName;
        
        BaseWeapon weapon = GameObject.Instantiate(Resources.Load<BaseWeapon>(path)).GetComponent<BaseWeapon>();
        SetWeaponSettings(weapon);
        currentWeapon = weapon;
        return weapon;
    }

    private void SetWeaponSettings(BaseWeapon weapon)
    {
        weapon.Init(RayCastPoint);
        weapon.transform.SetParent(gunHolder);
        weapon.transform.position = gunHolder.position;
        weapon.transform.rotation = gunHolder.rotation;
    }

    public void SwitchWeapon(string weaponName, float deactivationTime)
    {
        if (GetWeaponName(currentWeapon) == weaponName)
            return;

        SetTimer(deactivationTime);

        previousWeaponName = GetWeaponName(currentWeapon);
        
        GameObject.Destroy(currentWeapon.gameObject);
        currentWeapon = LoadWeapon(weaponName);
    }

    private void SetTimer(float time)
    {
        timer = time;
        timerStarted = true;
    }

    private string GetWeaponName(BaseWeapon weapon)
    {
        return weapon.name.Replace("(Clone)", ""); // name(clone) => name
    }

    public void DeactivateWeaponByTimer()   
    {
        timer -= Time.deltaTime;

        if (timerStarted && timer < 0)
        {
            timerStarted = false;
            GameObject.Destroy(currentWeapon.gameObject);
            LoadWeapon(previousWeaponName);
        }
    }

    public void Shoot()
    {
        currentWeapon.Shoot();
    }


    // в конструкторе передаем ганХолдер
    // на старре тэйк веарон из ресуррсес лоад 
    // запоминаем в переменную и получаем бейзвеапон.
    // у него вызываем нужный метод
}




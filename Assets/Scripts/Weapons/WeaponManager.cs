using System;
using UnityEngine;

public class WeaponManager
{
    private BaseWeapon currentWeapon;
    public static Transform gunHolder;

    public WeaponManager(Transform gunHolder)
    {
        WeaponManager.gunHolder = gunHolder;
    }

    public void Init()
    {
        LoadWeapon("M1911");
    }

    public BaseWeapon LoadWeapon(string weaponName)
    {
        string path = "Weapons/" + weaponName;
        
        BaseWeapon weapon = GameObject.Instantiate(Resources.Load<BaseWeapon>(path)).GetComponent<BaseWeapon>();
        SetWeaponSettings(weapon);
        currentWeapon = weapon;
        Debug.Log(currentWeapon.name);
        return weapon;
    }

    private void SetWeaponSettings(BaseWeapon weapon)
    {
        weapon.transform.SetParent(gunHolder);
        weapon.transform.position = gunHolder.position;
        weapon.transform.rotation = gunHolder.rotation;
    }

    public void Shoot()
    {
        currentWeapon.Shoot();
    }

    public void SwitchWeapon(string weaponName)
    {
        if (currentWeapon.name == weaponName)
            return;

        GameObject.Destroy(currentWeapon.gameObject);
        
        currentWeapon = LoadWeapon(weaponName);
    }

    
    
    
    
    
    
    
    
    
    
    

    // в конструкторе передаем ганХолдер
    // на старре тэйк веарон из ресуррсес лоад 
    // запоминаем в переменную и получаем бейзвеапон.
    // у него вызываем нужный метод
}




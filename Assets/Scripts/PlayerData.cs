using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public float health;
    public List<Weapon> equippedWeapons = new List<Weapon>();
    public List<WeaponData> weaponDataList = new List<WeaponData>();
    public enum WeaponType
    {
        shooter,
        shotgun
    }

    [System.Serializable]
    public class Weapon
    {
        public float shootTimer;
        public WeaponData data;
        public Weapon (WeaponData weaponData)
        {
            data = weaponData;
        }

        public bool IncrimentTimer()
        {
            shootTimer += Time.deltaTime;
            if(shootTimer >= data.fireRate)
            {
                shootTimer = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    public static void addWeapon(WeaponType weapon)
    {
        int chosenWeaponInt = (int) weapon;
        //new Weapon(weaponDataList[chosenWeaponInt]);
    }
    
    public void OnHit(float damage)
    {
        health -= damage;
    }
}

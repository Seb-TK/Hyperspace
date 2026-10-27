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
            float randomOffsetRange = 0.1f;
            float randomOffset = Random.Range(-randomOffsetRange,randomOffsetRange);
            shootTimer += Time.deltaTime + (Time.deltaTime * randomOffset);
            if(shootTimer >= data.fireRate)
            {
                shootTimer = 0;
                Debug.Log("shoot timer is above fire rate");
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

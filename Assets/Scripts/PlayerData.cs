using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public List<Weapon> equippedWeapons = new List<Weapon>();
    public List<WeaponData> weaponDataList = new List<WeaponData>();
    public enum WeaponType
    {
        Shooter,
        MachineGun,
        Shotgun,
        Flamethrower,
        Sniper

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
        public void ResetTimer()
        {
            float fireOffset = Random.Range(0f,0.2f);
            shootTimer = fireOffset;
        }

        public bool IncrimentTimer()
        {
            if(shootTimer >= data.fireRate / PlayerStats.Mods[CrdMngr.Stats.FireRate])
            {
                shootTimer = 0;
                return true;
            }
            else
            {
                shootTimer += Time.deltaTime;
                return false;
            }
        }

    }

    public void addWeapon(WeaponType weapon)
    {
        WeaponData chosenWeaponData = weaponDataList.Find(d => d.weaponType == weapon);
        Weapon newWeapon = new Weapon(chosenWeaponData);
    }
}

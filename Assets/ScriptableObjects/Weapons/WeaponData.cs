using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public String weaponName;
    public GameObject bulletObj;
    public float damage;
    public float fireRate;
    public float speed;
    public int count;
    public float size;
    public float accuracy;
    public float range;
}

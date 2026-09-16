using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public GameObject bullet;
    public float bulletDamage;
    public float bulletSpeed;
    public float fireRate;
    public float bulletSize;
    public float bulletRange;
    public float health;
    
    public void OnHit(float damage)
    {
        health -= damage;
    }
}

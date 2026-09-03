using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet;
    private PlayerData data;
    private float bulletDamage;
    private float bulletSpeed;
    private float fireRate;
    private float fireRateTimer;
    private float bulletSize;
    private bool isShooting;
    private int enemyDetectionRadius;
    void Awake()
    {
        data = GetComponent<PlayerData>();
        bullet = data.bullet;
        bulletDamage = data.bulletDamage;
        bulletSpeed = data.bulletSpeed;
        fireRate = data.fireRate;
        bulletSize = data.bulletSize;
    }
    void Update()
    {
        MouseLook();
        ShootInput();
    }

    void MouseLook()
    {
        Camera camera = Camera.main;
        float camOffset = -camera.transform.position.z;

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camOffset);
        Vector3 worldPosition = camera.ScreenToWorldPoint(mousePosition);

        Vector2 direction = worldPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle - 90); // 90 is for up offset
    }

    void ShootInput()
    {
        //ToggleInput();
        DetectEnemy();
        if(isShooting){
            if (Time.time > fireRateTimer)
            {
                Shoot();
                fireRateTimer = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        GameObject spawnedBullet = Instantiate(bullet, transform.position, transform.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Damage = bulletDamage;
        bulletScript.MoveSpeed = bulletSpeed;
        bulletScript.BulletSize = bulletSize;
    }

    void ToggleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = !isShooting;
        }
    }

    void DetectEnemy()
    {
        float rayCastDistance = 20f;
        RaycastHit output;
        LayerMask enemyMask = LayerMask.GetMask("Enemy");

        bool isSeeingEnemy = Physics.SphereCast(
            transform.position, 
            enemyDetectionRadius, 
            transform.up, 
            out output,
            rayCastDistance);

        isShooting = isSeeingEnemy;
        //Debug.Log(isShooting);
    }
    
}

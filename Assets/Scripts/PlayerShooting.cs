using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
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
    private float bulletRange;
    private int bulletCount;
    private float bulletAccuracy;
    private bool isShooting;
    private GameObject bulletVisualMesh;
    private int enemyDetectionRadius;
    private PlayerData playerData;
    [SerializeField] private float lockOnRange;
    [SerializeField] private float lockOnRangeZ;
    [SerializeField] private bool hasToggleInput;
    [SerializeField] private bool hasLockOn;

    void Start()
    {
        PlayerData.WeaponType shooter = PlayerData.WeaponType.shooter;
        PlayerData.addWeapon(shooter);
        playerData = GetComponent<PlayerData>();
    }
    void Update()
    {
        if (hasLockOn)
        {
            
        }
        else
        {
            MouseLook();
        }
        if (hasToggleInput)
        {
            if (ToggleInput())
            {
                FireGuns();
            }
        }
        else
        {
            if (Input.GetMouseButton(1))
            {
                FireGuns();
            }
        }
        
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

    void ApplyWeaponData(PlayerData.Weapon weapon)
    {
        WeaponData data = weapon.data;
        bullet = data.bulletObj;
        bulletDamage = data.damage;
        bulletSpeed = data.speed;
        fireRate = data.fireRate;
        bulletSize = data.size;
        bulletRange = data.range;
        bulletCount = data.count;
        bulletAccuracy = data.accuracy;
        bulletVisualMesh = data.bulletVisualMesh;
    }

    void FireGuns()
    {
        for(int i = 0; i < playerData.equippedWeapons.Count; i++)
        {
            if (playerData.equippedWeapons[i].IncrimentTimer())
            {
                ApplyWeaponData(playerData.equippedWeapons[i]);
                Shoot();
            }
        }
    }

    void Shoot()
    {
        for(int j = 0; j < bulletCount; j++)
        {
            float randomOffsetFloat = Random.Range(-bulletAccuracy, bulletAccuracy);
            Quaternion rotationOffset = Quaternion.Euler(0, 0, randomOffsetFloat);
            SpawnBullet(transform.position, transform.rotation * rotationOffset, bulletVisualMesh);
        }
    }
        

    void SpawnBullet(Vector3 position, Quaternion rotation, GameObject visualMesh)
    {
        GameObject spawnedBullet = Instantiate(bullet, position, rotation);
        Bullet bulletScript = spawnedBullet.GetComponent<Bullet>();
        bulletScript.BulletVisualMesh = bulletVisualMesh;
        bulletScript.Damage = bulletDamage;
        bulletScript.MoveSpeed = bulletSpeed;
        bulletScript.BulletSize = bulletSize;
        bulletScript.BulletRange = bulletRange;
        bulletScript.isPlayerBullet = true;
    }

    bool ToggleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = !isShooting;
        }
        return isShooting;
    }

    

    GameObject DetectEnemy()
    {
        GameObject output = null;
        LayerMask enemyMask = LayerMask.GetMask("Enemy");

        Collider[] hits = Physics.OverlapSphere(
            transform.position, 
            enemyDetectionRadius, 
            enemyMask);
        if (hits.Length == 0)
        {
            return null;
        }

        for(int i = 0; i < hits.Length; i++)
        {
            
        }

        
        return output;
        
    }
    
}

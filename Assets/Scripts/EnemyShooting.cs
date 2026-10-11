using Unity.VisualScripting;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private EnemyData EnemyDataComponent;
    private EnemyDataScriptableObject data;
    private GameObject mesh;
    private GameObject player;
    private EnemyDataScriptableObject.AiTypes AiType;
    private float fireRateTimer;
    private float damage;
    private float fireRate;
    private float range;
    private int bulletCount;
    private float accuracy;
    private float speed;
    private float bulletSize;
    private GameObject bulletPrefab;
    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>().gameObject;

        data = GetComponent<EnemyData>().EnemyDataContainer;
        AiType = data.aiType;
        if(AiType == EnemyDataScriptableObject.AiTypes.Charger)
        {
            enabled = false;
        }
        fireRate = data.fireRate;
        range = data.bulletRange;
        bulletCount = data.bulletCount;
        speed = data.bulletSpeed;
        accuracy = data.bulletAccuracy;
        bulletSize = data.bulletSize;
        bulletPrefab = data.bulletPrefab;
    }

    void Update()
    {
        if (Time.time > fireRateTimer)
        {
            Shoot(bulletCount, accuracy);
            fireRateTimer = Time.time + fireRate;
        }
    }

    void Shoot(int bulletCount, float accuracy)
    {
        for(int i = 0; i < this.bulletCount; i++)
        {
            float randomOffsetFloat = Random.Range(-this.accuracy, this.accuracy);
            Quaternion randomOffset = Quaternion.Euler(0, 0, randomOffsetFloat - 90);
            SpawnBullet(transform.position, transform.rotation * randomOffset);
        }
    }
    void SpawnBullet(Vector3 position, Quaternion rotation)
    {
        GameObject spawnedBullet = Instantiate(bulletPrefab, position, rotation);
        Bullet bulletScript = spawnedBullet.GetComponent<Bullet>();
        bulletScript.isPlayerBullet = false;
        bulletScript.BulletSize = bulletSize;
        bulletScript.Damage = damage;
        bulletScript.MoveSpeed = speed;
        bulletScript.bulletRange = range;
    }

}

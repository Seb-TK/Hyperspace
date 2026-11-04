using Unity.VisualScripting;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private EnemyData EnemyDataComponent;
    private EnemyDataObj data;
    private GameObject mesh;
    private GameObject bulletVisualMesh;
    private GameObject player;
    private EnemyDataObj.AiTypes AiType;
    private float fireRateTimer;
    private float damage;
    private float fireRate;
    private float range;
    private int bulletCount;
    private float accuracy;
    private float speed;
    private float bulletSize;
    private float rangeRandom;
    private bool ignoreEdgeWrap;
    private GameObject bulletPrefab;
    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>().gameObject;

        data = GetComponent<EnemyData>().EnemyDataContainer;
        AiType = data.aiType;
        if(AiType == EnemyDataObj.AiTypes.Charger)
        {
            enabled = false;
        }
        bulletVisualMesh = data.bulletVisualMesh;
        fireRate = data.fireRate;
        range = data.bulletRange;
        bulletCount = data.bulletCount;
        speed = data.bulletSpeed;
        accuracy = data.bulletAccuracy;
        bulletSize = data.bulletSize;
        bulletPrefab = data.bulletPrefab;
        ignoreEdgeWrap = data.ignoreEdgeWrap;
        rangeRandom = data.rangeRandom;
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
            Vector3 randomOffset = new Vector3(0, 0, randomOffsetFloat);
            Vector3 direction = player.transform.position - transform.position;
            direction.z = 0;
            SpawnBullet(transform.position, Quaternion.Euler(randomOffset) * direction);
            
        }
    }
    void SpawnBullet(Vector3 position, Vector3 rotation)
    {
        Bullet bulletObj = bulletPrefab.GetComponent<Bullet>();
        Bullet spawnedBullet = Instantiate(bulletObj, position, transform.rotation);
        spawnedBullet.Init(rotation);
        spawnedBullet.BulletVisualMesh = bulletVisualMesh;
        spawnedBullet.isPlayerBullet = false;
        spawnedBullet.ignoreEdgeWrap = ignoreEdgeWrap;
        spawnedBullet.BulletSize = bulletSize;
        spawnedBullet.Damage = damage;
        spawnedBullet.MoveSpeed = speed;
        spawnedBullet.BulletRange = range;
        spawnedBullet.rangeRandom = rangeRandom;
    }

}

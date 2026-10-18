using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    
    [SerializeField] private EdgeWrapping edgeWrapping;
    [SerializeField] private float collisionDistance;
    public float MoveSpeed;
    public float bulletRange;
    public float Damage;
    public float BulletSize;
    public bool ignoreEdgeWrap = false;
    private Rigidbody rigidBody;
    public bool isPlayerBullet;
    private Vector3 startingPos;
    private float distanceTraveled;
    public float rangeRandom;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        transform.localScale = Vector3.one * BulletSize;
        startingPos = transform.position;
    }
    void Update()
    {
        transform.position += transform.up * MoveSpeed * Time.deltaTime;
        distanceTraveled += MoveSpeed * Time.deltaTime;
        if (distanceTraveled > (bulletRange + UnityEngine.Random.Range(0,rangeRandom)))
        {
            //Destroy(gameObject);
        }
    }

    void DieOnEdge()
    {
        WorldPlane worldPlane = FindAnyObjectByType<WorldPlane>();
        float boundWidthPos = worldPlane.boundWidth;
        float boundHeightPos = worldPlane.boundHeight;
        if(transform.position.x > boundWidthPos)
        {
            Destroy(gameObject);
        }
        if(transform.position.x < -boundWidthPos)
        {
            Destroy(gameObject);
        }
        if(transform.position.y > boundHeightPos)
        {
            Destroy(gameObject);
        }
        if(transform.position.y < -boundHeightPos)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collider = other.gameObject;
        float distanceToCollider = transform.position.z - collider.transform.position.z;
        distanceToCollider = Mathf.Abs(distanceToCollider);
        if(distanceToCollider <= collisionDistance)
        {
            if (isPlayerBullet)
            {
                if (collider.GetComponent<EnemyData>() != null)
                {
                    EnemyData enemyDataScript = collider.GetComponent<EnemyData>();
                    enemyDataScript.OnHit(Damage);
                }
            }
            else
            {
                if (collider.GetComponent<PlayerData>() != null)
                {
                    PlayerData playerData = collider.GetComponent<PlayerData>();
                    playerData.OnHit(Damage);
                }
            }
        }
    }
    
}

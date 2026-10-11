using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    
    [SerializeField] private EdgeWrapping edgeWrapping;
    public float MoveSpeed;
    public float bulletRange;
    public float Damage;
    public float BulletSize;
    private Rigidbody rigidBody;
    private float boundWidthPos;
    private float boundHeightPos;
    public bool isPlayerBullet;
    private Vector3 startingPos;
    private Vector3 savedVelocity;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        transform.localScale = Vector3.one * BulletSize;
        float bounds = 1.1f;
        float [] boundArray = edgeWrapping.FindBoundPos(bounds, bounds);
        boundWidthPos = boundArray[0];
        boundHeightPos = boundArray[1];
        startingPos = transform.position;
    }
    void Update()
    {
        transform.position += transform.up * MoveSpeed;
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
        if (Vector3.Distance(transform.position, startingPos) > bulletRange)
        {
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collider = other.gameObject;
        if(GetComponent<ObjectBehavior>().collisionCheck(startingPos.z, collider.transform.position.z))
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

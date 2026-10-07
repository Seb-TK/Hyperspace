using System;
using Unity.VisualScripting;
using UnityEngine;

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
    void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + transform.up * MoveSpeed * Time.deltaTime);
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
        GameObject colliderGameObject = other.gameObject;
        if (isPlayerBullet)
        {
            if (colliderGameObject.GetComponent<EnemyData>() != null)
            {
                EnemyData enemyDataScript = colliderGameObject.GetComponent<EnemyData>();
                enemyDataScript.OnHit(Damage);
            }
        }
        else
        {
            if (colliderGameObject.GetComponent<PlayerData>() != null)
            {
                PlayerData playerData = colliderGameObject.GetComponent<PlayerData>();
                playerData.OnHit(Damage);
            }
        }
    }
    
}

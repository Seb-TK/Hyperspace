using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    
    [SerializeField] private EdgeWrapping edgeWrapping;
    [SerializeField] private float collisionDistance;
    public GameObject BulletVisualMesh;
    public float MoveSpeed;
    public float BulletRange;
    public float Damage;
    public float BulletSize;
    public bool ignoreEdgeWrap = false;
    private Rigidbody rigidBody;
    public bool isPlayerBullet;
    private Vector3 startingPos;
    private float distanceTraveled;
    public float rangeRandom;
    private Vector3 moveDirection;
    public bool doesDieOnEdge;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        transform.localScale = Vector3.one * BulletSize;
        startingPos = transform.position;
        GameObject VisualMeshObj = Instantiate(BulletVisualMesh, transform.position, transform.rotation);
        VisualMeshObj.transform.parent = transform;
    }
    public void Init(Vector3 direction)
    {
        moveDirection = direction.normalized;
        Vector3 moveDirectionNoZ = moveDirection;
        moveDirectionNoZ.z = 0f;
        transform.up = moveDirectionNoZ;
    }
    void Update()
    {
        transform.position += moveDirection * MoveSpeed * Time.deltaTime;
        distanceTraveled += MoveSpeed * Time.deltaTime;
        if (distanceTraveled > (BulletRange + UnityEngine.Random.Range(0,rangeRandom)))
        {
            Destroy(gameObject);
        }
        if (isPlayerBullet | doesDieOnEdge)
        {
            DieOnEdge();
        }
    }

    void DieOnEdge()
    {
        WorldPlane worldPlane = FindAnyObjectByType<WorldPlane>();
        float boundWidthPos = worldPlane.boundWidth;
        float boundHeightPos = worldPlane.boundHeight;
        float cameraX = Camera.main.transform.position.x;
        float cameraY = Camera.main.transform.position.y;
        if(transform.position.x > boundWidthPos + cameraX)
        {
            Destroy(gameObject);
        }
        if(transform.position.x < -boundWidthPos + cameraX)
        {
            Destroy(gameObject);
        }
        if(transform.position.y > boundHeightPos + cameraY)
        {
            Destroy(gameObject);
        }
        if(transform.position.y < -boundHeightPos + cameraY)
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

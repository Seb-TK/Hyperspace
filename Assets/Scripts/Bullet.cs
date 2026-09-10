using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float killDistance;
    public float MoveSpeed;
    public float Damage;
    public float BulletSize;
    private Rigidbody rigidBody;
    private float boundWidthPos;
    private float boundHeightPos;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        transform.localScale = Vector3.one * BulletSize;
        EdgeWrapping edgeWrapping = GetComponent<EdgeWrapping>();
        float bounds = 1.1f;
        edgeWrapping.FindBoundPos(boundWidthPos,boundHeightPos, bounds, bounds);
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
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject colliderGameObject = other.gameObject;
        if (colliderGameObject.GetComponent<EnemyData>() != null)
        {
            EnemyData enemyDataScript = colliderGameObject.GetComponent<EnemyData>();
            enemyDataScript.OnHit(Damage);
        }
    }
}

using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float MoveSpeed;
    public float Damage;
    public float BulletSize;
    private Rigidbody rigidBody;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        transform.localScale = Vector3.one * BulletSize;
    }
    void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + transform.up * MoveSpeed * Time.deltaTime);
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

using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public EnemyDataScriptableObject EnemyDataScriptableObject;
    private float health;
    void Awake()
    {
        GameObject meshPrefab = EnemyDataScriptableObject.enemyVisualMesh;
        Instantiate(meshPrefab, this.transform);
        health = EnemyDataScriptableObject.health;

        ObjectBehavior objectBehavior = GetComponent<ObjectBehavior>();
        objectBehavior.objectWidth = EnemyDataScriptableObject.objectWidth;
        objectBehavior.sizeChangeMultiplier = EnemyDataScriptableObject.sizeChangeMultiplier;
        objectBehavior.sizeChangeOffset = EnemyDataScriptableObject.sizeChangeOffset;

        objectBehavior.objectHitBoxWidth = EnemyDataScriptableObject.objectHitBoxWidth;
    }

    public void OnHit(float damage)
    {
        health -= damage;
        if(health <= 0f)
        {
            Die();
        }
        Debug.Log(health);
    }
    private void Die()
    {
        Debug.Log("die");
        Destroy(gameObject);
    }
    
}

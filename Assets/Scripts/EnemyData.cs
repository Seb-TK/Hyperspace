using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public EnemyDataScriptableObject EnemyDataContainer;
    private float health;
    void Start()
    {

        health = EnemyDataContainer.health;

        ObjectBehavior objectBehavior = GetComponent<ObjectBehavior>();
        objectBehavior.objectWidth = EnemyDataContainer.objectWidth;
        objectBehavior.sizeChangeMultiplier = EnemyDataContainer.sizeChangeMultiplier;
        objectBehavior.sizeChangeOffset = EnemyDataContainer.sizeChangeOffset;

        objectBehavior.objectHitBoxWidth = EnemyDataContainer.objectHitBoxWidth;
        EnemyVisualMesh visualMesh = transform.GetChild(0).gameObject.GetComponent<EnemyVisualMesh>();
        visualMesh.SizeMultiplier = EnemyDataContainer.visualSizeMultiplier;
    }

    public void OnHit(float damage)
    {
        health -= damage;
        if(health <= 0f)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    
}

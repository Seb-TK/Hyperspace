using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public EnemyDataScriptableObject EnemyDataContainer;
    private float health;
    private EnemyVisualMesh visualMesh;
    private ObjectBehavior objectBehavior;
    private float iFramesEnd;
    [SerializeField] private float invincibilityLength;
    void Start()
    {

        health = EnemyDataContainer.health;

        objectBehavior = GetComponent<ObjectBehavior>();
        objectBehavior.objectWidth = EnemyDataContainer.objectWidth;
        objectBehavior.sizeChangeMultiplier = EnemyDataContainer.sizeChangeMultiplier;
        objectBehavior.sizeChangeOffset = EnemyDataContainer.sizeChangeOffset;

        objectBehavior.objectHitBoxWidth = EnemyDataContainer.objectHitBoxWidth;
        visualMesh = transform.GetChild(0).gameObject.GetComponent<EnemyVisualMesh>();
        visualMesh.SizeMultiplier = EnemyDataContainer.visualSizeMultiplier;
    }

    

    public void OnHit(float damage)
    {
        if (Time.time > iFramesEnd)
        {
            iFramesEnd = Time.time + invincibilityLength;
        } else { 
            return;
        }
        health -= damage;
        EnemyAnimations enemyAnimations = visualMesh.gameObject.GetComponent<EnemyAnimations>();
        enemyAnimations.getHit();
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

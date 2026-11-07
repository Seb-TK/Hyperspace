using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public EnemyDataObj EnemyDataContainer;
    private float health;
    private EnemyVisualMesh visualMesh;
    private ObjectBehavior objectBehavior;
    private float iFramesEnd;
    private float xpDropped;
    private UpgradeManager upgradeManager;
    [SerializeField] private float invincibilityLength;
    [SerializeField] private GameObject offScreenArrow;
    void Start()
    {
        health = EnemyDataContainer.health;
        visualMesh = transform.GetChild(0).gameObject.GetComponent<EnemyVisualMesh>();
        xpDropped = EnemyDataContainer.xpDropped;
        upgradeManager = FindAnyObjectByType<UpgradeManager>();
        if(EnemyDataContainer.aiType == EnemyDataObj.AiTypes.Drifter)
        {
            objectBehavior = GetComponent<ObjectBehavior>();
            objectBehavior.objectWidth = EnemyDataContainer.objectWidth;
            objectBehavior.sizeChangeMultiplier = EnemyDataContainer.sizeChangeMultiplier;
            objectBehavior.sizeChangeOffset = EnemyDataContainer.sizeChangeOffset;

            objectBehavior.objectHitBoxWidth = EnemyDataContainer.objectHitBoxWidth;
            visualMesh.SizeMultiplier = EnemyDataContainer.visualSizeMultiplier;
        }
        
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
        EnemyManager enemyManager = FindAnyObjectByType<EnemyManager>();
        enemyManager.enemyCount--;
        upgradeManager.GainXp(xpDropped);
        Destroy(gameObject);
    }
    
}

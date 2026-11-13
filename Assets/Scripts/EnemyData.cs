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
    private float hitAnimationEnd;
    private float xpDropped;
    private UpgradeManager upgradeManager;
    [SerializeField] private float invincibilityLength;
    [SerializeField] private float hitAnimationLength;
    [SerializeField] private GameObject offScreenArrow;
    [SerializeField] private GameObject explolion;
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
        if (Time.time > hitAnimationEnd)
        {
            hitAnimationEnd = Time.time + hitAnimationLength;
            EnemyAnimations enemyAnimations = visualMesh.gameObject.GetComponent<EnemyAnimations>();
            enemyAnimations.GetHit();
        }
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
        AnimationManager animationManager = FindAnyObjectByType<AnimationManager>();
        animationManager.SpawnExplolion(transform.position);
        Destroy(gameObject);
    }
    
}

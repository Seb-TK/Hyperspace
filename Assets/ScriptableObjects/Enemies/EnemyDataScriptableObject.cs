using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataScriptableObject", menuName = "Scriptable Objects/EnemyDataScriptableObject")]
public class EnemyDataScriptableObject : ScriptableObject
{
    

    //Learn about [System.Serializable] sometime
    [Header("Movement")]
    public float movementSpeed;
    public float movementSpeedZ;
    public float movementRandomness;
    public float rotationSpeed;

    public enum AiTypes{
        Ranger,
        Charger,
        Drifter
    };

    public AiTypes aiType;
    [Header("Ranged")] 
    public float hoverDistance;

    [Header("Charge")]    
    public float chargeSpeed;
    public float chargeDistance;
    public float chargePauseLength;
    public float chargeRate;

    [Header("Object Dimensions")]
    public GameObject enemyVisualMesh;
    public float objectWidth;
    public float objectHitBoxWidth;
    public float sizeChangeMultiplier;
    public float sizeChangeOffset;
    public float visualSizeMultiplier = 1;

    [Header("Stats")]
    public float health;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public float fireRate;
    public float bulletSize;
    public float bulletRange;
    public float bulletSpeed;
    public int bulletCount;
    public float bulletAccuracy;
}

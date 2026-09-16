using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataScriptableObject", menuName = "Scriptable Objects/EnemyDataScriptableObject")]
public class EnemyDataScriptableObject : ScriptableObject
{
    public GameObject enemyVisualMesh;
    [Header("Movement")]
    public float movementSpeed;
    public float movementSpeedZ;
    public float movementRandomness;
    public float rotationSpeed;
    public float chargeSpeed;
    public float chargeDistance;
    public float chargePauseLength;
    public float chargeRate;
    public enum AiTypes{
        Ranger,
        Charger,
        Drifter
    };
    public AiTypes aiType;
    public float hoverDistance;
    [Header("Object Dimensions")]
    public float objectWidth;
    public float objectHitBoxWidth;
    public float sizeChangeMultiplier;
    public float sizeChangeOffset;
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

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataScriptableObject", menuName = "Scriptable Objects/EnemyDataScriptableObject")]
public class EnemyDataObj : ScriptableObject
{
    

    //Learn about [System.Serializable] sometime
    [Header("Movement")]
    public float movementSpeed;
    public float velocityDecayRate;
    public float maxVelocity;
    public float movementSpeedZ;
    public float velocityDecayRateZ;
    public float maxVelocityZ;
    public float movementRandMultiplierInputZ;
    public float movementBurstsRate;
    public float movementBurstsAmount;
    public float rotationSpeed;
    public float rotationalVelocityClamp;
    public float rotationalVelocityDecay;
    public bool ignoreEdgeWrap;
    public float hoverDistance;
    public float driftSpeed;

    public enum AiTypes{
        Ranger,
        Charger,
        Drifter
    };

    public AiTypes aiType;

    [Header("Charge")]
    public float chargePower;
    public float chargePauseLength;
    public float chargeEndPauseLength;
    public float chargeRate;
    public float chargeSpeedDecayMultiplier;

    [Header("Object Dimensions")]
    public GameObject enemyVisualMesh;
    public float objectWidth;
    public float objectHitBoxWidth;
    public float sizeChangeMultiplier;
    public float sizeChangeOffset;
    public float visualSizeMultiplier = 1;

    [Header("Stats")]
    public float health;
    public int waveUnlocked;
    public int energyCost;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public GameObject bulletVisualMesh;
    public float fireRate;
    public float bulletSize;
    public float bulletRange;
    public float bulletSpeed;
    public int bulletCount;
    public float bulletAccuracy;
    public float rangeRandom;
}

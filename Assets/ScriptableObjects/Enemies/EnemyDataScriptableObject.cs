using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataScriptableObject", menuName = "Scriptable Objects/EnemyDataScriptableObject")]
public class EnemyDataScriptableObject : ScriptableObject
{
    public GameObject bullet;
    public GameObject enemyVisualMesh;
    [Header("Movement")]
    public float movementSpeed;
    public float movementRandomness;
    public float rotationSpeed;
    [Header("Object Dimensions")]
    public float objectWidth;
    public float objectHitBoxWidth;
    public float sizeChangeMultiplier;
    public float sizeChangeOffset;
    [Header("Stats")]
    public float health;

}

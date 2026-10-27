using UnityEngine;

[CreateAssetMenu(fileName = "StatCard", menuName = "Scriptable Objects/StatCard")]
public class StatCard : ScriptableObject
{
    [Header("General Traits")]
    public GameObject VisualMesh;
    public string Name;
    public string Description;
    public string DetailedDescription;
    public enum Rarities{
        Basic,
        Good,
        Great,
        Epic,
        Mythical,
        Godly
    };

    public Rarities Rarity;
    
    [Header("Shooting")]
    public float fireRateMulti;
    public float fireRateAdd;
    public float damageMulti;
    public float damageAdd;
    public float bulletSpeedMulti;
    public float bulletSpeedAdd;
    public float bulletSizeMulti;
    public float bulletSizeAdd;
    public float bulletCountAdd;


    [Header("Survivability")]
    public float healthMulti;
    public float healthAdd;
    public float movementSpeedMulti;
    public float movementSpeedAdd;
    public float ZaxisMovementSpeedMulti;
    public float ZaxisMovementSpeedAdd;
    public float hitBoxSizeMulti;
    public float hitBoxSizeAdd;
    public float dashSpeedMulti;
    public float dashSpeedAdd;
    public float dashCooldownMulti;
    public float dashCooldownAdd;
    public float cameraZoomMulti;
    public float cameraZoomAdd;
}

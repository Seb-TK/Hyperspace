using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static List<string> Rarities = new List<string> 
        {
            "Basic",
            "Good",
            "Great",
            "Epic",
            "Mythical",
            "Godly"
        };

    public static List<int> RarityWeights = new List<int> 
        {
            12,
            10,
            8,
            5,
            3,
            1
        };
    public static List<float> RarityValues = new List<float> 
        {
            2,
            5,
            10,
            25,
            50,
            80
        };

    public enum StatTypes
    {
        FireRate, 
        Damage, 
        BulletSpeed, 
        BulletSize,
        BulletCount,
        Health,
        MovementSpeed,
        ZAxisMovementSpeed,
        PlayerSize,
        DashSpeed,
        DashCooldown,
        CameraView,
    }
    private PlayerStats playerStats;
    void Start()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
        // currentStats = playerStats.CurrentStats;
    }



}
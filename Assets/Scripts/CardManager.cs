using System.Collections.Generic;
using UnityEngine;

public class CrdMngr : MonoBehaviour
{
    public static List<string> Rarities = new List<string> 
        {
            "Good",
            "Great",
            "Epic",
            "Mythical",
            "Godly"
        };

    public static List<int> RarityWeights = new List<int> 
        {
            10,
            8,
            5,
            3,
            1
        };
    public static List<float> RarityValues = new List<float> 
        {
            0.05f,
            0.1f,
            0.25f,
            0.5f,
            0.8f
        };

    public enum Stats
    {
        FireRate, 
        Damage, 
        BulletSpeed, 
        BulletSize,
        BulletCount,
        BulletRange,
        MaxHealth,
        MovementSpeed
    }
    private PlayerStats playerStats;
    void Start()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
    }



}
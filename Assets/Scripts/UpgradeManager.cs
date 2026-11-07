using System.Collections;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public int currentLevel;
    public float currentXp;
    public float XpRequirement;
    [SerializeField] public float levelOneXp;
    [SerializeField] public float XpPerLevelMultiplier;
    [SerializeField] public GameObject cardSpawnerObj;
    public static bool isWaitingForUpgradeChoice;
    
    void Start()
    {
        currentLevel = 1;
        currentXp = 0f;
        EnemyManager.AnnounceWave += NewWave;
        isWaitingForUpgradeChoice = false;
    }

    private void NewWave(int wave)
    {
        if(currentXp >= XpRequirement)
        {
            LevelUp();
        }
        else
        {
            isWaitingForUpgradeChoice = false;
        }
    }

    public void GainXp(float xpGained)
    {
        currentXp += xpGained;
    }

    private void LevelUp()
    {
        currentXp -= XpRequirement;
        currentLevel++;
        XpRequirement = levelOneXp * XpPerLevelMultiplier;
        GameObject cardSpawner = Instantiate(cardSpawnerObj);
        cardSpawner.GetComponent<CardSpawner>().SpawnCards();
    }
}

using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI enemiesLeftTextObj;
    [SerializeField] private TextMeshProUGUI healthTextObj;
    [SerializeField] private TextMeshProUGUI XpTextObj;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private UpgradeManager upgradeManager;
    private HealthManager healthManager;

    void Start()
    {
        healthManager = FindAnyObjectByType<HealthManager>().GetComponent<HealthManager>();
    }
    void Update()
    {
        string enemiesRemainingText = "Enemies Remaining: ";
        enemiesRemainingText += enemyManager.enemyCount;
        enemiesLeftTextObj.SetText(enemiesRemainingText);

        string healthText = "Health: ";
        healthText += Mathf.RoundToInt(healthManager.Health);
        healthText += " / ";
        healthText += Mathf.RoundToInt(healthManager.MaxHealth);
        healthTextObj.SetText(healthText);

        string xpText = "Current XP: ";
        xpText += Mathf.RoundToInt(upgradeManager.currentXp);
        xpText += " / ";
        xpText += Mathf.RoundToInt(upgradeManager.XpRequirement);
        xpText += "\nCurrent Level: ";
        xpText += upgradeManager.currentLevel;
        XpTextObj.SetText(xpText);
    }
}

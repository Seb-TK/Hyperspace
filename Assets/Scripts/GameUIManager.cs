using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI enemiesLeftTextObj;
    [SerializeField] private TextMeshProUGUI healthTextObj;
    [SerializeField] private EnemyManager enemyManager;
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
        healthText += healthManager.Health;
        healthTextObj.SetText(healthText);
    }
}

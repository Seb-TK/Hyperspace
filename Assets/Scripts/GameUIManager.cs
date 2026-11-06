using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI enemiesLeftTextObj;
    [SerializeField] EnemyManager enemyManager;

    void Update()
    {
        string enemiesRemainingText = "Enemies Remaining: ";
        enemiesRemainingText += enemyManager.enemyCount;
        enemiesLeftTextObj.SetText(enemiesRemainingText);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    public GameObject worldPlane;

    void Awake()
    {
        GameObject player = GameObject.Instantiate(playerPrefab);
        ResetStatics();
    }

    void ResetStatics()
    {
        Card.ResetStatics();
        EnemyManager enemyManager = FindAnyObjectByType<EnemyManager>();
        enemyManager.ResetStatics();
        UpgradeManager upgradeManager = FindAnyObjectByType<UpgradeManager>();
        upgradeManager.ResetStatics();
    }

    public void OnPlayerDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnExitToMenu()
    {
        ResetStatics();
        SceneManager.LoadScene("MainMenu");
    }

}

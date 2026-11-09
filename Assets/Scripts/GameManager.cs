using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    public GameObject worldPlane;

    void Awake()
    {
        GameObject player = GameObject.Instantiate(playerPrefab);
    }

    public void OnPlayerDeath()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

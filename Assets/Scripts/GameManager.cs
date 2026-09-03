using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    public GameObject worldPlane;

    void Awake()
    {
        GameObject player = GameObject.Instantiate(playerPrefab, worldPlane.transform);
    }

}

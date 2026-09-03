using Unity.VisualScripting;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private EnemyData EnemyDataComponent;
    private EnemyDataScriptableObject data;
    private GameObject mesh;
    private GameObject player;
    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>().gameObject;

        EnemyDataComponent = GetComponent<EnemyData>();
        data = EnemyDataComponent.EnemyDataScriptableObject;
    }
}

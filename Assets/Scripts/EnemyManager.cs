using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private List<EnemyDataScriptableObject> enemyDataList;

    void Start()
    {
        SpawnEnemy(0);
    }

    private void SpawnEnemy(int enemyDataIndex)
    {

        GameObject SpawnedEnemy = Instantiate(EnemyPrefab, new Vector3(3,0,0), quaternion.identity);
    }
}

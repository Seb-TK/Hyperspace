using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject SpawnCirclePrefab;
    [SerializeField] private List<EnemyDataScriptableObject> enemyDataList;

    void Start()
    {
        SpawnEnemy(0);
    }

    private void SpawnEnemy(int enemyDataIndex)
    {
        GameObject SpawnCircle = Instantiate(SpawnCirclePrefab, new Vector3(3,0,0), quaternion.identity);
        GameObject SpawnedEnemy = Instantiate(EnemyPrefab, new Vector3(3,0,0), quaternion.identity);
        SpawnedEnemy.SetActive(false);
        SpawnCircle.GetComponent<SpawnCircle>().Enemy = SpawnedEnemy;
    }

    

    IEnumerator SpawnWave(float spawnRate, int enemyCount)
    {
        int enemyListLength = enemyDataList.Count;
        for(int i = 0; i < enemyCount; i++)
        {
            yield return new WaitForSeconds(spawnRate);
            int randomEnemyInt = UnityEngine.Random.Range(0,enemyListLength);
            SpawnEnemy(enemyListLength);
        }
    }
}

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
        EdgeWrapping edgeWrapping = FindAnyObjectByType<EdgeWrapping>();
        float boundDimensions = 1f;
        float[] boundList = edgeWrapping.FindBoundPos(boundDimensions, boundDimensions);
        float spawnRangeX = boundList[0];
        float spawnRangeY = boundList[1];

        WorldPlane worldPlane = FindAnyObjectByType<WorldPlane>();
        float dimensionRange = worldPlane.dimensionRange;
        float spawnRangeZ = UnityEngine.Random.Range(-dimensionRange, dimensionRange);

        GameObject SpawnCircle = Instantiate(SpawnCirclePrefab, new Vector3(spawnRangeX,spawnRangeY,spawnRangeZ), quaternion.identity);
        GameObject SpawnedEnemy = Instantiate(EnemyPrefab, new Vector3(spawnRangeX,spawnRangeY,spawnRangeZ), quaternion.identity);
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

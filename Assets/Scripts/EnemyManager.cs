using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject SpawnCirclePrefab;
    
    [SerializeField] private int ChosenEnemy;
    
    [SerializeField] private List<EnemyDataScriptableObject> enemyDataList;
    [SerializeField] private EdgeWrapping edgeWrapping;

    [SerializeField] private float waitTimeMax;
    [SerializeField] private float waitTimeMin;
    void Start()
    {
        SpawnEnemy(ChosenEnemy, Vector3.zero);
    }

    IEnumerator SpawnLoop()
    {
        while (true) // while game running
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(waitTimeMin,waitTimeMax));
            Vector3 spawnLocation = EnemySpawnLocationSetup();
            int enemyType = UnityEngine.Random.Range(0,enemyDataList.Count);
            SpawnEnemy(enemyType, spawnLocation);
        }
        
    }

    private Vector3 EnemySpawnLocationSetup()
    {
        edgeWrapping = EnemyPrefab.GetComponent<EdgeWrapping>();
        float boundDimensions = 1f;
        float[] boundList = edgeWrapping.FindBoundPos(boundDimensions, boundDimensions);
        float spawnRangeX = boundList[0];
        float spawnRangeY = boundList[1];

        WorldPlane worldPlane = FindAnyObjectByType<WorldPlane>();
        float dimensionRange = worldPlane.dimensionRange;
        float spawnRangeZ = UnityEngine.Random.Range(-dimensionRange, dimensionRange);
        return new Vector3(spawnRangeX,spawnRangeY,spawnRangeZ);
    }
    private void SpawnEnemy(int enemyDataIndex, Vector3 spawnLocation)
    {
        GameObject SpawnCircle = Instantiate(SpawnCirclePrefab, spawnLocation, quaternion.identity);
        GameObject SpawnedEnemy = Instantiate(EnemyPrefab, spawnLocation, quaternion.identity);
        EnemyData enemyData = SpawnedEnemy.GetComponent<EnemyData>();
        enemyData.EnemyDataContainer = enemyDataList[enemyDataIndex];
        GameObject enemyVisualmeshPrefab = enemyDataList[enemyDataIndex].enemyVisualMesh;
        Instantiate(enemyVisualmeshPrefab, SpawnedEnemy.transform);

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
            SpawnEnemy(enemyListLength, EnemySpawnLocationSetup());
        }
    }
}



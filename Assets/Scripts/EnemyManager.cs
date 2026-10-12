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
    [SerializeField] private List<EnemyDataObj> enemyDataList;
    [SerializeField] private EdgeWrapping edgeWrapping;
    [SerializeField] private float waitTimeMax;
    [SerializeField] private float waitTimeMin;
    [SerializeField] private float spawnDelay;
    [SerializeField] private float defaultEnergy;
    [SerializeField] private float energyPerWave;
    [SerializeField] private float waveWaitTime;
    private int wave;
    private float startTime;


    void Start()
    {
        //SpawnEnemy(ChosenEnemy, new Vector3(0,0,0));
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        startTime = Time.time;
        while (true) // while game running
        {
            wave++;
            float energy = defaultEnergy + wave * energyPerWave;
            SpawnWave(energy);
            yield return new WaitForSeconds(waveWaitTime);
        }
    }

    private Vector3 EnemySpawnLocationSetup()
    {
        edgeWrapping = EnemyPrefab.GetComponent<EdgeWrapping>();
        float boundDimensions = 1f;
        float[] boundList = edgeWrapping.FindBoundPos(boundDimensions, boundDimensions);
        float spawnRangeX = boundList[0];
        float spawnRangeY = boundList[1];
        float spawnPointX = UnityEngine.Random.Range(-spawnRangeX, spawnRangeX);
        float spawnPointY = UnityEngine.Random.Range(-spawnRangeY, spawnRangeY);

        WorldPlane worldPlane = FindAnyObjectByType<WorldPlane>();
        float dimensionRange = worldPlane.dimensionRange;
        float spawnPointZ = UnityEngine.Random.Range(-dimensionRange, dimensionRange);
        return new Vector3(spawnPointX,spawnPointY,spawnPointZ);
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
    

    IEnumerator SpawnWave(float energy)
    {
        int enemyListLength = enemyDataList.Count;
        while(energy >= 0)
        {
            yield return new WaitForSeconds(spawnDelay);
            int randomEnemyInt;
            EnemyDataObj chosenEnemy;
            int energyCost;
            do {
                randomEnemyInt = UnityEngine.Random.Range(0,enemyListLength);
                chosenEnemy = enemyDataList[randomEnemyInt];
                energyCost = chosenEnemy.energyCost;
            } while(energyCost > energy & wave <= chosenEnemy.waveUnlocked);
            energy -= energyCost;
            SpawnEnemy(randomEnemyInt, EnemySpawnLocationSetup());
        }
    }
}



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
    [SerializeField] private int defaultEnergy;
    [SerializeField] private int energyPerWave;
    [SerializeField] private float waveWaitTime;
    private int wave;
    public int enemyCount;
    private bool temp;


    void Start()
    {
        //SpawnEnemy(ChosenEnemy, new Vector3(0,0,0));
        StartCoroutine(SpawnLoop());
        wave = 10;
    }

    IEnumerator SpawnLoop()
    {
        while (true) // while game running
        {
            wave++;
            int energy = defaultEnergy + wave * energyPerWave;
            yield return StartCoroutine(SpawnWave(energy));
            yield return new WaitUntil(() => enemyCount == 0);
            yield return new WaitForSeconds(waveWaitTime);
        }
    }


    private Vector3 EnemySpawnLocationSetup()
    {
        edgeWrapping = EnemyPrefab.GetComponent<EdgeWrapping>();
        WorldPlane worldPlane = FindAnyObjectByType<WorldPlane>();
        float dimensionRange = worldPlane.dimensionRange;
        float spawnRangeX = worldPlane.boundWidth;
        float spawnRangeY = worldPlane.boundHeight;
        float spawnPointX = UnityEngine.Random.Range(-spawnRangeX, spawnRangeX);
        float spawnPointY = UnityEngine.Random.Range(-spawnRangeY, spawnRangeY);

        float spawnPointZ = UnityEngine.Random.Range(-dimensionRange, dimensionRange);
        return new Vector3(spawnPointX,spawnPointY,spawnPointZ);
    }
    private void SpawnEnemy(int enemyDataIndex, Vector3 spawnLocation)
    {
        enemyCount++;
        GameObject SpawnCircle = Instantiate(SpawnCirclePrefab, spawnLocation, quaternion.identity);
        GameObject SpawnedEnemy = Instantiate(EnemyPrefab, spawnLocation, quaternion.identity);
        EnemyData enemyData = SpawnedEnemy.GetComponent<EnemyData>();
        enemyData.EnemyDataContainer = enemyDataList[enemyDataIndex];
        GameObject enemyVisualmeshPrefab = enemyDataList[enemyDataIndex].enemyVisualMesh;
        Instantiate(enemyVisualmeshPrefab, SpawnedEnemy.transform);

        SpawnedEnemy.SetActive(false);
        SpawnCircle.GetComponent<SpawnCircle>().Enemy = SpawnedEnemy;
    }

    IEnumerator SpawnWave(int inputEnergy)
    {
        //Debug.Log("wave spawned");
        int energy = inputEnergy;
        int enemyListLength = enemyDataList.Count;
        while(energy >= 0)
        {
            yield return new WaitForSeconds(spawnDelay);
            int randomEnemyInt;
            EnemyDataObj chosenEnemy;
            int energyCost;
            //Debug.Log("energy is at: " + energy);
            do {
                randomEnemyInt = UnityEngine.Random.Range(0,enemyListLength);
                chosenEnemy = enemyDataList[randomEnemyInt];
                energyCost = chosenEnemy.energyCost;
                yield return null;
                // Debug.Log("i picked: " + chosenEnemy);
                // Debug.Log("it had a cost of: " + energyCost);
            } while(energyCost > energy | wave < chosenEnemy.waveUnlocked);
            //Debug.Log("time to spawn, my energy went from: " + energy);
            energy -= energyCost;
            //Debug.Log("now it's: " + energy);
            SpawnEnemy(randomEnemyInt, EnemySpawnLocationSetup());
            // Debug.Log("Enemy spawned");
            // Debug.Log("");
        }
    }
}
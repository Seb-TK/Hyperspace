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
    public List<EnemyDataObj> currentEnemyList;
    private int wave;
    public int enemyCount;
    public static event Action<int> AnnounceWave;

    public void ResetStatics()
    {
        AnnounceWave = null;
    }

    void Start()
    {
        StartCoroutine(SpawnLoop());

        //test functions (disable together)
        // currentEnemyList = enemyDataList;
        // SpawnEnemy(ChosenEnemy, new Vector3(1,3,0));
    }

    IEnumerator SpawnLoop()
    {
        while (true) // while game running
        {
            wave++;
            int energy = defaultEnergy + wave * energyPerWave;
            yield return StartCoroutine(SpawnWave(energy));
            yield return new WaitUntil(() => enemyCount == 0);
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
        enemyData.EnemyDataContainer = currentEnemyList[enemyDataIndex];
        GameObject enemyVisualmeshPrefab = currentEnemyList[enemyDataIndex].enemyVisualMesh;
        Instantiate(enemyVisualmeshPrefab, SpawnedEnemy.transform);

        SpawnedEnemy.SetActive(false);
        SpawnCircle.GetComponent<SpawnCircle>().Enemy = SpawnedEnemy;
    }

    private int ChooseEnemy(int energy)
    {
        int randomEnemyInt;
        EnemyDataObj chosenEnemy;
        int energyCost;
        int currentEnergy = energy;
        do {
            randomEnemyInt = UnityEngine.Random.Range(0,currentEnemyList.Count);
            chosenEnemy = currentEnemyList[randomEnemyInt];
            energyCost = chosenEnemy.energyCost;
        } while(energyCost > energy);
        currentEnergy -= energyCost;
        SpawnEnemy(randomEnemyInt, EnemySpawnLocationSetup());
        return currentEnergy;
    }

    IEnumerator SpawnWave(int inputEnergy)
    {
        currentEnemyList.Clear();
        for(int i = 0; i < enemyDataList.Count; i++)
        {
            if (wave >= enemyDataList[i].waveUnlocked)
            {
                currentEnemyList.Add(enemyDataList[i]);
            }
        }
        UpgradeManager.isWaitingForUpgradeChoice = true;
        AnnounceWave?.Invoke(wave);
        yield return new WaitUntil(() => UpgradeManager.isWaitingForUpgradeChoice == false);
        yield return new WaitForSeconds(waveWaitTime);
        int energy = inputEnergy;
        while(energy > 0)
        {
            yield return new WaitForSeconds(spawnDelay);
            energy = ChooseEnemy(energy);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawner Variables")]
    [Tooltip("Put in you spawners")]
    public Spawner[] spawners;
    [Tooltip("Changing this will change the starting amount of enemies.")]
    public int enemySpawnTotal = 10;

     int enemyCount = 0;
     int spawnCount = 0;

    int round = 1;
    bool newRound = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (newRound)
        {
            round++;
            spawnCount = 0;
            newRound = false;
        }
        else if (newRound != true && enemyCount == 0)
        {
            newRound = true;
        }
    }

    public void IncrementCounts()
    {
        enemyCount++;
        spawnCount++;
    }

    public int GetEnemyCount()
    {
        return enemyCount;
    }

    public void DecrementEnemyCount()
    {
        enemyCount--;
    }

    public int GetSpawnCount()
    {
        return spawnCount;
    }
}

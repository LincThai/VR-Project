using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Tooltip("Put in the money manager")]
    public MoneyManager moneyManager;
    [Tooltip("Put in you spawners")]
    public Spawner[] spawners;
    [Header("Spawner Variables")]
    [Tooltip("Changing this will change the starting amount of enemies.")]
    public int enemySpawnTotal = 10;
    [Tooltip("Chance that a unit will be special. Change to affect starting chance.")]
    public int specialUnitChance = 0;
    [Tooltip("Max chance that a spawned unit will be special.")]
    public int maxSpecialUnitChance;
    [Tooltip("How much the chances of a special unit spawning will be increased by each round.")]
    public int specialUnitChanceIncrease = 1;
    [Tooltip("How much to scale up the total enemies spawned by each round. E.g 0.1 means it will increase by 10%")]
    public float enemySpawnTotalScale = 0.1f;

    public List<GameObject> enemyList = new List<GameObject>();
    int enemyCount = 0;
    int spawnCount = 0;

    bool newRound = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (newRound && spawnCount == enemySpawnTotal)
        {
            spawnCount = 0;
            newRound = false;

            enemySpawnTotal += (int)(enemySpawnTotal * enemySpawnTotalScale);

            if (specialUnitChance + specialUnitChanceIncrease > maxSpecialUnitChance)
            {
                
            }
            else if (specialUnitChance != maxSpecialUnitChance && specialUnitChance + specialUnitChanceIncrease > maxSpecialUnitChance)
            {
                specialUnitChance = maxSpecialUnitChance;
            }
            else
            {
                specialUnitChance += specialUnitChanceIncrease;
            }
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

    public void AddToEnemies(GameObject newEnemy)
    {
        enemyList.Add(newEnemy);
    }

    public void RemoveFromEnemies(GameObject enemy)
    {
        enemyList.Remove(enemy);
    }
}

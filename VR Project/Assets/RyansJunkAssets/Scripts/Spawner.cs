using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Manager Variables")]
    [Tooltip("The spawn manager that controls this spawner.")]
    public GameObject myManager;
    SpawnManager spawnManager;

    [Header("Spawning Variables")]
    [Tooltip("How far from the spawner enemies will be spawned.")]
    public float spawnRadius;
    [Tooltip("How often a minion will spawn in seconds.")]
    public float spawnInterval;
    [Header("Agent Variables")]
    [Tooltip("Enemies to be spawned.")]
    public GameObject[] enemyPrefabs;
    [Tooltip("Where the enemy will go to.")]
    public GameObject goal;

    GameObject newAgent;
    Move agentsMove;

    float time = 0.0f;

    int specialUnitChance = 0;
    int unitChanceValue = 0;
    int unitIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = myManager.GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= spawnInterval && spawnManager.GetSpawnCount() <= spawnManager.enemySpawnTotal)
        {
            float xOffset;
            float zOffset;
            Vector3 newPos = transform.position;

            bool goodCoords = false;
            while (goodCoords == false)
            {
                xOffset = Random.Range(-spawnRadius, spawnRadius);
                zOffset = Random.Range(-spawnRadius, spawnRadius);

                newPos.x += xOffset;
                newPos.z += zOffset;
                if (Vector3.Distance(transform.position, newPos) <= spawnRadius)
                {
                    goodCoords = true;
                }

            }

            unitChanceValue = Random.Range(0, 101);
            if (unitChanceValue <= specialUnitChance)
            {
                unitIndex = Random.Range(0, 3);
            }
            else
            {
                unitIndex = 0;
            }

            spawnManager.IncrementCounts();
            time = 0.0f;
            newAgent = Instantiate(enemyPrefabs[unitIndex], newPos, transform.rotation);
            agentsMove = newAgent.GetComponent<Move>();
            agentsMove.goal = goal;
        }
    }
}

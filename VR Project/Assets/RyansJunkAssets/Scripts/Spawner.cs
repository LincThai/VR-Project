using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawning Variables")]
    [Tooltip("How far from the spawner enemies will be spawned.")]
    public float spawnRadius;
    [Tooltip("How often a minion will spawn in seconds.")]
    public float spawnInterval;
    [Tooltip("How many minions will spawn in total.")]
    public int spawnAmount;
    [Header("Agent Variables")]
    [Tooltip("Enemies to be spawned.")]
    public GameObject[] enemyPrefabs;
    [Tooltip("Where the enemy will go to.")]
    public GameObject goal;

    GameObject newAgent;
    Move agentsMove;

    float time = 0.0f;
    [Header("Don't Change")]
    public int spawnCount = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= spawnInterval && spawnCount < spawnAmount)
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

            spawnCount++;
            time = 0.0f;
            newAgent = Instantiate(enemyPrefabs[0], newPos, transform.rotation);
            agentsMove = newAgent.GetComponent<Move>();
            agentsMove.goal = goal;
        }
    }
}

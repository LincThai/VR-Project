using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawners")]
    [Tooltip("Put in you spawners")]
    public Spawner[] spawners;

    bool spaceClicked = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            spaceClicked = true;
        }

        if (spaceClicked)
        {
            foreach (Spawner spawner in spawners)
            {
                spawner.spawnCount = 0;
            }
            spaceClicked = false;
        }
    }
}

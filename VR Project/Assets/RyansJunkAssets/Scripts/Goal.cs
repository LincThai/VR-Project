using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [Header("Spawn Manager")]
    [Tooltip("The spawn manager that controls the spawners.")]
    public GameObject theManager;
    SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = theManager.GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            spawnManager.DecrementEnemyCount();
        }
    }
}

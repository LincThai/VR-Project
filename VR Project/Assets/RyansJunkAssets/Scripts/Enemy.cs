using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpawnManager spawnManager;

    [Tooltip("Enemy health")]
    public float health;
    [Tooltip("How much gold the player gets for killing it.")]
    public int goldDrop;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            spawnManager.RemoveFromEnemies(gameObject);
            spawnManager.DecrementEnemyCount();
            Destroy(gameObject);
        }
    }

    public void SetManager(GameObject manager)
    {
        spawnManager = manager.GetComponent<SpawnManager>();
    }
}

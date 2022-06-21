using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpawnManager spawnManager;
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
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Projectile"))
        {
            //spawnManager.RemoveFromEnemies(gameObject);
            //Destroy(gameObject);
        }
    }

    public void SetManager(GameObject manager)
    {
        spawnManager = manager.GetComponent<SpawnManager>();
    }
}

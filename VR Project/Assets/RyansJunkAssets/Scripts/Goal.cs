using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [Header("Spawn Manager")]
    [Tooltip("The spawn manager that controls the spawners.")]
    public GameObject theManager;
    SpawnManager spawnManager;

    [Header("Goal Variables")]
    [Tooltip("Players bases' health.")]
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = theManager.GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene("LoseScene");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            spawnManager.DecrementEnemyCount();
            health -= collision.gameObject.GetComponent<Enemy>().damage;
        }
    }
}

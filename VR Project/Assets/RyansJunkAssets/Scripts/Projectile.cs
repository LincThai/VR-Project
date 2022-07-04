using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    SpawnManager spawnManager;

    List<GameObject> deleteList = new List<GameObject>();

    BezierCurve flightPath;
    float projectileSpeed;

    float blastRadius;
    float damage;

    float t = 0.0f;

    // Update is called once per frame
    void Update()
    {
        t = Mathf.Lerp(0.0f, 1.0f, t);
        transform.position = new Vector3(flightPath.FindX(t), flightPath.FindY(t), flightPath.FindZ(t));

        t += projectileSpeed * Time.deltaTime;
    }

    public void SetProjectileValues(Vector3 startPosition, Vector3 targetPosition, float controlPointHeight, float pSPeed)
    {
        flightPath = new BezierCurve(startPosition, new Vector3((startPosition.x + targetPosition.x) / 2, ((startPosition.y + targetPosition.y) / 2) + controlPointHeight, (startPosition.z + targetPosition.z) / 2), targetPosition);
        projectileSpeed = pSPeed;

        if (targetPosition == new Vector3((startPosition.x + targetPosition.x) / 2, ((startPosition.y + targetPosition.y) / 2) + controlPointHeight, (startPosition.z + targetPosition.z) / 2)) // wouldn't be surprised if this is bad
        {
            Destroy(gameObject);
        }
    }

    public void SetSpawnManager(SpawnManager sManager)
    {
        spawnManager = sManager;
    }

    public void SetBlastRadius(float blastRad)
    {
        blastRadius = blastRad;
    }

    public void SetDamage(float dam)
    {
        damage = dam;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Tower"))
        {
            if (spawnManager.enemyList.Count != 0)
            {
                Enemy enemyStats;
                if (blastRadius != 0)
                {
                    foreach (GameObject enemy in spawnManager.enemyList)
                    {
                        if (Vector3.Distance(transform.position, enemy.transform.position) <= blastRadius)
                        {
                            enemyStats = enemy.GetComponent<Enemy>();
                            enemyStats.health -= damage;
                            if (enemyStats.health <= 0)
                            {
                                deleteList.Add(enemy);
                            }
                        }
                    }
                }
                else
                {
                    enemyStats = collision.gameObject.GetComponent<Enemy>();
                    enemyStats.health -= damage;
                    if (enemyStats.health <= 0)
                    {
                        deleteList.Add(collision.gameObject);
                    }

                }
                if (deleteList.Count != 0)
                {
                    foreach (GameObject enemy in deleteList)
                    {
                        spawnManager.DecrementEnemyCount();
                        spawnManager.RemoveFromEnemies(enemy);
                        spawnManager.moneyManager.AddGold(enemy.GetComponent<Enemy>().goldDrop);
                        Destroy(enemy);
                    }
                }

            }
            Destroy(gameObject);
        }
    }
}

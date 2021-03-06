using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    SpawnManager spawnManager;

    public GameObject boomPrefab;

    List<GameObject> deleteList = new List<GameObject>();

    BezierCurve flightPath;
    float projectileSpeed;

    float blastRadius;
    float damage;

    AudioSource shellAudio;

    bool disabled = false;

    float t = 0.0f;
    Vector3 lastPos;
    void Start()
    {
        shellAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //t = Mathf.Lerp(0.0f, 1.0f, t);

        //t += projectileSpeed * Time.deltaTime;

        float distanceToTravel;

        distanceToTravel = Vector3.Distance(flightPath.GetStart(), flightPath.GetControlPoint()) + Vector3.Distance(flightPath.GetControlPoint(), flightPath.GetEnd());

        if (t <= 1.0f && blastRadius == 0)
        {
            t += projectileSpeed * Time.deltaTime * distanceToTravel;
        }
        else if (t <= 1.0f)
        {
            t += projectileSpeed * Time.deltaTime;
        }

        transform.position = new Vector3(flightPath.FindX(t), flightPath.FindY(t), flightPath.FindZ(t));

        if (lastPos == transform.position)
        {
            Destroy(gameObject);
        }

        lastPos = transform.position;

        if (transform.position == flightPath.GetEnd())
        {
            Destroy(gameObject);
        }
    }

    public void SetProjectileValues(Vector3 startPosition, Vector3 targetPosition, float controlPointHeight, float pSPeed)
    {
        flightPath = new BezierCurve(startPosition, new Vector3((startPosition.x + targetPosition.x) / 2, ((startPosition.y + targetPosition.y) / 2) + controlPointHeight, (startPosition.z + targetPosition.z) / 2), targetPosition);
        projectileSpeed = pSPeed;
        lastPos = startPosition;

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
        //if (collision.gameObject.CompareTag("Respawn"))
        //{
        //    blastRadius = 10;
        //    blastRadius -= 10;
        //}
            if (disabled == false)
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
                else if (collision.gameObject.CompareTag("Enemy"))
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

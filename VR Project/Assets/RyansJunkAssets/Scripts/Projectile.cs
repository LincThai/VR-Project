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
    }

    public void SetSpawnManager(SpawnManager sManager)
    {
        spawnManager = sManager;
    }

    public void SetBlastRadius(float blastRad)
    {
        blastRadius = blastRad;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Tower"))
        {
            if (spawnManager.enemyList.Count != 0)
            {
                foreach (GameObject enemy in spawnManager.enemyList)
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) <= blastRadius)
                    {
                        deleteList.Add(enemy);
                    }
                }
                if (deleteList.Count != 0)
                {
                    foreach (GameObject enemy in deleteList)
                    {
                        spawnManager.DecrementEnemyCount();
                        spawnManager.RemoveFromEnemies(enemy);
                        Destroy(enemy);
                    }
                }

            }
            Destroy(gameObject);
        }
    }
}

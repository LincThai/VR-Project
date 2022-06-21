using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tower : MonoBehaviour
{
    public GameObject theManager; // Being set in inspector for now but should be done in code later by whatever is used to make the towers
    SpawnManager spawnManager;

    public GameObject projectile;
    public float projectileSpeed;
    public float range;
    public float delay;
    public float blastRadius;

    BoxCollider boxCollider;

    float time = 0.0f;
    bool readyToShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = theManager.GetComponent<SpawnManager>();

        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (!readyToShoot && time >= delay)
        {
            time = 0.0f;
            readyToShoot = true;
        }

        if (readyToShoot)
        {
            if (spawnManager.enemyList.Count != 0)
            {
                GameObject target = null;
                float distance = float.MaxValue;
                foreach (GameObject enemy in spawnManager.enemyList)
                {
                    float d = Vector3.Distance(transform.position, enemy.transform.position);
                    if (d <= range && d < distance)
                    {
                        distance = d;
                        target = enemy;
                    }
                }
                if (target != null)
                {
                    Vector3 predictedPosition = target.transform.position + (target.GetComponent<NavMeshAgent>().velocity * target.GetComponent<NavMeshAgent>().speed);

                    GameObject p = Instantiate(projectile, new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.max.y, boxCollider.bounds.center.z), transform.rotation);

                    p.GetComponent<Projectile>().SetProjectileValues(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.max.y, boxCollider.bounds.center.z), predictedPosition, 200.0f, 0.2f);
                    p.GetComponent<Projectile>().SetSpawnManager(spawnManager);
                    p.GetComponent<Projectile>().SetBlastRadius(blastRadius);

                    readyToShoot = false;
                }
            }
        }
    }
}

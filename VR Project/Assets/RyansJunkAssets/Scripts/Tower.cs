using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tower : MonoBehaviour
{
    public GameObject theManager; // Being set in inspector for now but should be done in code later by whatever is used to make the towers
    SpawnManager spawnManager;

    [Header("Placement Variables")]
    [Tooltip("How much the turret costs")]
    public int cost = 1;

    [Header("Shooting Variables")]
    [Tooltip("Speed in which the projectile will fly at. 0.0 - 1.0")]
    public float projectileSpeed;
    [Tooltip("Determines how far the tower can shoot.")]
    public float range;
    [Tooltip("How long the tower will wait to shoot again.")]
    public float delay;
    [Tooltip("Set to 0 for straight shots. The larger the offset the larger the shot curves up.")]
    public float arcOffset;

    [Header("Projectile Variables")]
    [Tooltip("Prefab for desired projectile.")]
    public GameObject projectile;
    [Tooltip("Radius for projectile damage. Set to 0 for no aoe.")]
    public float blastRadius;
    [Tooltip("Projectile damage.")]
    public float damage;

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

                    p.GetComponent<Projectile>().SetProjectileValues(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.max.y, boxCollider.bounds.center.z), predictedPosition, arcOffset, 0.2f);
                    p.GetComponent<Projectile>().SetSpawnManager(spawnManager);
                    p.GetComponent<Projectile>().SetBlastRadius(blastRadius);
                    p.GetComponent<Projectile>().SetDamage(damage);

                    readyToShoot = false;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Tower : MonoBehaviour
{
    public SpawnManager spawnManager;

    [Header("Tower Variables")]
    [Tooltip("How much the turret costs")]
    public int cost = 1;
    [Tooltip("The percentage you are refunded")]
    public float refundPercent = 0.5f;
    [Tooltip("How long a tower range indicator will remain visible for after being hovered over")]
    public float rangeDisplayTimeLength = 5.0f;
    [Tooltip("Put in prefab of this turrets desired type")]
    public GameObject towerPrefab;
    [Tooltip("Put in textbox for tower cost")]
    public TextMeshProUGUI costDisplay;

    GameObject rangeIndicator;


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

    AudioSource boomAudio;

    BoxCollider boxCollider;

    Vector3 startPosition;

    float time = 0.0f;
    float rangeDisplayTimer = 0.0f;
    bool readyToShoot = true;
    bool active = true;
    bool used = false;

    // Start is called before the first frame update
    void Start()
    {
        boomAudio = GetComponent<AudioSource>();

        startPosition = transform.position;

        boxCollider = GetComponent<BoxCollider>();

        rangeIndicator = transform.GetChild(0).gameObject;

        rangeIndicator.transform.localScale = new Vector3(range / transform.localScale.x * 2, range / transform.localScale.z * 2, 1.0f);

        rangeIndicator.SetActive(false);

        if (costDisplay != null)
        {
            costDisplay.text = "Cost: " + cost;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rangeDisplayTimer > 0.0f && !rangeIndicator.active)
        {
            rangeIndicator.SetActive(true);
        }
        else if (rangeDisplayTimer <= 0.0f && rangeIndicator.active)
        {
            rangeIndicator.SetActive(false);
        }
        else if (rangeDisplayTimer > 0.0f)
        {
            rangeDisplayTimer -= Time.deltaTime;
        }

        if (active)
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
                        boomAudio.Play();

                        Vector3 predictedPosition = target.transform.position + (target.GetComponent<NavMeshAgent>().velocity * target.GetComponent<NavMeshAgent>().speed);

                        GameObject p = Instantiate(projectile, new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.max.y, boxCollider.bounds.center.z), transform.rotation);

                        p.GetComponent<Projectile>().SetProjectileValues(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.max.y, boxCollider.bounds.center.z), predictedPosition, arcOffset, 0.2f);
                        p.GetComponent<Projectile>().SetSpawnManager(spawnManager);
                        p.GetComponent<Projectile>().SetBlastRadius(blastRadius);
                        p.GetComponent<Projectile>().SetDamage(damage);

                        readyToShoot = false;
                        time = 0.0f;
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, range);
    }

    public void ActivateTower()
    {
        active = true;
    }

    public void DeactivateTower()
    {
        active = false;
    }

    public void ResetRangeDisplayTimer()
    {
        rangeDisplayTimer = rangeDisplayTimeLength;
    }

    public Vector3 GetStartingPosition()
    {
        return startPosition;
    }

    public bool isUsed()
    {
        return used;
    }

    public void setAsUsed()
    {
        used = true;
    }
}

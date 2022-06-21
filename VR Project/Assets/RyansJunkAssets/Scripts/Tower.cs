using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tower : MonoBehaviour
{
    public GameObject theManager; // Being set in inspector for now but should be done in code later by whatever is used to make the towers
    SpawnManager spawnManager; 

    public float projectileFlightTime;
    public float range;

    public GameObject thing;

    Vector3 predictedPosition;

    BoxCollider boxCollider;
    
    BezierCurve shotCurve;

    bool readyToShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        shotCurve = new BezierCurve(new Vector3 (46.2f, 20.0f, -73.0f), new Vector3((46.2f + 20) / 2, 100, (-73.0f + 20) / 2), new Vector3(20, 0, 20));

        for (float i = 0; i < 1.0f; i += 0.01f)
        {
            Vector3 location = new Vector3(shotCurve.FindX(i), shotCurve.FindY(i), shotCurve.FindZ(i));
            Instantiate(thing, location, transform.rotation);
        }

        spawnManager = theManager.GetComponent<SpawnManager>();
        //rangeBoi.transform.position = new Vector3(transform.position.x + range, rangeBoi.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //if (readyToShoot && spawnManager.enemyList.Count != 0)
        //{
        //    float distance = float.MaxValue;
        //    GameObject target = null;
        //    foreach (GameObject enemy in spawnManager.enemyList)
        //    {
        //        if (Vector3.Distance(enemy.transform.position, gameObject.transform.position) <= range && Vector3.Distance(enemy.transform.position, gameObject.transform.position) < distance)
        //        {
        //            distance = Vector3.Distance(enemy.transform.position, gameObject.transform.position);
        //            target = enemy;
        //        }
        //    }
        //    if (target != null)
        //    {
        //        predictedPosition += target.GetComponent<NavMeshAgent>().velocity * projectileFlightTime;

        //        Vector3 planarTarget = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        //        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

        //       // float yOffset = 

        //        readyToShoot = false;
        //    }
        //}
    }
}

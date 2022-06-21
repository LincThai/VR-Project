using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    BezierCurve flightPath;
    float projectileSpeed;

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
        flightPath = new BezierCurve(startPosition, new Vector3((startPosition.x + targetPosition.x) / 2, controlPointHeight, (startPosition.z + targetPosition.z) / 2), targetPosition);
        projectileSpeed = pSPeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Tower"))
        {
            Destroy(gameObject);
        }
    }
}

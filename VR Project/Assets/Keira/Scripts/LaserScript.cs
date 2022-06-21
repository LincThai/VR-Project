using UnityEngine;
using System.Collections;

//Taken from https://answers.unity.com/questions/1142318/making-raycast-line-visibld.html
//Edited by me

public class LaserScript : MonoBehaviour
{

    public Transform handPos;

    LineRenderer laserLineRenderer;
    public float laserWidth = 0.1f;
    public float laserMaxLength = 5f;

    public bool laserActive = true;


    void Start()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        //laserLineRenderer.SetWidth(laserWidth, laserWidth);
        laserLineRenderer.startWidth = laserWidth * 2;
        laserLineRenderer.endWidth = laserWidth;
    }

    void Update()
    {
        if (laserActive)
        {
            laserLineRenderer.enabled = true;
        }
        else
        {
            laserLineRenderer.enabled = false;
        }
    }

    public void SetLaserPosition(float length)
    {

        laserLineRenderer.SetPosition(0, handPos.position);
        laserLineRenderer.SetPosition(1, handPos.position + (handPos.forward * length));
    }
}
using UnityEngine;
using System.Collections;

//Taken from https://answers.unity.com/questions/1142318/making-raycast-line-visibld.html
//Edited by me

public class LaserScript : MonoBehaviour
{

    public Transform handPos;

    public float laserWidth = 0.1f;
    public float laserStartDistance = 2f;
    public float laserMaxDistance = 250f;

    [Tooltip("Sets the colour in the line renderer, but doesnt change the colour of the line being rendered... no idea.")]
    public Color laserColour = Color.cyan;

    public bool laserActive = true;

    LineRenderer laserLineRenderer;

    void Start()
    {
        laserLineRenderer = this.gameObject.AddComponent<LineRenderer>();
        
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        
        laserLineRenderer.startWidth = laserWidth * 2;
        laserLineRenderer.endWidth = laserWidth;
        SetLaserColour(laserColour);
    }

    void Update()
    {

        //turn this into a get set? so it doesnt run every frame and only on update.
        if (laserActive)
        {
            laserLineRenderer.enabled = true;
        }
        else
        {
            laserLineRenderer.enabled = false;
        }

        SetLaserColour(laserColour);
    }

    //Sets the laser from the hand position, in the direction the hand is facing, for the input length
    public void SetLaserDistFromHand(float length)
    {
        SetLaserPos(handPos.position + (handPos.forward * laserStartDistance), handPos.position + (handPos.forward * length));
    }

    public void SetLaserPos(Vector3 startPos, Vector3 endPos)
    {
        laserLineRenderer.SetPosition(0, startPos);
        laserLineRenderer.SetPosition(1, endPos);
    }

    //no worky :(
    public void SetLaserColour(Color colour)
    {
        laserLineRenderer.startColor = colour;
        laserLineRenderer.endColor = colour;    
    }


}
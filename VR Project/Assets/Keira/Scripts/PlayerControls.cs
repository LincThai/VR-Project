using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    public GameObject leftHandTransform;
    public GameObject rightHandTransform;

    public GameObject node;

    Controls controls;

    Gamepad gamepad;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        
        
    }

    public void PlsWork()
    {
        node.transform.position = leftHandTransform.transform.position + leftHandTransform.transform.forward * 10;
        
    }

}


class Controls
{
    public UnityEngine.XR.InputDevice leftHand;
    public UnityEngine.XR.InputDevice rightHand;


    bool controlsFound = false;

    public Controls()
    {
        GrabControllers();
    }

    void GrabControllers()
    {
        List<UnityEngine.XR.InputDevice> leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        List<UnityEngine.XR.InputDevice> rightHandDevices = new List<UnityEngine.XR.InputDevice>();

        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);

        if (leftHandDevices.Count == 1 && rightHandDevices.Count == 1)
        {
            leftHand = leftHandDevices[0];
            rightHand = rightHandDevices[0];

            Debug.Log(string.Format("LeftHand Controller name '{0}' with role '{1}'", leftHand.name, leftHand.role.ToString()));
            Debug.Log(string.Format("RightHand Controller name '{0}' with role '{1}'", leftHand.name, leftHand.role.ToString()));

            controlsFound = true;
        }
        else if (leftHandDevices.Count > 1)
        {
            Debug.Log("Too many left hand controllers found: " + leftHandDevices.Count + " found!");
        }
        else if (rightHandDevices.Count > 1)
        {
            Debug.Log("Too many right hand controllers found: " + rightHandDevices.Count + " found!");
        }
        else if (leftHandDevices.Count < 1)
        {
            Debug.Log("No left hand controller found!");
        }
        else if (rightHandDevices.Count < 1)
        {
            Debug.Log("No right hand controller found!");
        }
    }
}
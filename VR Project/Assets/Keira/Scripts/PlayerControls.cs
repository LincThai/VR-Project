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
        gamepad = Gamepad.current;
        controls = new Controls();


    }

    // Update is called once per frame
    void Update()
    {
        if (gamepad.leftShoulder.IsPressed())
        {
            PlsWork();
        }
        


        bool triggerValueLeft = false;
        if (controls.leftHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValueLeft) && triggerValueLeft)
        {
            Debug.Log("Trigger on left pressed");
            node.transform.position = leftHandTransform.transform.position;
        }
        bool triggerValueRight = false;
        if (controls.leftHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValueRight) && triggerValueRight)
        {
            Debug.Log("Trigger on right pressed");
            node.transform.position = rightHandTransform.transform.position;
        }
    }

    public void PlsWork()
    {
        node.transform.position = leftHandTransform.transform.position;
        Debug.Log("Text?");
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
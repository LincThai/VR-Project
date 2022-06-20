using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;

public class InteractingController : MonoBehaviour
{

    public float radius = 0.1f;
    public float setMaxDistFromHand = 0.5f;

    public float maxDistFromHand = 25;
    public float minDistFromHand = 0.1f;

    public float moveSpeed = 0.5f;

    public Controller leftHand;
    public Controller rightHand;
    public GameObject node;




    // Start is called before the first frame update
    void Start()
    {
        //gets left controller
        List<InputDevice> inputDeviceLeft = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, inputDeviceLeft);
        leftHand.device = inputDeviceLeft[0];
        //gets right controller
        List<InputDevice> inputDeviceRight = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, inputDeviceRight);
        rightHand.device = inputDeviceRight[0];





    }

    // Update is called once per frame
    void Update()
    {

        //Physics.Raycast(Vec3 origin, Vec3 direction, out RaycastHit info, float maxDist)
        RaycastHit hit;
        //if (Physics.Raycast())


    }

    Vector2 GetAxisPos2D(Controller controller)
    {
        Vector2 axisPos2D = Vector2.zero;

        controller.device.TryGetFeatureValue(CommonUsages.primary2DAxis, out axisPos2D);

        return axisPos2D;
    }

    float GetAxisPosX(Controller controller)
    {
        Vector2 axis2D = GetAxisPos2D(controller);
        Vector2 axis2DAbs = new Vector2(Mathf.Abs(axis2D.x), Mathf.Abs(axis2D.y)); 
        float axisX = 0;

        if (axis2DAbs.x <= axis2DAbs.y)
        {
            return 0;
        } else
        {
            return axis2D.x;
        }
    }

    public void tempOnTouch()
    {
        Vector2 Axis2D = GetAxisPos2D(leftHand);
        Vector3 Axis3D = new Vector3(Axis2D.x, 0, Axis2D.y);
        node.transform.position = leftHand.transform.position + (Axis3D * 10);
    }

    //Moves the "interact" ball forwards and backwards for max 'reach'
    public void OnAxis2DTouch()
    {
        float axisX = GetAxisPosX(leftHand);

        setMaxDistFromHand = Mathf.Clamp(axisX * moveSpeed * Time.deltaTime, minDistFromHand, maxDistFromHand);

        SetInteractBallDist();
    }


    void SetInteractBallDist()
    {
        Vector3 pos = new Vector3(setMaxDistFromHand, 0, 0);
        this.transform.localPosition = pos;

    }

    void TryToGrab()
    {

    }

}

public class Controller
{

    public InputDevice device;
    public Transform transform;
}
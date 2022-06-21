using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;

public class InteractingController : MonoBehaviour
{
    
    public float radius = 0.1f;


    public float maxRaycastLength = 150;

    float interactBallDistFromHand;
    public float minDistFromHand = 0.1f;

    public float moveSpeed = 2.5f;
    
    public Controller leftHand;
    public Controller rightHand;
    public GameObject node;
    
    //if object is currently grabbed
    bool objectGrabbed;


    MeshRenderer ballRenderer;

    //Was the interact button held last frame?
    bool interactButtonHeldLastFrame;

    //Last object hit with raycast, alongside "objecthitlastcheck" for AttemptInteract() to use if the button is pressed
    GameObject lastObjectHit;
    bool objectHitLastCheck;

    //Laser visual
    LaserScript laser;

    // Start is called before the first frame update
    void Start()
    {

        laser = this.GetComponent<LaserScript>();

        interactBallDistFromHand = maxRaycastLength;

        //Get ball renderer on this object
        ballRenderer = this.GetComponent<MeshRenderer>();
        ballRenderer.enabled = false;

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

        laser.SetLaserPosition(150.0f);
        laser.laserActive = true;

        //Physics.Raycast(Vec3 origin, Vec3 direction, out RaycastHit info, float maxDist)
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.forward, out hit, maxRaycastLength, LayerMask.GetMask("Interactable")))
        {

            ballRenderer.enabled = true;
            lastObjectHit = hit.rigidbody.gameObject;

            if (gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                objectHitLastCheck = true;
            }
            else
            {
                objectHitLastCheck = false;
            }


        } else
        {
            ballRenderer.enabled = false;
            objectHitLastCheck = false;
        }


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
        if (objectGrabbed)
        {
            float axisX = GetAxisPosX(leftHand);

            interactBallDistFromHand = Mathf.Clamp(axisX * moveSpeed * Time.deltaTime, minDistFromHand, maxRaycastLength);

            SetInteractBallDist(interactBallDistFromHand);
        }
    }


    void SetInteractBallDist(float distance)
    {
        this.transform.localPosition = Vector3.forward * distance;

        //also needs to set pos of object held
    }

    //When the "grab" button is held down, attempt to grab
    public void AttemptInteract()
    {
        SetInteractBallDist(10);
    }

    //When you let go of "grab"
    public void StopInteract()
    {

    }
}

public class Controller
{
    [SerializeField]
    public InputDevice device;
    public Transform transform;
}
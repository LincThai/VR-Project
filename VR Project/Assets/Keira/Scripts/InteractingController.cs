using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;

public class InteractingController : MonoBehaviour
{

    [Header("General Raycast settings")]

    [Tooltip("Max distance of raycast")]
    public float maxRaycastLength = 150;
    [Tooltip("The last item hit by a raycast can still be grabbed if nothing has been looking at for x time")]
    [Range(0f, 5f)]
    public float maxTimeToGrab = 0.5f;
    [Tooltip("Max distance from node to 'grab' the object being held")]
    public float nodeGrabDistance = 15.0f;
    [Tooltip("")]
    public float turretHoverDistance = 25.0f;

    [Header("Tag and Layer names")]
    [Tooltip("Tag name of Nodes")]
    string nodeTag = "Node";


    [Header("Scene objects to set")]

    [Tooltip("Left Hand Controller")]
    public Controller leftHand;
    [Tooltip("Right Hand Controller")]
    public Controller rightHand;
    [Tooltip("The sphere childed to this hand")]
    public GameObject ballChild;
    [Tooltip("Temp node I am using to see position of certain things")]
    public GameObject node;



    //ball thing - dumb ball renderer wont let me turn it off :(
    MeshRenderer ballRenderer;



    //Raycast data - Data needed to properly process grabbings items
    bool isObjectHeld = false;
    bool interactButtonHeldLastFrame;
    GameObject objectBeingHeld = null;
    //Closest node to item being held
    GameObject closestNode = null;
    //if the closest node is close enough that when the turret is let go of, it can go to it.
    bool isClosestNodeValid = false;


    //Last object hit with raycast, alongside "objectHitLastCheck" for AttemptInteract() to use if the button is pressed
    GameObject lastObjectHit; //Object that was last hit by raycast
    bool objectHitLastCheck; //if an object was hit last time the raycast happened
    float timeSinceLastRaycastHit = 0; //Alternative that might be used to object hit last raycast check,
                                       //as it will allow a person to "grab" an object they just stopped touching

    //Laser visual
    LaserScript laser;

    // Start is called before the first frame update
    void Start()
    {

        laser = this.GetComponent<LaserScript>();

        //Get ball renderer on this object
        ballRenderer = ballChild.GetComponent<MeshRenderer>();
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

        if (!isObjectHeld)
        {
            //raycast to do when no item is held
            GeneralRaycast();
            
        }
        else
        {
            //What do to when an item is held
            RaycastWhenItemHeld();

            
        }


        
        
    }

    void GeneralRaycast ()
    {
        laser.laserActive = true;
        //When holding an item, it needs to do a second raycast from this items position in the same direction to position it just above the surface
        //Physics.Raycast(Vec3 origin, Vec3 direction, out RaycastHit info, float maxDist)
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, maxRaycastLength))//, LayerMask.GetMask("Interactable")))
        {

            //Only care about what we hit if it is an 'interactable', but still need the raycast data if it hit something else
            if (lastObjectHit.layer == LayerMask.NameToLayer("Interactable"))
            { 
                lastObjectHit = hit.transform.gameObject;
                objectHitLastCheck = true;
                timeSinceLastRaycastHit = 0;
            }
            else
            {
                objectHitLastCheck = false;
                timeSinceLastRaycastHit += Time.deltaTime;
            }


            ballRenderer.enabled = true;
            laser.SetLaserDistFromHand(hit.distance);
            SetInteractBallDist(hit.distance);

        }
        else
        {
            timeSinceLastRaycastHit += Time.deltaTime;
            //node.transform.position = this.transform.position + this.transform.forward * 10;
            SetInteractBallDist(maxRaycastLength);
            ballRenderer.enabled = false;
            objectHitLastCheck = false;
            laser.SetLaserDistFromHand(25.0f);

        }
    }

    void RaycastWhenItemHeld()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, maxRaycastLength, LayerMask.GetMask("Interactable")))
        {
            /* 
             * Get point on ground of where turret currently is
             * Check for closest nodes, if closest in range is used
             * check for second closest, if in range and used,
             * etc etc until you find one in range unused or none remain in range
             * if one is in range, put turret above.
             */

            Vector3 raycastpointtouselater = hit.point;
            float distance;

            GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
            GameObject closest = GetClosestOpenNode(nodes, raycastpointtouselater, out distance);

            if (closest != null && nodeGrabDistance > distance)
            {
                //Sets turret to hover above node
                SetHeldItemPosition(closest.transform.position + Vector3.up * turretHoverDistance);
                isClosestNodeValid = true;
            }
            else
            {
                //Sets turret to hover over point it is at
                SetHeldItemPosition(hit.point + Vector3.up * turretHoverDistance);
                isClosestNodeValid = false;
            }


        } else 
        {
            SetHeldItemPosition(this.transform.position + (this.transform.forward * maxRaycastLength));
            isClosestNodeValid = false;
        }
    }

    void SetHeldItemPosition(Vector3 position)
    {
        objectBeingHeld.transform.position = position; 
    }

    GameObject GetClosestOpenNode(GameObject[] nodes, Vector3 position, out float outDistance)
    {
        float closestDist = float.MaxValue;

        GameObject closestOpenNode = null;

        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].GetComponent<Node>().nodeAvailable)
            {
                float distance = Vector3.Distance(nodes[i].transform.position, position);
                if (closestDist > distance)
                {
                    closestDist = distance;
                    closestOpenNode = nodes[i];
                }

            }
        }
        outDistance = closestDist;
        return closestOpenNode;
    }


    //When the "grab" button is held down, attempt to grab
    public void AttemptInteract()
    {
        interactButtonHeldLastFrame = true;
        if (maxTimeToGrab > timeSinceLastRaycastHit && !isObjectHeld)
        {
            //'grab' item
            if (lastObjectHit.CompareTag("Turret"))
            {
                Interactable turret = lastObjectHit.GetComponent<Interactable>();
                if (turret.CanBeGrabbed())
                {
                    isObjectHeld = true;
                    objectBeingHeld = lastObjectHit;
                }

            }
        }
        else
        {

        }
        //SetInteractBallDist(10);
    }

    //When you let go of "grab"
    public void StopInteract()
    {
        interactButtonHeldLastFrame = false;
        if (isObjectHeld)
        {
            isObjectHeld = false;
            Interactable turret = objectBeingHeld.GetComponent<Interactable>();
            //tell closest node to take turret, tell turret it has been placed, if no closest node, tell turret it was dropped.
            //Turret or node need to set turrets position to the node.
            if (isClosestNodeValid)
            {
                turret.SetToNode(closestNode);
                isClosestNodeValid = false;
            }
            else
            {
                turret.Voided();
            }

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
        //probably not going to be used       

        //if (objectGrabbed)
        //{
        //    float axisX = GetAxisPosX(leftHand);

        //    interactBallDistFromHand = Mathf.Clamp(axisX * moveSpeed * Time.deltaTime, minDistFromHand, maxRaycastLength);

        //    SetInteractBallDist(interactBallDistFromHand);
        //}
    }


    void SetInteractBallDist(float distance)
    {
        ballChild.transform.position = this.transform.position + this.transform.forward * distance;

        //also needs to set pos of object held
    }

    //Currently does nothing until I fix this up :(
    void SetInteractBallColour(Color colour)
    {
        //Need to figure out the proper way to change colour of (and also maybe size) of these via code
        //Dont really wanna make a whole shader exclusive to a ball on the end of a pointer ya know.
    }
}

public class Controller
{
    [SerializeField]
    public InputDevice device;
    public Transform transform;
}


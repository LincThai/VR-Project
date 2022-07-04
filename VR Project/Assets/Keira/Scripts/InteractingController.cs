using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [Tooltip("Distance the turret will hover over the ground/node while it is held")]
    public float turretHoverDistance = 10.0f;

    [Header("Tag and Layer names")]
    [Tooltip("Tag name of Nodes")]
    string nodeTag = "Node";
    [Tooltip("Tag name of Turrets")]
    string turretTag = "Turret";
    [Tooltip("Tag name of UI")]
    string uiTag = "UI";
    [Tooltip("Layer name of Interactables")]
    string interactableLayer = "Interactable";

    [Header("Scene objects to set")]

    [Tooltip("Add the money manager")]
    public MoneyManager moneyManager;
    [Tooltip("The sphere childed to this hand")]
    public GameObject ballChild;
    [Tooltip("Temp node I am using to see position of certain things")]
    public GameObject node;

    //ball thing - dumb ball renderer wont let me turn it off :(
    MeshRenderer ballRenderer;



    //Raycast data - Data needed to properly process grabbings items
    bool isObjectHeld = false;
    bool interactButtonHeldLastFrame = false;
    GameObject objectBeingHeld = null;
    //Closest node to item being held
    GameObject closestNode = null;
    //if the closest node is close enough that when the turret is let go of, it can go to it.
    bool isClosestNodeValid = false;

    GameObject[] nodeList;
    bool nodeListUpdated = false;


    //Last object hit with raycast, alongside "objectHitLastCheck" for AttemptInteract() to use if the button is pressed
    GameObject lastObjectHit = null; //Object that was last hit by raycast
    bool objectHitLastCheck = false; //if an object was hit last time the raycast happened
    float timeSinceLastRaycastHit = 10000; //Alternative that might be used to object hit last raycast check,
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
            GameObject objectHit = hit.transform.gameObject;
            if (objectHit.layer == LayerMask.NameToLayer(interactableLayer))
            { 
                lastObjectHit = objectHit;

                Interactable interact = lastObjectHit.GetComponentInParent<Interactable>();

                if (interact != null)
                {

                    interact.HoveredOver();
                    objectHitLastCheck = true;
                    timeSinceLastRaycastHit = 0;
                }
                else
                {
                    Debug.LogWarning("Interactable object does not have a findable 'interactable' script attached");
                }
                

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
            ballRenderer.enabled = true;
            objectHitLastCheck = false;
            laser.SetLaserDistFromHand(maxRaycastLength);

        }
    }

    void RaycastWhenItemHeld()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, maxRaycastLength, ~LayerMask.GetMask(interactableLayer, "Ignore Raycast")))
        {

  
            /* 
             * Get point on ground of where turret currently is
             * Check for closest nodes, if closest in range is used
             * check for second closest, if in range and used,
             * etc etc until you find one in range unused or none remain in range
             * if one is in range, put turret above.
             */

            float distance;
            if (!nodeListUpdated)
            {
                nodeListUpdated = true;
                
                nodeList = GameObject.FindGameObjectsWithTag(nodeTag);
            }
            GameObject closest = GetClosestOpenNode(nodeList, hit.point, out distance);

            if (closest != null && nodeGrabDistance > distance)
            {
                //Sets turret to hover above node
                SetHeldItemPosition(closest.transform.position + Vector3.up * turretHoverDistance);
                closestNode = closest;
                isClosestNodeValid = true;
            }
            else
            {
                //Sets turret to hover over point it is at
                SetHeldItemPosition(hit.point + Vector3.up * turretHoverDistance);
                isClosestNodeValid = false;
            }
            SetInteractBallDist(hit.distance);
            laser.SetLaserDistFromHand(hit.distance);

        } else 
        {
            SetHeldItemPosition(this.transform.position + (this.transform.forward * maxRaycastLength));
            SetInteractBallDist(maxRaycastLength);
            laser.SetLaserDistFromHand(25.0f);
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
            if (nodes[i].GetComponent<Node>().IsNodeAvailable())
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
    public void AttemptInteract(InputAction.CallbackContext interaction)
    {
        if (interaction.phase != InputActionPhase.Started)
        {
            return;
        }
        Debug.Log("AttemptInteract Called");

        interactButtonHeldLastFrame = true;
        if (maxTimeToGrab > timeSinceLastRaycastHit && !isObjectHeld)
        {
            //'grab' item
            if (lastObjectHit.CompareTag(turretTag) || lastObjectHit.CompareTag(uiTag))
            {
                Debug.Log("Get 'Turret' from LastObjectHit (1)");
                Interactable turret = lastObjectHit.GetComponentInParent<Interactable>();


                if (turret.CanBeGrabbed(moneyManager))
                {
                    Debug.Log("Object could be grabbed!");
                    isObjectHeld = true;
                    objectBeingHeld = GetBaseParentOfTurret(lastObjectHit);


                }
                else
                {
                    Debug.Log("Object could not be grabbed :(");
                }

            }
        }
        else
        {

        }
        //SetInteractBallDist(10);
    }

    //When you let go of "grab"
    public void StopInteract(InputAction.CallbackContext interaction)
    {
        if (interaction.phase != InputActionPhase.Canceled)
        {
            return;
        }
        Debug.Log("StopInteract called");


        interactButtonHeldLastFrame = false;
        if (isObjectHeld)
        {
            nodeListUpdated = false;
            isObjectHeld = false;
            Interactable turret = objectBeingHeld.GetComponentInParent<Interactable>();
            //tell closest node to take turret, tell turret it has been placed, if no closest node, tell turret it was dropped.
            //Turret or node need to set turrets position to the node.
            if (turret == null)
            {
                Debug.LogWarning("Interact Script Not Found!!!");

                return;
            }

            if (isClosestNodeValid)
            {
                turret.SetToNode(closestNode);
                isClosestNodeValid = false;
            }
            else
            {
                turret.Voided(moneyManager);
            }

        }
    }

    GameObject GetBaseParentOfTurret(GameObject turret)
    {
        if (!turret.CompareTag(turretTag))
        {
            Debug.LogWarning("Wrong Object passed into function 'GetBaseParentOfTurret(GameObject Turret)' pls fix");
            return turret;
        }

        GameObject highestWithTag = turret;
        GameObject higherParent = null;

        if (turret.transform.parent == null)
        {
            Debug.Log("Early exit from 'GetBaseParentOfTurret'");
            return highestWithTag;
        } else
        {
            higherParent = turret.transform.parent.gameObject;
        }
        
        while (higherParent.CompareTag(turretTag) && higherParent.transform.parent != null)
        {
            highestWithTag = higherParent;
            higherParent = higherParent.transform.parent.gameObject;
        }
        //not sure of a way I can incoperate the last part of this into the while loop
        //It leaves the loop early due to the null check, but the check is required
        if (higherParent.CompareTag(turretTag))
        {
            highestWithTag = higherParent;
        }

        return highestWithTag;
    }

    void SetInteractBallDist(float distance)
    {
        ballChild.transform.position = this.transform.position + this.transform.forward * distance;
    }

    //Currently does nothing until I fix this up :(
    void SetInteractBallColour(Color colour)
    {
        //Need to figure out the proper way to change colour of (and also maybe size) of these via code
        //Dont really wanna make a whole shader exclusive to a ball on the end of a pointer ya know.
    }
}



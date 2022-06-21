using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    enum InteractableType { Turret, UI };

    [SerializeField]
    InteractableType interactableType;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* 
     * for "Interacting controller" to ask if it can be grabbed
     * If it returns true, IC will child this to itself, to move it around the map, thinking this is a turret
     * 
     * 
     */
    public bool CanBeGrabbed()
    {
        if (interactableType == InteractableType.Turret)
        {
            //check if player has enough coins to buy turret, if not return false, if they do, return true, turret is grabbed, and will be told it is "let go" of at somepoint soon
            //When let go
        } else if (interactableType == InteractableType.UI)
        {
            //do ui thing, whatever idk
            return false;
        }

        return false;
    }
}

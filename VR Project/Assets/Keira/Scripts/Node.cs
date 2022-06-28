using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("Colours for the states of the node")]
    [Tooltip("Node is Available")]
    public Color available = Color.yellow;
    [Tooltip("Node will take turret if turret is released (Aka selected node)")]
    public Color selected = Color.green;
    [Tooltip("Node is unavailable")]
    public Color locked = Color.red;

    public bool nodeAvailable = true;
    public bool nodeTaken = false;




    // Start is called before the first frame update
    void Start()
    {
        SetColour(selected);

    }

    public void SetColour (Color colour)
    {
        Renderer renderer = GetComponent<Renderer>();
        
        renderer.material.SetColor("Colour", colour);
    }



    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    bool nodeAvailable = true;


    // Start is called before the first frame update
    void Start()
    {

    }

    public bool IsNodeAvailable()
    {
        return nodeAvailable;
    }

    public void SetAvailable()
    {
        nodeAvailable = true;
    }

    public void SetUnavailable()
    {
        nodeAvailable = false;
    }




}

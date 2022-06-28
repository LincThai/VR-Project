using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    // When hovering the display should show
    // Testing if placing worked

    enum InteractableType { Turret, UI };

    [SerializeField]
    InteractableType interactableType;

    [SerializeField]
    UnityEvent uiFunction = null;

    Tower tower;

    Node node;


    // Start is called before the first frame update
    void Start()
    {
        tower = GetComponent<Tower>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* 
     * for "Interacting controller" to ask if it can be grabbed
     * If it returns true, It will child this to itself, to move it around the map, thinking this is a turret
     */
    public bool CanBeGrabbed(MoneyManager mManager)
    {
        if (interactableType == InteractableType.Turret)
        {
            if (!tower.isUsed())
            {
                if (mManager.SpendGold(tower.cost))
                {
                    return true;
                }
            }
            else
            {
                mManager.AddGold((int)(tower.cost * tower.refundPercent));
                Destroy(gameObject);
                if (node != null)
                {
                    node.nodeAvailable = true;
                }
            }
            //check if player has enough coins to buy turret, if not return false, if they do, return true, turret is grabbed, and will be told it is "let go" of at somepoint soon
            //When let go
        } else if (interactableType == InteractableType.UI)
        {
            //do ui thing, whatever idk
            return false;
        }

        return false;
    }

    public void HoveredOver()
    {
        tower.ResetRangeDisplayTimer();
    }

    //If the turret got dropped without a node to connect to
    public void Voided(MoneyManager mManager)
    {
        mManager.AddGold(tower.cost);
        Destroy(gameObject);

        RefreshTurret();
    }

    //If the turret is dropped onto a node.
    public void SetToNode(GameObject setNode)
    {
        tower.ActivateTower();
        tower.setAsUsed();

        node = setNode.GetComponent<Node>();
        node.nodeAvailable = false;

        RefreshTurret();
    }

    void RefreshTurret()
    {
        GameObject newTower = Instantiate(tower.towerPrefab, tower.GetStartingPosition(), transform.rotation);
        newTower.GetComponent<Tower>().spawnManager = tower.spawnManager; 
    }
}

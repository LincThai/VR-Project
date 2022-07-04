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

    [SerializeField]
    float platformHeight = 4.2f;

    Tower tower;

    Node node;

    bool beingHeld;

    // Start is called before the first frame update
    void Start()
    {
        tower = GetComponent<Tower>();
    }

    // Update is called once per frame
    void Update()
    {
        if (beingHeld)
        {
            tower.ResetRangeDisplayTimer();
        }
    }

    /* 
     * for "Interacting controller" to ask if it can be grabbed
     * If it returns true, It will child this to itself, to move it around the map, thinking this is a turret
     */
    public bool CanBeGrabbed(MoneyManager mManager)
    {
        if (interactableType == InteractableType.Turret)
        {
            //Debug.Log("Tower Cost: " + tower.cost + " gold available: " + mManager.GetGold());
            if (!tower.isUsed())
            {
                if (mManager.SpendGold(tower.cost))
                {
                    beingHeld = true;
                    return true;
                }
            }
            else
            {
                mManager.AddGold((int)(tower.cost * tower.refundPercent));
                Debug.Log("Tower Refunded");
                if (node != null)
                {
                    node.SetAvailable();
                }
                Debug.Log("self destruct");
                Destroy(gameObject);
            }
            //check if player has enough coins to buy turret, if not return false, if they do, return true, turret is grabbed, and will be told it is "let go" of at somepoint soon
            //When let go
        } else if (interactableType == InteractableType.UI)
        {

            //do ui thing, whatever idk
            if (uiFunction != null)
            {
                uiFunction.Invoke();
                
 
            }
            return false;
        }

        return false;
    }

    public void HoveredOver()
    {
        if (tower != null)
        {
            tower.ResetRangeDisplayTimer();
        }
    }

    //If the turret got dropped without a node to connect to
    public void Voided(MoneyManager mManager)
    {
        mManager.AddGold(tower.cost);
        RefreshTurret();
        Destroy(gameObject);

    }

    //If the turret is dropped onto a node.
    public void SetToNode(GameObject setNode)
    {

        node = setNode.GetComponent<Node>();

        tower.ActivateTower();
        tower.setAsUsed();
        tower.transform.position = node.transform.position + Vector3.up * platformHeight;

        node.SetUnavailable();

        beingHeld = false;
        RefreshTurret();
    }

    void RefreshTurret()
    {
        GameObject newTower = Instantiate(tower.towerPrefab, tower.GetStartingPosition(), transform.rotation);
        newTower.GetComponent<Tower>().spawnManager = tower.spawnManager; 
    }
}

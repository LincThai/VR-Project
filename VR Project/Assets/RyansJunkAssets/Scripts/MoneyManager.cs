using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    [SerializeField]
    int gold = 20;
    [SerializeField]
    TextMeshProUGUI goldDisplay;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTextDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetGold()
    {
        return gold;
    }

    // Returns true and subtracts cost from gold if you have enough gold or just returns false if you don't have enough
    public bool SpendGold(int cost)
    {
        if (gold >= cost)
        {
            gold -= cost;
            UpdateTextDisplay();
            return true;
        }
        else
        {
            return false;
        }

    }

    public void AddGold(int gold)
    {
        gold += gold;
        UpdateTextDisplay();
    }

    public void UpdateTextDisplay()
    {
        goldDisplay.text = "Gold: " + gold;
    }
}

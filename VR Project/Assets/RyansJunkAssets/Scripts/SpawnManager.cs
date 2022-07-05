using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    [Tooltip("Put in the start button")]
    public GameObject startButton;
    [Tooltip("Put in the money manager")]
    public MoneyManager moneyManager;
    [Tooltip("Put in you spawners")]
    public Spawner[] spawners;
    [Header("Spawner Variables")]
    [Tooltip("Changing this will change the starting amount of enemies.")]
    public int enemySpawnTotal = 10;
    [Tooltip("Chance that a unit will be special. Change to affect starting chance.")]
    public int specialUnitChance = 0;
    [Tooltip("Max chance that a spawned unit will be special.")]
    public int maxSpecialUnitChance;
    [Tooltip("How much the chances of a special unit spawning will be increased by each round.")]
    public int specialUnitChanceIncrease = 1;
    [Tooltip("How much to scale up the total enemies spawned by each round. E.g 0.1 means it will increase by 10%")]
    public float enemySpawnTotalScale = 0.1f;

    public List<GameObject> enemyList = new List<GameObject>();
    public int enemyCount = 0;
    public int spawnCount = 0;
    int roundNum = 0;
    [Tooltip("Textbox for round num")]
    public TextMeshProUGUI roundDisplay;

    [Header("Audio GameObjects")]
    public AudioSource breakAudio;
    public AudioSource backgroundAudio;
    public AudioSource towerSellAudio;
    public AudioSource towerPickupAudio;

    bool newRound = false;
    bool gameStart = true;

    bool tested = false;
    public float t = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //spawnCount = enemySpawnTotal;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart == true)
        {
            startButton.SetActive(true);
            gameStart = false;
        }
        else if (newRound && spawnCount == enemySpawnTotal && enemyCount == 0)
        {
            newRound = false;
            startButton.SetActive(true);
        }

        if (newRound == false)
        {
            breakAudio.mute = false;
            backgroundAudio.mute = true;
        }
        else
        {
            breakAudio.mute = true;
            backgroundAudio.mute = false;
        }



        if (t > 10.0f && tested == false)
        {
            tested = true;
            newRound = false;
            NewRound();
        }
        t += Time.deltaTime;
    }

    public void IncrementCounts()
    {
        enemyCount++;
        spawnCount++;
    }

    public int GetEnemyCount()
    {
        return enemyCount;
    }

    public void DecrementEnemyCount()
    {
        enemyCount--;
    }

    public int GetSpawnCount()
    {
        return spawnCount;
    }

    public void AddToEnemies(GameObject newEnemy)
    {
        enemyList.Add(newEnemy);
    }

    public void RemoveFromEnemies(GameObject enemy)
    {
        enemyList.Remove(enemy);
    }

    public void NewRound()
    {
        spawnCount = 0;
        newRound = true;
        roundNum++;
        UpdateRoundNum();

        enemySpawnTotal += (int)(enemySpawnTotal * enemySpawnTotalScale);

        if (specialUnitChance + specialUnitChanceIncrease > maxSpecialUnitChance)
        {

        }
        else if (specialUnitChance != maxSpecialUnitChance && specialUnitChance + specialUnitChanceIncrease > maxSpecialUnitChance)
        {
            specialUnitChance = maxSpecialUnitChance;
        }
        else
        {
            specialUnitChance += specialUnitChanceIncrease;
        }
        startButton.SetActive(false);
    }

    void UpdateRoundNum()
    {
        if (roundDisplay != null)
        {
            roundDisplay.text = "Round: " + roundNum;
        }
    }
}

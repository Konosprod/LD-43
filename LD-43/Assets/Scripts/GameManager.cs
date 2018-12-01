using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [HideInInspector]
    public static GameManager _instance;
    [HideInInspector]
    public int goalLayer;
    [HideInInspector]
    public int mobLayer;

    [Header("GameLogic")]
    public GameObject start;
    public GameObject goal;
    public GameObject mobPrefab;

    [Header("UI")]
    public Text waveText;
    public Text villagerText;
    public Text moneyText;


    // Internal game logic
    private int villagerCount = 50;
    private int wave = 1;
    private int money = 500;
    private const float pauseTime = 30f;
    private float currentPauseTime = 10f;
    private bool isPlaying = false; // false = pause time between waves, true = wave

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this);
    }

    // Use this for initialization
    void Start()
    {
        goalLayer = LayerMask.NameToLayer("Goal");
        mobLayer = LayerMask.NameToLayer("Mob");

        UpdateVillagerText();
        UpdateWaveText();
        UpdateMoneyText();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPlaying)
        {
            currentPauseTime -= Time.deltaTime;
            if(currentPauseTime <= 0f)
            {
                // Start wave
                currentPauseTime = pauseTime;
                isPlaying = true;
            }
        }
        else
        {

        }

        /*if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject newMob = Instantiate(mobPrefab, start.transform.position, Quaternion.identity);
            NavMeshAgent agent = newMob.GetComponent<NavMeshAgent>();
            agent.SetDestination(goal.transform.position);
        }*/
    }


    // When a mob reaches the goal, you take damage
    public void TakeDamage(int damage)
    {
        villagerCount -= damage;
        if (villagerCount <= 0)
        {
            villagerCount = 0;
            LoseTheGame();
        }

        UpdateVillagerText();
    }

    public bool CanAfford(int amount)
    {
        return money >= amount;
    }

    private void SpendMoney(int amount)
    {
        if(CanAfford(amount))
        {
            money -= amount;
            UpdateMoneyText();
        }
    }

    public void BuyTower(GameObject towerPref, Tower tower)
    {
        Vector3 position = TowerManager._instance.selectedSpot.transform.position;
        position += new Vector3(0f, 0.025f, 0f);
        GameObject newTower = Instantiate(towerPref, position, Quaternion.identity);
        newTower.GetComponent<Tower>().isPreviewMode = false;
        TowerManager._instance.towers.Add(newTower);
        SpendMoney(tower.price);

        TowerManager._instance.towerShopPanel.SetActive(false);
    }

    private void UpdateVillagerText()
    {
        villagerText.text = "Villagers : " + villagerCount;
    }

    private void UpdateWaveText()
    {
        waveText.text = "Wave : " + wave;
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "Money : " + money;
    }

    private void LoseTheGame()
    {
        Debug.Log("RIP");
    }
}

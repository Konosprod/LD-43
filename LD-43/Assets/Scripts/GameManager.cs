﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
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
    public GameObject goal;
    public List<GameObject> spawners;
    public MobWave mobWave;
    public VillageManager villageManager;

    [Header("UI")]
    public Text waveText;
    public Text villagerText;
    public Text moneyText;
    public Text timeText;
    public Button buttonVillage;
    public GameObject panelVillageInfo;


    // Internal game logic
    public int villagerCount = 450;
    private int wave = 1;
    private int money = 5000;
    private const float pauseTime = 30f;
    private float currentPauseTime = 10f;
    private bool isPlaying = false; // false = pause time between waves, true = wave
    private Dictionary<GameObject, List<Mob>> currentWave;
    private List<GameObject> currentWaveMobs = new List<GameObject>();
    private const float spawnDelay = 0.5f;
    private float currentSpawnDelay;

    private bool isPerfectWave = true;

    private bool isInVillage = false;


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
            Destroy(this);
    }

    // Use this for initialization
    void Start()
    {
        // Add a first random spawner
        int randSpawn = /*Random.Range(0, spawners.Count)*/0;
        mobWave.activeSpawners.Add(spawners[randSpawn]);

        // Hide inactive spawners
        foreach(GameObject spawn in spawners)
        {
            if (spawn != spawners[randSpawn])
                spawn.SetActive(false);
        }

        currentSpawnDelay = spawnDelay;

        goalLayer = LayerMask.NameToLayer("Goal");
        mobLayer = LayerMask.NameToLayer("Mob");

        UpdateVillagerText();
        UpdateWaveText();
        UpdateMoneyText();
        UpdateTimeText();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                currentPauseTime -= pauseTime; // Skip pause
            }

            currentPauseTime -= Time.deltaTime;
            if (currentPauseTime <= 0f)
            {
                // Start wave
                currentPauseTime = pauseTime;
                isPlaying = true;
                currentWave = mobWave.GenerateWave(wave);
            }
            buttonVillage.enabled = true;
        }
        else
        {
            if (IsWaveDone())
            {
                if (isPerfectWave)
                    villagerCount += villageManager.villagerPerPerfectWave;
                else
                    villagerCount += villageManager.villagerPerWave;

                wave++;
                isPlaying = false;
                isPerfectWave = true;
                UpdateWaveText();
                if (wave % 5 == 0 && wave <= 20)
                {
                    // Add a spawner to the available spawners
                    foreach(GameObject spawner in spawners)
                    {
                        if(!mobWave.activeSpawners.Contains(spawner))
                        {
                            mobWave.activeSpawners.Add(spawner);
                            spawner.SetActive(true);
                            break;
                        }
                    }
                }

            }
            else
            {
                if(!IsSpawnDone())
                {
                    currentSpawnDelay -= Time.deltaTime;
                    if (currentSpawnDelay < 0f)
                    {
                        GameObject removeSpawner = null;
                        foreach (GameObject spawner in currentWave.Keys)
                        {
                            if (currentWave[spawner].Count > 0)
                            {
                                Mob m = currentWave[spawner][0];
                                GameObject newMob = Instantiate(m.mobPrefab, spawner.transform.position, spawner.transform.rotation);
                                currentWave[spawner].RemoveAt(0);
                                currentWaveMobs.Add(newMob);

                                NavMeshAgent agent = newMob.GetComponent<NavMeshAgent>();
                                agent.SetDestination(goal.transform.position);
                            }
                            else
                            {
                                removeSpawner = spawner;
                            }
                        }

                        if(removeSpawner != null)
                        {
                            currentWave.Remove(removeSpawner);
                        }

                        currentSpawnDelay = spawnDelay;
                    }
                }
            }
        }

        UpdateTimeText();
    }


    private bool IsSpawnDone()
    {
        bool res = true;

        foreach (GameObject spawner in currentWave.Keys)
        {
            if (currentWave[spawner].Count > 0)
            {
                res = false;
                break;
            }
        }

        return res;
    }

    private bool IsWaveDone()
    {
        bool res = true;

        if (currentWaveMobs.Count == 0)
        {
            foreach (GameObject spawner in currentWave.Keys)
            {
                if (currentWave[spawner].Count > 0)
                {
                    res = false;
                    break;
                }
            }
        }
        else
            res = false;

        return res;
    }

    public void IAmAMobAndIDied(GameObject mob)
    {
        currentWaveMobs.Remove(mob);
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

        isPerfectWave = false;

        UpdateVillagerText();
    }

    public bool CanAfford(int amount)
    {
        return money >= amount;
    }

    private void SpendMoney(int amount)
    {
        if (CanAfford(amount))
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
        TowerManager._instance.AddUsedTowerSpot(newTower);
        SpendMoney(tower.price);

        TowerManager._instance.towerShopPanel.SetActive(false);
    }

    public void GoToVillage()
    {
        villageManager.GenerateVillagers();
        isInVillage = true;
        Vector3 pos = Camera.main.transform.position;

        pos.z = 32.7f;
        pos.y = 4.8f;
        pos.x = -4.1f;

        Camera.main.transform.position = pos;
        Camera.main.transform.localEulerAngles = new Vector3(30.5f, 90, 0);

        buttonVillage.GetComponentInChildren<Text>().text = "Back";
        buttonVillage.onClick.RemoveAllListeners();
        buttonVillage.onClick.AddListener(GoBack);

        panelVillageInfo.SetActive(true);
    }

    private void GoBack()
    {
        Vector3 pos = Camera.main.transform.position;

        pos.z = -16f;
        pos.y = 13.31f;
        pos.x = -8.8f;

        Camera.main.transform.position = pos;
        Camera.main.transform.localEulerAngles = new Vector3(50f, 90f, 0f);

        villageManager.RemoveVillagers();
        buttonVillage.GetComponentInChildren<Text>().text = "Village";
        buttonVillage.onClick.RemoveAllListeners();
        buttonVillage.onClick.AddListener(GoToVillage);
        isInVillage = false;

        panelVillageInfo.SetActive(false);
    }

    public void UpgradeSelectedTower()
    {
        SpendMoney(TowerManager._instance.selectedTowerTower.GetUpgradeCost());
        TowerManager._instance.selectedTowerTower.LevelUp();

        TowerManager._instance.upgradeTowerPanel.GetComponent<TowerUpgradePanel>().UpdateValues();
    }

    public void SellSelectedTower()
    {
        money += TowerManager._instance.selectedTowerTower.GetSellValue();
        UpdateMoneyText();
        TowerManager._instance.DeleteSelectedTower();

        TowerManager._instance.DisableUpgradeTowerPanel();
    }

    private void UpdateTimeText()
    {
        if (isPlaying)
            timeText.text = "Time : Playing";
        else
            timeText.text = "Time : " + currentPauseTime.ToString("0") + " s";
    }

    public void UpdateVillagerText()
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

    public void LoseTheGame()
    {
        SceneManager.LoadScene("GameOver");
    }
}

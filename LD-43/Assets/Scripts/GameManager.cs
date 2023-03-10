using System.Collections;
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
    public Image timerProgressBar;
    public Text foodText;
    public Text scoreText;
    public Button buttonVillage;
    public GameObject panelVillageInfo;
    public GameObject minimap;
    public GameObject skipText;


    // Internal game logic
    public int villagerCount = 450;
    private int wave = 1;
    private int money = 400;
    public int food = 10;
    private const float pauseTime = 30f;
    private float currentPauseTime = 30f;
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
        foreach (GameObject spawn in spawners)
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
        UpdateFoodText();
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            if (isInVillage)
                GoBack();
            else
                GoToVillage();
        }


        if (!isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentPauseTime -= pauseTime; // Skip pause
            }

            currentPauseTime -= Time.deltaTime;
            if (currentPauseTime <= 0f)
            {
                // Start wave
                currentPauseTime = pauseTime;
                isPlaying = true;
                skipText.SetActive(false);
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

                villagerCount += villageManager.villagerPerWave;

                villageManager.UpdateVillagers();
                UpdateVillagerText();

                wave++;
                isPlaying = false;
                skipText.SetActive(true);
                isPerfectWave = true;
                UpdateWaveText();
                if (wave % 5 == 0 && wave <= 20)
                {
                    // Add a spawner to the available spawners
                    foreach (GameObject spawner in spawners)
                    {
                        if (!mobWave.activeSpawners.Contains(spawner))
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
                if (!IsSpawnDone())
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

                        if (removeSpawner != null)
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

    public void IAmAMobAndIDied(GameObject mob, bool killed)
    {
        if (killed)
        {
            Mob m = mob.GetComponent<Mob>();
            food += m.foodLoot;
            SettingsManager._instance.gameSettings.Score += (int)Mathf.Floor(m.spawnCost * GetStatScaleForMobs());
            UpdateScoreText();
        }

        UpdateFoodText();
        villageManager.UpdateUpgradeButtons();
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
        
        villageManager.UpdateVillagers();

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

    public void EarnMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();
    }

    public void BuyTower(GameObject towerPref, Tower tower)
    {
        Vector3 position = TowerManager._instance.selectedSpot.transform.position;
        position += new Vector3(0f, 0.025f, 0f);
        GameObject newTower = Instantiate(towerPref, position, Quaternion.identity);
        newTower.GetComponent<Tower>().isPreviewMode = false;
        TowerManager._instance.AddUsedTowerSpot(newTower);
        SpendMoney(tower.price);

        TowerManager._instance.CheckTowerBuffs();

        TowerManager._instance.towerShopPanel.SetActive(false);
    }

    public void GoToVillage()
    {
        villageManager.GenerateVillagers();
        isInVillage = true;
        Vector3 pos = Camera.main.transform.position;

        pos.z = 32.7f;
        pos.y = 6.14f;
        pos.x = -3.31f;

        Camera.main.transform.position = pos;
        Camera.main.transform.localEulerAngles = new Vector3(30.5f, 90, 0);
        Camera.main.orthographicSize = 6f;

        buttonVillage.GetComponentInChildren<Text>().text = "Back (V)";
        buttonVillage.onClick.RemoveAllListeners();
        buttonVillage.onClick.AddListener(GoBack);

        TowerManager._instance.DisableUpgradeTowerPanel();
        TowerManager._instance.towerShopPanel.SetActive(false);

        panelVillageInfo.SetActive(true);
        minimap.SetActive(true);
    }

    private void GoBack()
    {
        Vector3 pos = Camera.main.transform.position;

        pos.z = -12.97f;
        pos.y = 12.23f;
        pos.x = -8.82f;

        Camera.main.transform.position = pos;
        Camera.main.transform.localEulerAngles = new Vector3(40f, 90f, 0f);
        Camera.main.orthographicSize = 8.9f;

        villageManager.RemoveVillagers();
        buttonVillage.GetComponentInChildren<Text>().text = "Village (V)";
        buttonVillage.onClick.RemoveAllListeners();
        buttonVillage.onClick.AddListener(GoToVillage);
        isInVillage = false;

        panelVillageInfo.SetActive(false);
        minimap.SetActive(false);
    }

    public void UpgradeSelectedTower()
    {
        SpendMoney(TowerManager._instance.selectedTowerTower.GetUpgradeCost());
        TowerManager._instance.selectedTowerTower.LevelUp();

        TowerManager._instance.CheckTowerBuffs();

        TowerManager._instance.upgradeTowerPanel.GetComponent<TowerUpgradePanel>().UpdateValues();
    }

    public void SellSelectedTower()
    {
        EarnMoney(TowerManager._instance.selectedTowerTower.GetSellValue());

        TowerManager._instance.DeleteSelectedTower();

        TowerManager._instance.CheckTowerBuffs();

        TowerManager._instance.DisableUpgradeTowerPanel();
    }

    public float GetStatScaleForMobs()
    {
        return 1f + (wave * wave * Mathf.Sqrt(wave) / 1000f);
    }

    private void UpdateTimeText()
    {
        if (isPlaying)
        {
            timerProgressBar.fillAmount = 0f;
            timeText.text = "Time : Playing";
        }
        else
        {
            timerProgressBar.fillAmount = currentPauseTime / pauseTime;
            timeText.text = "Time : " + currentPauseTime.ToString("0") + " s";
        }
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


    public void UpdateFoodText()
    {
        foodText.text = "Food : " + food;
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score : " + SettingsManager._instance.gameSettings.Score;
    }

    public void LoseTheGame()
    {
        SceneManager.LoadScene("GameOver");
    }
}

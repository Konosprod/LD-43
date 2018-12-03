using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VillageManager : MonoBehaviour
{

    [Header("Game Logic")]
    public int maxVillagers = 30;
    public float radiusSpawn = 3f;


    //Value managed/used by the village
    public int baseVillagerPerWave = 15;
    public int baseVillagerPerPerfectWave = 25;
    public int baseVillagerPerTime = 10;
    [HideInInspector]
    public int villagerPerWave;
    [HideInInspector]
    public int villagerPerPerfectWave;
    [HideInInspector]
    public int villagerPerTime;
    public float spawnRate = 10f;
    public int pricePw = 30;
    public int pricePpw = 50;
    public int priceRate = 20;


    private int pwLevel = 1;
    private int ppwLevel = 1;
    private int rateLevel = 1;

    public List<GameObject> listPrefab;
    public List<Villager> villagerList = new List<Villager>();
    private GameManager gm;

    [Header("UI")]
    public GameObject panelInfoVillager;
    public Text valueText;
    public Text villagerPerTimeText;
    public Text villagerPerWaveText;
    public Text villagerPerPerfectWaveText;

    [Header("UI Upgrade")]
    public GameObject upgradePanel;
    public Button upgradeRateButton;
    public Button upgradePerWaveButton;
    public Button upgradePerPerfectWaveButton;

    public GameObject spawner;
    private GameObject selectedVillager;

    private float villagerTime;

    // Use this for initialization
    void Start()
    {

        villagerTime = spawnRate;

        gm = GameManager._instance;

        for (int i = 0; i < maxVillagers; i++)
        {
            Vector3 originPoint = spawner.gameObject.transform.position;
            originPoint.x += Random.Range(-radiusSpawn, radiusSpawn);
            originPoint.z += Random.Range(-radiusSpawn, radiusSpawn);

            int draw = Random.Range(0, listPrefab.Count);
            GameObject villager = Instantiate(listPrefab[draw], originPoint, Quaternion.identity);

            villager.SetActive(false);
            villagerList.Add(villager.GetComponent<Villager>());
        }

        villagerPerWave = baseVillagerPerWave;
        villagerPerTime = baseVillagerPerTime;
        villagerPerPerfectWave = baseVillagerPerPerfectWave;

        upgradeRateButton.onClick.AddListener(UpgradeRate);
        upgradePerWaveButton.onClick.AddListener(UpgradePw);
        upgradePerPerfectWaveButton.onClick.AddListener(UpgradePpw);

        UpdateUpgradeButtons();
    }

    // Update is called once per frame
    void Update()
    {
        villagerTime -= Time.deltaTime;

        //Spawn villager per time
        if (villagerTime <= 0)
        {
            gm.villagerCount += villagerPerTime;
            gm.UpdateVillagerText();
            villagerTime = spawnRate;

            UpdateVillagers();
        }


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<Villager>() != null)
                {
                    selectedVillager = hit.transform.gameObject;
                    SacrificeVillager();
                    /*valueText.text = "Value : " + hit.transform.GetComponent<Villager>().valueVillagers;
                    panelInfoVillager.SetActive(true);*/
                }
                else
                {
                    //panelInfoVillager.SetActive(false);
                }
            }
        }
    }

    /*public void GenerateVillagers()
    {
        int indexVillager = 0;
        int amount = gm.villagerCount;


        while (amount > 0)
        {
            int numberPower = (int)Mathf.Floor(Mathf.Log10(amount));
            int nbVillager = amount / (int)Mathf.Pow(10, numberPower);

            for (int i = 0; i < nbVillager; i++)
            {
                villagerList[indexVillager].gameObject.SetActive(true);
                villagerList[indexVillager].valueVillagers = (int)Mathf.Pow(10, numberPower);
                indexVillager++;
            }

            amount -= nbVillager * (int)Mathf.Pow(10, numberPower);
        }

        UpdateVillageInfo();
    }*/

    public void GenerateVillagers()
    {
        int points = gm.villagerCount;

        if (points <= maxVillagers)
        {
            int valuePerVillager = 1;
            for (int i = 0; i < points; i++)
            {
                villagerList[i].gameObject.SetActive(true);
                villagerList[i].valueVillagers = valuePerVillager;
                villagerList[i].UpdateValueText();
            }
        }
        else
        {
            List<int> valuesPerVillager = new List<int>();
            int randValueSum = 0;
            for (int i = 0; i < maxVillagers; i++)
            {
                int randValue = Random.Range(1 + (points / 10), points);
                valuesPerVillager.Add(randValue);
                randValueSum += randValue;
            }

            int pointsCopy = points;
            for (int i = 0; i < maxVillagers - 1; i++)
            {
                int trueValue = valuesPerVillager[i] * points / randValueSum;
                if (trueValue > 0)
                {
                    villagerList[i].gameObject.SetActive(true);
                    villagerList[i].valueVillagers = trueValue;
                    villagerList[i].UpdateValueText();
                    pointsCopy -= trueValue;
                }
            }

            villagerList[maxVillagers - 1].gameObject.SetActive(true);
            villagerList[maxVillagers - 1].valueVillagers = pointsCopy;
            villagerList[maxVillagers - 1].UpdateValueText();
        }

        UpdateVillageInfo();
        UpdateUpgradeButtons();
    }

    public void UpdateVillagers()
    {
        int points = gm.villagerCount;
        int pointsInUse = 0;
        int nbActiveVillagers = 0;

        Villager inactiveVillager = null;

        for (int i = 0; i < maxVillagers; i++)
        {
            if (villagerList[i].gameObject.activeSelf)
            {
                pointsInUse += villagerList[i].valueVillagers;
                nbActiveVillagers++;
            }
            else
            {
                inactiveVillager = villagerList[i];
            }
        }

        if (pointsInUse > points)
        {
            // We must reduce those points ! We've taken a hit
            int excess = pointsInUse - points;
            while (excess > 0)
            {
                foreach (Villager villager in villagerList)
                {
                    if (villager.valueVillagers > excess)
                    {
                        villager.valueVillagers -= excess;
                        villager.UpdateValueText();
                        excess = 0;
                    }
                    else
                    {
                        // Make them die maybe
                        excess -= villager.valueVillagers;
                        villager.valueVillagers = 0;
                        villager.gameObject.SetActive(false);
                    }
                }
            }
        }
        else if (pointsInUse < points) // If == there is nothing to do :D
        {
            if (nbActiveVillagers < maxVillagers)
            {
                // We can activate more villagers
                if (inactiveVillager != null)
                {
                    inactiveVillager.gameObject.SetActive(true);
                    inactiveVillager.valueVillagers = points - pointsInUse;
                    inactiveVillager.UpdateValueText();
                }
                else
                {
                    Debug.LogError("O no " + villagerList.Count + " " + nbActiveVillagers + " " + maxVillagers);
                }
            }
            else
            {
                // Distribute the points among the villagers
                int nbWinners = Random.Range(1, maxVillagers);
                int extraPointsPerWinner = (points - pointsInUse) / nbWinners;
                for (int i = 0; i < nbWinners; i++)
                {
                    int winner = Random.Range(0, maxVillagers);
                    villagerList[winner].valueVillagers += extraPointsPerWinner;
                    villagerList[winner].UpdateValueText();
                }
            }
        }

    }

    public void SacrificeVillager()
    {
        Villager sacrifice = selectedVillager.GetComponent<Villager>();
        if (!sacrifice.isSacrified)
        {
            sacrifice.isSacrified = true;
            int value = sacrifice.valueVillagers;
            gm.villagerCount -= value;
            gm.UpdateVillagerText();

            gm.EarnMoney(value);

            if (gm.villagerCount <= 0)
                gm.LoseTheGame();

            // Destroy the villager !
            villagerList.Remove(sacrifice);
            sacrifice.Execute();

            // Generate a new one
            Vector3 originPoint = spawner.gameObject.transform.position;
            originPoint.x += Random.Range(-radiusSpawn, radiusSpawn);
            originPoint.z += Random.Range(-radiusSpawn, radiusSpawn);

            int draw = Random.Range(0, listPrefab.Count);
            GameObject villager = Instantiate(listPrefab[draw], originPoint, Quaternion.identity);

            villager.SetActive(false);
            villagerList.Add(villager.GetComponent<Villager>());


            UpdateVillagers();

            //panelInfoVillager.SetActive(false);
        }
    }

    public void RemoveVillagers()
    {
        upgradePanel.SetActive(false);
        //panelInfoVillager.SetActive(false);
        for (int i = 0; i < villagerList.Count; i++)
        {
            villagerList[i].gameObject.SetActive(false);
        }
    }

    private void UpdateVillageInfo()
    {
        villagerPerTimeText.text = villagerPerTime.ToString() + " villgers every " + spawnRate.ToString() + " s";
        villagerPerWaveText.text = villagerPerWave.ToString() + " villager per wave";
        villagerPerPerfectWaveText.text = villagerPerPerfectWave.ToString() + " each perfect wave";
    }

    public void ShowUpgradePanel()
    {
        UpdateUpgradeButtons();
        upgradePanel.SetActive(true);
    }

    private int GetPwUpgradeCost()
    {
        return pricePw / 2 * (int)Mathf.Ceil(Mathf.Pow(1.5f, pwLevel - 1));
    }

    private int GetPpwUpgradeCost()
    {
        return pricePpw / 2 * (int)Mathf.Ceil(Mathf.Pow(1.5f, ppwLevel - 1));
    }

    private int GetRateUpgradeCost()
    {
        return priceRate / 2 * (int)Mathf.Ceil(Mathf.Pow(1.5f, rateLevel - 1));
    }


    private void UpgradePw()
    {
        gm.food -= GetPwUpgradeCost();
        gm.UpdateFoodText();

        villagerPerWave += (int)Mathf.Floor(1.5f * baseVillagerPerWave);
        pwLevel++;

        UpdateUpgradeButtons();
        UpdateVillageInfo();
    }

    private void UpgradePpw()
    {
        gm.food -= GetPpwUpgradeCost();
        gm.UpdateFoodText();

        villagerPerPerfectWave += (int)Mathf.Floor(1.5f * baseVillagerPerPerfectWave);
        ppwLevel++;

        UpdateUpgradeButtons();
        UpdateVillageInfo();
    }

    private void UpgradeRate()
    {
        gm.food -= GetRateUpgradeCost();
        gm.UpdateFoodText();

        villagerPerTime += (int)Mathf.Floor(1.5f * baseVillagerPerTime);
        rateLevel++;

        UpdateUpgradeButtons();
        UpdateVillageInfo();
    }

    public void UpdateUpgradeButtons()
    {
        int food = gm.food;

        upgradePerPerfectWaveButton.GetComponentInChildren<Text>().text = "Per Perfect Cost : " + GetPpwUpgradeCost().ToString();
        upgradePerWaveButton.GetComponentInChildren<Text>().text = "Per Wave Cost : " + GetPwUpgradeCost().ToString();
        upgradeRateButton.GetComponentInChildren<Text>().text = "Rate Cost : " + GetRateUpgradeCost();

        if (GetPpwUpgradeCost() > food)
        {
            upgradePerPerfectWaveButton.interactable = false;
        }
        else
        {
            upgradePerPerfectWaveButton.interactable = true;
        }

        if (GetRateUpgradeCost() > food)
        {
            upgradeRateButton.interactable = false;
        }
        else
        {
            upgradeRateButton.interactable = true;
        }

        if (GetPwUpgradeCost() > food)
        {
            upgradePerWaveButton.interactable = false;
        }
        else
        {
            upgradePerWaveButton.interactable = true;
        }
    }
}

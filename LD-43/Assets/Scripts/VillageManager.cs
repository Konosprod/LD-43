using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VillageManager : MonoBehaviour {

    [Header("Game Logic")]
    public int maxVillagers = 30;
    public float radiusSpawn = 3f;


    //Value managed/used by the village
    public int villagerPerWave = 15;
    public int villagerPerPerfectWave = 25;
    public int villagerPerTime = 10;
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
	void Start () {

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

        upgradeRateButton.onClick.AddListener(UpgradeRate);
        upgradePerWaveButton.onClick.AddListener(UpgradePw);
        upgradePerPerfectWaveButton.onClick.AddListener(UpgradePpw);
    }
	
	// Update is called once per frame
	void Update ()
    {
        villagerTime -= Time.deltaTime;

        //Spawn villager per time
        if(villagerTime <= 0)
        {
            gm.villagerCount += villagerPerTime;
            gm.UpdateVillagerText();
            villagerTime = spawnRate;
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
                    valueText.text = "Value : " + hit.transform.GetComponent<Villager>().valueVillagers;
                    panelInfoVillager.SetActive(true);
                }
                else
                {
                    panelInfoVillager.SetActive(false);
                }
            }
        }
    }

    public void GenerateVillagers()
    {
        int indexVillager = 0;
        int amount = gm.villagerCount;


        while(amount > 0)
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
    }

    public void SacrificeVillager()
    {
        gm.villagerCount -= selectedVillager.GetComponent<Villager>().valueVillagers;
        gm.UpdateVillagerText();

        if (gm.villagerCount <= 0)
            gm.LoseTheGame();

        selectedVillager.SetActive(false);
        panelInfoVillager.SetActive(false);
    }

    public void RemoveVillagers()
    {
        for(int i = 0; i < villagerList.Count; i++)
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
        int food = gm.food;
        upgradePerPerfectWaveButton.GetComponentInChildren<Text>().text = "Per Perfect Cost : " + GetPpwUpgradeCost().ToString();
        upgradePerWaveButton.GetComponentInChildren<Text>().text = "Per Wave Cost : " + GetPwUpgradeCost().ToString();
        upgradeRateButton.GetComponentInChildren<Text>().text = "Rate Cost : " + GetRateUpgradeCost();

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

        UpdateUpgradeButtons();

    }

    private void UpgradePpw()
    {
        gm.food -= GetPpwUpgradeCost();
        gm.UpdateFoodText();

        UpdateUpgradeButtons();
    }

    private void UpgradeRate()
    {
        gm.food -= GetRateUpgradeCost();
        gm.UpdateFoodText();

        UpdateUpgradeButtons();

    }

    private void UpdateUpgradeButtons()
    {
        int food = gm.food;

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

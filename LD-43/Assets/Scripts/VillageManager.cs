using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VillageManager : MonoBehaviour {

    public int maxVillagers = 30;
    public float radiusSpawn = 3f;
    public List<GameObject> listPrefab;
    public List<GameObject> villagerList = new List<GameObject>();
    private GameManager gm;

    [Header("UI")]
    public GameObject panelInfoVillager;
    public Text valueText;

    public GameObject spawner;
    private GameObject selectedVillager;

	// Use this for initialization
	void Start () {

        gm = GameManager._instance;

        for (int i = 0; i < maxVillagers; i++)
        {
            Vector3 originPoint = spawner.gameObject.transform.position;
            originPoint.x += Random.Range(-radiusSpawn, radiusSpawn);
            originPoint.z += Random.Range(-radiusSpawn, radiusSpawn);

            int draw = Random.Range(0, listPrefab.Count);
            GameObject villager = Instantiate(listPrefab[draw], originPoint, Quaternion.identity);

            villager.SetActive(false);
            villagerList.Add(villager);
        }

    }
	
	// Update is called once per frame
	void Update () {

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
                villagerList[indexVillager].SetActive(true);
                villagerList[indexVillager].GetComponent<Villager>().valueVillagers = (int)Mathf.Pow(10, numberPower);
                indexVillager++;
            }

            amount -= nbVillager * (int)Mathf.Pow(10, numberPower);
        }
    }

    public void SacrificeVillager()
    {
        selectedVillager.SetActive(false);
        panelInfoVillager.SetActive(false);
    }

    public void RemoveVillagers()
    {
        for(int i = 0; i < villagerList.Count; i++)
        {
            villagerList[i].SetActive(false);
        }
    }

}

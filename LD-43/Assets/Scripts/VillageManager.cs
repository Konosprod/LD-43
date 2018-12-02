using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageManager : MonoBehaviour {


    public List<GameObject> listPrefab;
    GameManager gm;

    public GameObject spawner;

	// Use this for initialization
	void Start () {

        gm = GameManager._instance;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateVillagers()
    {
        float radius = 3f;

        int amount = gm.villagerCount;


        while(amount > 0)
        {
            int numberPower = (int)Mathf.Floor(Mathf.Log10(amount));
            int nbVillager = amount / (int)Mathf.Pow(10, numberPower);

            for (int i = 0; i < nbVillager; i++)
            {
                Vector3 originPoint = spawner.gameObject.transform.position;
                originPoint.x += Random.Range(-radius, radius);
                originPoint.z += Random.Range(-radius, radius);

                int draw = Random.Range(0, listPrefab.Count);
                GameObject villager = Instantiate(listPrefab[draw], originPoint, Quaternion.identity);

                villager.GetComponent<Villager>().valueVillagers = (int)Mathf.Pow(10, numberPower);
            }

            amount -= nbVillager * (int)Mathf.Pow(10, numberPower);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerShopPanel : MonoBehaviour {

    public List<GameObject> towerPrefabs = new List<GameObject>();
    public GameObject buyTowerButtonPrefab;

    void Awake()
    {
        foreach(GameObject tower in towerPrefabs)
        {
            GameObject button = Instantiate(buyTowerButtonPrefab, transform);
            button.GetComponent<BuyTowerButton>().SetTower(tower);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

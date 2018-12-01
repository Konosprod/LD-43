using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyTowerButton : MonoBehaviour
{
    private GameObject towerPrefab;
    private Tower tower;
    private Button button;

    // Use this for initialization
    void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    public void SetTower(GameObject towerPref)
    {
        towerPrefab = towerPref;
        tower = towerPref.GetComponent<Tower>();
        //Debug.Log(towerPrefab.name);
        button.image.sprite = Resources.Load<Sprite>("Previews/" + towerPref.name + "Preview");
        button.GetComponentInChildren<Text>().text = towerPref.name + " : " + tower.price;
    }

    // Update is called once per frame
    void Update()
    {
        button.interactable = GameManager._instance.CanAfford(tower.price);
    }

    // Buys the related tower
    public void BuyTower()
    {
        GameManager._instance.BuyTower(towerPrefab, tower);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradePanel : MonoBehaviour {

    public Button upgradeButton;
    public Button sellButton;

    void OnEnable()
    {
        UpdateValues();
    }

    void Update()
    {
        upgradeButton.interactable = GameManager._instance.CanAfford(TowerManager._instance.selectedTowerTower.GetUpgradeCost());
    }

    public void UpdateValues()
    {
        upgradeButton.GetComponentInChildren<Text>().text = "Upgrade : " + TowerManager._instance.selectedTowerTower.GetUpgradeCost();
        sellButton.GetComponentInChildren<Text>().text = "Sell : " + TowerManager._instance.selectedTowerTower.GetSellValue();
    }

	public void UpgradeTower()
    {
        GameManager._instance.UpgradeSelectedTower();
    }

    public void SellTower()
    {
        GameManager._instance.SellSelectedTower();
    }
}

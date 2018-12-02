using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerStatsPanel : MonoBehaviour {

    public Text towerNameLevel;
    public Text towerDamage;
    public Text towerFireRate;
    public Text towerRange;
    public Text towerSpecials;


    void OnEnable()
    {
        SetTowerStatsInfo();
    }

    public void SetTowerStatsInfo()
    {
        GameObject tower = TowerManager._instance.selectedTower;
        Tower towerTower = TowerManager._instance.selectedTowerTower;
        string towerType = tower.name.Split('(')[0];
        towerNameLevel.text = towerType + " level " + towerTower.level;
        towerDamage.text = "Damage : \n" + towerTower.damage.ToString("F") + " => " + towerTower.nextLevelDamage.ToString("F") + " <color=#00ffffff>(+" + (towerTower.nextLevelDamage - towerTower.damage).ToString("F") + ")</color>";
        towerFireRate.text = "FireRate : " + (1 / towerTower.fireTime).ToString("F") + ((towerTower.fireTime == towerTower.nextLevelFireTime)?"":" => " + towerTower.nextLevelFireTime + " <color=#00ffffff>(+" + (towerTower.nextLevelFireTime - towerTower.fireTime).ToString("F") + ")</color>");
        towerRange.text = "Range : " + towerTower.range.ToString("F") + ((towerTower.range == towerTower.nextLevelRange) ? "" : " => " + towerTower.nextLevelRange + " <color=#00ffffff>(+" + (towerTower.nextLevelRange - towerTower.range).ToString("F") + ")</color>"); ;

        towerSpecials.text = "Specials : ";

        if (towerType == "IceTower")
        {
            towerSpecials.text += "Slows enemies by 30%";
        }
        else if (towerType == "CanonTower")
        {
            towerSpecials.text += "Deals damage over an area";
        }
        else
        {
            towerSpecials.text += "None";
        }
    }
}

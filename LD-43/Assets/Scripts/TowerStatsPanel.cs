﻿using System.Collections;
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
        towerFireRate.text = "FireRate : \n" + (1 / towerTower.fireTime).ToString("F") + ((towerTower.fireTime == towerTower.nextLevelFireTime)?"":(" => " + (1 / towerTower.nextLevelFireTime).ToString("F") + " <color=#00ffffff>(+" + ((1 / towerTower.nextLevelFireTime) - (1 / towerTower.fireTime)).ToString("F") + ")</color>"));
        towerRange.text = "Range : \n" + towerTower.range.ToString("F") + ((towerTower.range == towerTower.nextLevelRange) ? "" :( " => " + towerTower.nextLevelRange.ToString("F") + " <color=#00ffffff>(+" + (towerTower.nextLevelRange - towerTower.range).ToString("F") + ")</color>"));

        towerSpecials.text = "Specials : \n";

        if (towerType == "IceTower")
        {
            towerSpecials.text += "Slows enemies by 30%\n";
            if (towerTower.level == 4)
                towerSpecials.text += "<color=#00ffffff>Can use up to three beams on different targets</color>";
            if(towerTower.level > 4)
                towerSpecials.text += "Can use up to three beams on different targets";

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

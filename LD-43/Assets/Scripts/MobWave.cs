using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MobWave : MonoBehaviour {
    
    public int basePoints = 10; // basePoints + (wave * 10) * ln(wave * 10) 

    public List<Mob> mobTypes = new List<Mob>();
    public List<Mob> bossTypes = new List<Mob>();

    public List<GameObject> activeSpawners;

    public Dictionary<GameObject, List<Mob>> GenerateWave(int wave) // GameObject is the spawner
    {
        Dictionary<GameObject, List<Mob>> waveInfo = new Dictionary<GameObject, List<Mob>>();

        foreach(GameObject spawner in activeSpawners)
        {
            waveInfo.Add(spawner, new List<Mob>());
        }

        int points = basePoints + (wave * 15 * (int)Mathf.Floor(Mathf.Sqrt(wave)));

        // Debug.Log("points : " + points);

        if(wave >= 15 && wave % 5 == 0)
        {
            waveInfo[activeSpawners[0]].Add(bossTypes[0]);
        }

        while (points > 0)
        {
            List<Mob> availableMobs = new List<Mob>();
            foreach (Mob m in mobTypes)
            {
                if (m.minWave <= wave && m.spawnCost <= points)
                {
                    availableMobs.Add(m);
                }
            }

            if (availableMobs.Count == 0)
            {
                Debug.LogError("Impossibru : " + points + " points");
                break;
            }
            else
            {
                Mob mob = availableMobs[Random.Range(0, availableMobs.Count)];
                waveInfo[activeSpawners[Random.Range(0, activeSpawners.Count)]].Add(mob);

                points -= mob.spawnCost;
            }
        }

        return waveInfo;
    }

}

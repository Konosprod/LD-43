using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobDetection : MonoBehaviour
{
    public List<GameObject> mobsInRange = new List<GameObject>();

    void Update()
    {
        mobsInRange.RemoveAll(mob => mob == null);
    }

    public GameObject GetTargetClosestToGoal()
    {
        GameObject target = null;

        float minDist = Mathf.Infinity;

        foreach (GameObject mob in mobsInRange)
        {
            if(mob != null)
            {
                Mob m = mob.GetComponent<Mob>();
                if (m.canDealDamage)
                {
                    NavMeshAgent nma = mob.GetComponent<NavMeshAgent>();
                    NavMeshPath nmp = nma.path;
                    float dist = 0f;
                    for (int i = 0; i < nmp.corners.Length - 1; i++)
                    {
                        dist += Vector3.Distance(nmp.corners[i], nmp.corners[i + 1]);
                    }

                    if (dist < minDist)
                    {
                        minDist = dist;
                        target = mob;
                    }
                }
            }
        }

        return target;
    }

    public List<GameObject> GetThreeClosestTargets()
    {
        List<GameObject> targets = new List<GameObject>();

        if(mobsInRange.Count <= 3)
        {
            foreach(GameObject go in mobsInRange)
            {
                targets.Add(go);
            }
        }
        else
        {
            for(int k=0; k<3; k++)
            {
                GameObject target = null;

                float minDist = Mathf.Infinity;

                foreach (GameObject mob in mobsInRange)
                {
                    if (mob != null && !targets.Contains(mob))
                    {
                        Mob m = mob.GetComponent<Mob>();
                        if (m.canDealDamage)
                        {
                            NavMeshAgent nma = mob.GetComponent<NavMeshAgent>();
                            NavMeshPath nmp = nma.path;
                            float dist = 0f;
                            for (int i = 0; i < nmp.corners.Length - 1; i++)
                            {
                                dist += Vector3.Distance(nmp.corners[i], nmp.corners[i + 1]);
                            }

                            if (dist < minDist)
                            {
                                minDist = dist;
                                target = mob;
                            }
                        }
                    }
                }

                targets.Add(target);
            }
        }

        return targets;
    }

    public GameObject GetTargetWithLowestHealth()
    {
        GameObject target = null;

        float minHp = Mathf.Infinity;

        foreach (GameObject mob in mobsInRange)
        {
            if (mob != null)
            {
                Mob m = mob.GetComponent<Mob>();
                if (m.canDealDamage)
                {
                    float hp = m.hp;

                    if (hp < minHp)
                    {
                        minHp = hp;
                        target = mob;
                    }
                }
            }
        }

        return target;
    }

    public GameObject GetTargetWithHighestHealth()
    {
        GameObject target = null;

        float maxHp = 0f;

        foreach (GameObject mob in mobsInRange)
        {
            if (mob != null)
            {
                Mob m = mob.GetComponent<Mob>();
                if (m.canDealDamage)
                {
                    float hp = m.hp;

                    if (hp > maxHp)
                    {
                        maxHp = hp;
                        target = mob;
                    }
                }
            }
        }

        return target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == GameManager._instance.mobLayer)
        {
            //Debug.Log("In range : " + other.name);
            mobsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == GameManager._instance.mobLayer)
        {
            //Debug.Log("Out of range : " + other.name);
            mobsInRange.Remove(other.gameObject);
        }
    }
}

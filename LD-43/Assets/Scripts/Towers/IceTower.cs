using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : MonoBehaviour
{

    public float slowValue = 0.7f;
    public IceBeam iceBeam;
    public IceBeam iceBeam2;
    public IceBeam iceBeam3;

    private Tower tower;
    private MobDetection mobDetection;

    private float lastAttack = 0.0f;

    // Use this for initialization
    void Start()
    {
        tower = GetComponent<Tower>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mobDetection == null)
            mobDetection = GetComponentInChildren<MobDetection>();

        if (!tower.isPreviewMode)
        {
            if (mobDetection.mobsInRange.Count > 0)
            {
                if (Time.time > lastAttack + (tower.isBuffedByArrowTower ? tower.fireTime / 1.2f : tower.fireTime)) // Deal damage
                {
                    if (tower.level < 5) // One beam
                    {
                        GameObject target = mobDetection.GetTargetClosestToGoal();
                        if (target != null)
                        {
                            Mob mob = target.GetComponent<Mob>();
                            mob.TakeDamage(tower.damage);
                            mob.ApplySlow(slowValue, tower.fireTime + 0.1f);

                            lastAttack = Time.time;

                            iceBeam.activated = true;
                            iceBeam.target = target;
                        }
                        else
                        {
                            iceBeam.activated = false;
                            iceBeam.target = null;
                        }
                    }
                    else // 3 beams
                    {
                        List<GameObject> targets = mobDetection.GetThreeClosestTargets();
                        if(targets.Count > 0)
                        {
                            int trueTargets = 0;
                            foreach(GameObject target in targets)
                            {
                                if (target != null)
                                {
                                    Mob mob = target.GetComponent<Mob>();
                                    mob.TakeDamage(tower.damage);
                                    mob.ApplySlow(slowValue, tower.fireTime + 0.1f);

                                    if (trueTargets == 0)
                                    {
                                        lastAttack = Time.time;

                                        iceBeam.activated = true;
                                        iceBeam.target = target;
                                    }
                                    else if(trueTargets == 1)
                                    {
                                        iceBeam2.activated = true;
                                        iceBeam2.target = target;
                                    }
                                    else if (trueTargets == 2)
                                    {
                                        iceBeam3.activated = true;
                                        iceBeam3.target = target;
                                    }

                                    trueTargets++;
                                }
                            }

                            if(trueTargets == 0)
                            {
                                ResetBeams();
                            }
                            else if(trueTargets == 1)
                            {
                                ResetBeams23();
                            }
                            else if(trueTargets == 2)
                            {
                                ResetBeam3();
                            }
                        }
                        else
                        {
                            ResetBeams();
                        }
                    }
                }
                else
                {
                    if(!mobDetection.mobsInRange.Contains(iceBeam.target))
                    {
                        iceBeam.activated = false;
                        iceBeam.target = null;
                    }
                    if (!mobDetection.mobsInRange.Contains(iceBeam2.target))
                    {
                        iceBeam2.activated = false;
                        iceBeam2.target = null;
                    }
                    if (!mobDetection.mobsInRange.Contains(iceBeam3.target))
                    {
                        iceBeam3.activated = false;
                        iceBeam3.target = null;
                    }
                }
            }
            else
            {
                ResetBeams();
            }
        }
    }


    private void ResetBeams()
    {
        iceBeam.activated = false;
        iceBeam.target = null;
        iceBeam2.activated = false;
        iceBeam2.target = null;
        iceBeam3.activated = false;
        iceBeam3.target = null;
    }

    private void ResetBeams23()
    {
        iceBeam2.activated = false;
        iceBeam2.target = null;
        iceBeam3.activated = false;
        iceBeam3.target = null;
    }

    private void ResetBeam3()
    {
        iceBeam3.activated = false;
        iceBeam3.target = null;
    }
}

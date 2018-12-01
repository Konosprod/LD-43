using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : MonoBehaviour
{

    public float slowValue = 0.7f;
    public IceBeam iceBeam;

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
                if (Time.time > lastAttack + tower.fireTime) // Deal damage
                {
                    GameObject target = mobDetection.GetTargetClosestToGoal();
                    lastAttack = Time.time;
                    if (target != null)
                    {
                        Mob mob = target.GetComponent<Mob>();
                        mob.TakeDamage(tower.damage);
                        mob.ApplySlow(slowValue, tower.fireTime + 0.1f);

                        iceBeam.activated = true;
                        iceBeam.target = target;
                    }
                    else
                    {
                        iceBeam.activated = false;
                        iceBeam.target = null;
                    }
                }
                else
                {
                    if(!mobDetection.mobsInRange.Contains(iceBeam.target))
                    {
                        iceBeam.activated = false;
                        iceBeam.target = null;
                    }
                }
            }
            else
            {
                iceBeam.activated = false;
                iceBeam.target = null;
            }
        }
    }
}

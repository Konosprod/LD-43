using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : MonoBehaviour {

    public float slowValue = 0.7f;

    private Tower tower;
    public MobDetection mobDetection;

    private float lastAttack = 0.0f;

	// Use this for initialization
	void Start () {
        tower = GetComponent<Tower>();
	}
	
	// Update is called once per frame
	void Update () {
        if(mobDetection == null)
            mobDetection = GetComponentInChildren<MobDetection>();

        if (!tower.isPreviewMode)
        {
            if(mobDetection.mobsInRange.Count > 0)
            {
                if(Time.time > lastAttack + tower.fireTime) // Deal damage
                {
                    GameObject target = mobDetection.GetTargetClosestToGoal();
                    lastAttack = Time.time;
                    if (target != null)
                    {
                        Mob mob = target.GetComponent<Mob>();
                        mob.TakeDamage(tower.damage);
                        mob.ApplySlow(slowValue, tower.fireTime + 0.1f);
                    }
                }
            }
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : MonoBehaviour
{

    public GameObject ArrowPrefab;

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
                if (Time.time > lastAttack + (tower.isBuffedByArrowTower >= 1 ? tower.fireTime / (1 + 0.2f * tower.isBuffedByArrowTower) : tower.fireTime)) // Fire an arrow
                {
                    GameObject target = mobDetection.GetTargetClosestToGoal();
                    lastAttack = Time.time;
                    if (target != null)
                    {
                        Vector3 pos = transform.position;
                        pos += new Vector3(0f, 1f, 0f);
                        GameObject arrow = Instantiate(ArrowPrefab, pos, Quaternion.identity);
                        Projectile proj = arrow.GetComponent<Projectile>();
                        proj.target = target;
                        proj.damage = tower.damage;
                    }
                }
            }
        }
    }
}

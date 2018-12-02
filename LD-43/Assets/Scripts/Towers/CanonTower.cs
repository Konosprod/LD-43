using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonTower : MonoBehaviour {

    public GameObject canonballPrefab;
    public GameObject canonballMegaPrefab;
    public GameObject canon;

    private Tower tower;
    private MobDetection mobDetection;

    private float lastAttack = 0.0f;

    private int shotCounter = 0;

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
                        Vector3 targetPos = new Vector3(target.transform.position.x, canon.transform.position.y, target.transform.position.z);
                        canon.transform.LookAt(targetPos);

                        shotCounter++;

                        if (tower.level >= 5 && shotCounter % 5 == 0)
                        {
                            GameObject canonball = Instantiate(canonballMegaPrefab, canon.transform.position, Quaternion.identity);
                            Projectile proj = canonball.GetComponent<Projectile>();
                            proj.target = target;
                            proj.damage = tower.damage * 2f;
                            proj.radius = 4f;
                            proj.explosive = true;
                        }
                        else
                        {
                            GameObject canonball = Instantiate(canonballPrefab, canon.transform.position, Quaternion.identity);
                            Projectile proj = canonball.GetComponent<Projectile>();
                            proj.target = target;
                            proj.damage = tower.damage;
                            proj.radius = 2f;
                            proj.explosive = true;
                        }
                    }
                }
            }
        }
    }
}

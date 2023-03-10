using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalistaTower : MonoBehaviour {

    public GameObject balistaPrefab;
    public GameObject balista;

    private Tower tower;
    private MobDetection mobDetection;

    private float lastAttack = 0.0f;

    private const float superCooldown = 15.0f;
    private float currentCooldown = 15.0f;

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
                currentCooldown -= Time.deltaTime;
                if(tower.level >= 5 && currentCooldown < 0f && mobDetection.mobsInRange.Count >= 5)
                {
                    // Fire everything, literally
                    currentCooldown = superCooldown;
                    int shotsFired = 0;
                    foreach(GameObject target in mobDetection.mobsInRange)
                    {
                        if (shotsFired >= 15)
                            break;

                        if(target != null)
                        {
                            Vector3 targetPos = new Vector3(target.transform.position.x, balista.transform.position.y, target.transform.position.z);
                            balista.transform.LookAt(targetPos);

                            GameObject balistaProj = Instantiate(balistaPrefab, balista.transform.position, Quaternion.identity);
                            Projectile proj = balistaProj.GetComponent<Projectile>();
                            proj.target = target;
                            proj.damage = tower.damage;

                            shotsFired++;
                        }
                    }
                }
                if (Time.time > lastAttack + (tower.isBuffedByArrowTower >= 1 ? tower.fireTime / (1 + 0.2f * tower.isBuffedByArrowTower) : tower.fireTime)) // Fire an arrow
                {
                    GameObject target = mobDetection.GetTargetClosestToGoal();
                    lastAttack = Time.time;
                    if (target != null)
                    {
                        Vector3 targetPos = new Vector3(target.transform.position.x, balista.transform.position.y, target.transform.position.z);
                        balista.transform.LookAt(targetPos);

                        GameObject balistaProj = Instantiate(balistaPrefab, balista.transform.position, Quaternion.identity);
                        Projectile proj = balistaProj.GetComponent<Projectile>();
                        proj.target = target;
                        proj.damage = tower.damage;
                    }
                }
            }
        }
    }
}

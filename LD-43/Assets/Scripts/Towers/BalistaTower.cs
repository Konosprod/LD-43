using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalistaTower : MonoBehaviour {

    public GameObject balistaPrefab;
    public GameObject balista;

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
                if (Time.time > lastAttack + tower.fireTime) // Fire an arrow
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

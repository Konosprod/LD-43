using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [HideInInspector]
    public float damage = 0f;
    public float speed = 5f;
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public bool explosive = false;
    [HideInInspector]
    public float radius = 2f;

    public GameObject explosionObject;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            if (Vector3.Distance(target.transform.position, transform.position) < 0.3f)
            {
                if (!explosive)
                {
                    target.GetComponent<Mob>().TakeDamage(damage);
                    Destroy(gameObject);
                }
                else
                {
                    if (explosionObject != null)
                    {
                        Instantiate(explosionObject, target.transform.position, Quaternion.identity);
                    }
                    Collider[] cols = Physics.OverlapSphere(target.transform.position, radius, 1 << GameManager._instance.mobLayer);
                    foreach (Collider col in cols)
                    {
                        col.gameObject.GetComponent<Mob>().TakeDamage(damage);
                    }
                    Destroy(gameObject);
                }
            }
            else
            {
                Vector3 positionTarget = target.transform.position;
                positionTarget += new Vector3(0f, 0.1f, 0f);
                transform.LookAt(positionTarget);
                transform.position += (positionTarget - transform.position).normalized * speed * Time.deltaTime;
            }
        }
    }
}

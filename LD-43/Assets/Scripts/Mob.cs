using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [Header("Stats")]
    public int damage = 1;
    public float hp = 20f;

    private bool canDealDamage = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if(canDealDamage && other.gameObject.layer == GameManager._instance.goalLayer)
        {
            GameManager._instance.TakeDamage(damage);
            canDealDamage = false;
            Destroy(this.gameObject);
        }
    }
}

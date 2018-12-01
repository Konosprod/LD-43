using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mob : MonoBehaviour
{
    [Header("Stats")]
    public int damage = 1;
    public float hp = 20f;
    public float speed = 2f;

    [HideInInspector]
    public bool canDealDamage = true;

    // Debuffs
    private NavMeshAgent navMeshAgent;
    private bool isSlowed = false;
    private float slowValue = 0.7f;
    private float slowTime = 0.0f;

    // Use this for initialization
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(slowTime > 0f)
        {
            slowTime -= Time.deltaTime;
        }
        if(slowTime <= 0f)
        {
            slowTime = 0f;
            isSlowed = false;
        }

        if (!isSlowed)
            navMeshAgent.speed = speed;
        else
            navMeshAgent.speed = speed * slowValue;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if(hp <= 0f)
        {
            hp = 0f;
            canDealDamage = false;
            Destroy(this.gameObject);
        }
    }

    public void ApplySlow(float value, float duration)
    {
        slowValue = value;
        slowTime += duration;
        isSlowed = true;
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

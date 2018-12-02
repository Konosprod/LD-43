using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Mob : MonoBehaviour
{
    [Header("Stats")]
    public int spawnCost = 1;
    public int minWave = 1;
    public GameObject mobPrefab;
    public int damage = 1;
    public float maxHp = 20f;
    public float speed = 2f;

    [Header("UI")]
    public Image healthBar;

    public GameObject childMesh;

    [HideInInspector]
    public bool canDealDamage = true;

    [HideInInspector]
    public float hp = 20f;

    // Debuffs
    private NavMeshAgent navMeshAgent;
    private bool isSlowed = false;
    private float slowValue = 0.7f;
    private float slowTime = 0.0f;



    // Use this for initialization
    void Start()
    {
        hp = maxHp;
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
            GameManager._instance.IAmAMobAndIDied(gameObject);
            Destroy(this.gameObject);
        }

        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = hp / maxHp;
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
            GameManager._instance.IAmAMobAndIDied(gameObject);
            Destroy(this.gameObject);
        }
    }
}

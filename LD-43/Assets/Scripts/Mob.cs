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
    public int foodLoot = 10;
    public bool boss = false;

    [Header("UI")]
    public Image healthBar;

    public GameObject childMesh;

    private float animSpeed = 15f;
    private float animShakeAmount = 1.5f;
    private float randFactorStart = 0f;
    private float randFactorSpeed = 0.5f;

    [HideInInspector]
    public bool canDealDamage = true;


    public float hp = 20f;

    // Debuffs
    private NavMeshAgent navMeshAgent;
    private bool isSlowed = false;
    private float slowValue = 0.7f;
    private float slowTime = 0.0f;



    // Use this for initialization
    void Start()
    {
        hp = maxHp * GameManager._instance.GetStatScaleForMobs() * (boss ? GameManager._instance.GetStatScaleForMobs() : 1f);
        maxHp = hp;
        damage = (int)Mathf.Floor(damage * GameManager._instance.GetStatScaleForMobs());
        navMeshAgent = GetComponent<NavMeshAgent>();

        randFactorStart = Random.Range(-1f, 1f);
        randFactorSpeed = Random.Range(0.5f, 1f);
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


        float x = Mathf.Sin(randFactorStart + Time.time * animSpeed * randFactorSpeed) * animShakeAmount;
        childMesh.transform.localPosition = new Vector3(x, 0f, 0f);
        childMesh.transform.localEulerAngles = new Vector3(0f, 0f, -x * 3f);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if(hp <= 0f)
        {
            hp = 0f;
            canDealDamage = false;
            GameManager._instance.IAmAMobAndIDied(gameObject, true);
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
            GameManager._instance.IAmAMobAndIDied(gameObject, false);
            Destroy(this.gameObject);
        }
    }
}

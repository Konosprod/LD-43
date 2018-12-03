using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerMovement : MonoBehaviour
{

    private float initialWanderTimer = 0f;
    private float wanderTimer;

    private NavMeshAgent nma;

    // Use this for initialization
    void Start()
    {
        nma = GetComponent<NavMeshAgent>();
        initialWanderTimer = Random.Range(0f, 3f);
        wanderTimer = initialWanderTimer;
    }

    // Update is called once per frame
    void Update()
    {
        wanderTimer -= Time.deltaTime;
        if (wanderTimer < 0f)
        {
            nma.SetDestination(RandomNavSphere(transform.position, 2f, -1));
            wanderTimer = Random.Range(3f, 6f);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}

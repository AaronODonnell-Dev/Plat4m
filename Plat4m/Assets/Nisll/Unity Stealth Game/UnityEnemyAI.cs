using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnityEnemyAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float chaseWaitTime = 5f;
    public float patrolWaitTime = 1f;
    public Transform[] patrolWayPoints;

    private UnityEnemySight enemySight;
    private NavMeshAgent nav;
    private Transform player;
    //private PlayerHealth playerHealth;
    private LastPlayerSighting lastPlayerSighting;
    private float chaseTimer;
    private float patrolTimer;
    private int wayPointIndex;

    private void Awake()
    {
        enemySight = GetComponent<UnityEnemySight>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //playerHealth = player.GetComponent<PlayerHealth>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag("gameController").GetComponent<LastPlayerSighting>();
    }

    private void Update()
    {
        if (enemySight.playerInSight /*&& playerHealth.health > 0f*/)
        {
            Shooting();
        }
        else if (enemySight.personalLastSighting != lastPlayerSighting.resetPosition /*&& playerHealth.health > 0f*/)
        {
            Chasing();
        }
        else
        {
            Patrolling();
        }
    }

    private void Shooting()
    {
        Debug.Log("Shoting");
        nav.isStopped = true;
    }

    private void Chasing()
    {
        Debug.Log("Chasing");
        nav.isStopped = false;
        Vector3 sightingDeltaPosition = enemySight.personalLastSighting - transform.position;
        if (sightingDeltaPosition.sqrMagnitude > 2f)
        {
            nav.destination = enemySight.personalLastSighting;
        }

        nav.speed = chaseSpeed;

        if (nav.remainingDistance < nav.stoppingDistance)
        {
            chaseTimer += Time.deltaTime;

            if (chaseTimer > chaseWaitTime)
            {
                lastPlayerSighting.position = lastPlayerSighting.resetPosition;
                enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
                chaseTimer = 0f;
            }
        }
        else
        {
            chaseTimer = 0f;
        }
    }

    private void Patrolling()
    {
        Debug.Log("Patrolling");
        nav.isStopped = false;
        nav.speed = patrolSpeed;

        if (nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                if (wayPointIndex == patrolWayPoints.Length-1)
                {
                    wayPointIndex = 0;
                }
                else
                {
                    wayPointIndex++;
                }
            }
        }
        else
        {
            patrolTimer = 0f;
        }
        nav.destination = patrolWayPoints[wayPointIndex].position;
    }
}

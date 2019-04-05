using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float chaseWaitTime = 5f;
    public float patrolWaitTime = 1f;

    [SerializeField] Transform[] patrolWayPoints;

    private float chaseTimer;
    private float patrolTimer;
    private int wayPointIndex;
    private EnemySight enemySight;
    private NavMeshAgent nav;
    private Transform player;
    private VRPlayerHealth playerHealth;
    private InfiltrationManager infiltrationManager;

    private void Awake()
    {
        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag(Tags.vrPlayer).transform;
        playerHealth = player.GetComponent<VRPlayerHealth>();
        infiltrationManager = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<InfiltrationManager>();
    }

    private void Update()
    {
        if(enemySight.playerInSight && playerHealth.currentHealth > 0f)
        {
            Shooting();
        }
        else if (enemySight.personalLastSighting != infiltrationManager.resetPosition && playerHealth.currentHealth > 0f)
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
        nav.isStopped = true;
    }

    private void Chasing()
    {
        nav.isStopped = false;
        Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
        if( sightingDeltaPos.sqrMagnitude > 4f)
        {
            nav.destination = enemySight.personalLastSighting;
        }

        nav.speed = chaseSpeed;

        if( nav.remainingDistance < nav.stoppingDistance)
        {
            chaseTimer += Time.deltaTime;

            if( chaseTimer > chaseWaitTime)
            {
                infiltrationManager.lastSightingPosition = infiltrationManager.resetPosition;
                enemySight.personalLastSighting = infiltrationManager.resetPosition;
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
        nav.isStopped = false;
        nav.speed = patrolSpeed;

        if(nav.destination == infiltrationManager.resetPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;

            if(patrolTimer >= patrolWaitTime)
            {
                if(wayPointIndex >= patrolWayPoints.Length - 1)
                {
                    wayPointIndex = 0;
                }
                else
                {
                    wayPointIndex++;
                }

                patrolTimer = 0f;
            }
        }
        else
        {
            patrolTimer = 0f;
        }

        print("WayPoint Index: " + wayPointIndex);

        nav.destination = patrolWayPoints[wayPointIndex].position;
    }
}

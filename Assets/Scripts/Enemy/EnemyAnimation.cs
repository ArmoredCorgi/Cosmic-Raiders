using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    public float deadZone = 5f;

    private Transform player;
    private EnemySight enemySight;
    private NavMeshAgent nav;
    private Animator anim;
    private HashIDs hash;
    private AnimatorSetup animSetup;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();

        nav.updateRotation = false;
        animSetup = new AnimatorSetup(anim, hash);

        anim.SetLayerWeight(1, 1f); //Layer 1: Shooting
        anim.SetLayerWeight(2, 1f); //Layer 2: Gun

        deadZone *= Mathf.Deg2Rad;
    }

    private void Update()
    {
        NavAnimSetup();
    }

    private void OnAnimatorMove()
    {
        nav.velocity = anim.deltaPosition / Time.deltaTime;
        transform.rotation = anim.rootRotation;
    }

    private void NavAnimSetup()
    {
        float speed;
        float angle;

        if (enemySight.playerInSight)
        {
            speed = 0f;

            angle = FindAngle(transform.forward, player.position - transform.position, transform.up);
        }
        else
        {
            speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;

            angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);

            if(Mathf.Abs(angle) < deadZone)
            {
                transform.LookAt(transform.position + nav.desiredVelocity);
                angle = 0f;
            }
        }

        animSetup.Setup(speed, angle);
    }

    private float FindAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector)
    {
        if(toVector == Vector3.zero)
        {
            return 0f;
        }

        float angle = Vector3.Angle(fromVector, toVector);
        Vector3 normal = Vector3.Cross(fromVector, toVector);
        angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
        angle *= Mathf.Deg2Rad;

        return angle;
    }
}

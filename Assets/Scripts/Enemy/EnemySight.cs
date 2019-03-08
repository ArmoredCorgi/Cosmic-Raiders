using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight : MonoBehaviour
{
    //Public variables:
    public float fieldOfViewAngle = 110f;
    public bool playerInSight; //If enemy can see player (needs to be accessed by decision making script, EnemyAI
    public Vector3 personalLastSighting; //Unique enemy's last sighting of the player

    //Private variables:
    private NavMeshAgent nav; //Use length of the path to player to determine how far enemy can hear
    private SphereCollider enemySphereCol;
    private Animator anim; //has a playerInSight boolean parameter
    private InfiltrationManager infiltrationManager;
    private GameObject player;
    private Animator playerAnim;
    private VRPlayerHealth vrPlayerHealth; //Check player's health, ignore if dead
    private HashIDs hash; //Enum from Utility- stores animation controller states
    private Vector3 previousSighting;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        enemySphereCol = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        infiltrationManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<InfiltrationManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAnim = player.GetComponent<Animator>();
        vrPlayerHealth = player.GetComponent<VRPlayerHealth>();
        hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();

        personalLastSighting = infiltrationManager.resetPosition;
        previousSighting = infiltrationManager.resetPosition;
    }

    private void Update()
    {
        if (infiltrationManager.lastSightingPosition != previousSighting)
        {
            personalLastSighting = infiltrationManager.lastSightingPosition;
        }

        previousSighting = infiltrationManager.lastSightingPosition;

        if (vrPlayerHealth.currentHealth > 0f)
        {
            anim.SetBool(hash.playerInSightBool, playerInSight);
        }
        else
        {
            anim.SetBool(hash.playerInSightBool, false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            //---Check if enemy can SEE the player:

            playerInSight = false; //Default for if any of the following conditions fail

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;

                Vector3 raycastPos = transform.position + transform.up;

                if (Physics.Raycast(raycastPos, direction.normalized, out hit, enemySphereCol.radius))
                {
                    if (hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        infiltrationManager.lastSightingPosition = player.transform.position;
                    }
                }
            }
            //---

            //---Check if enemy can HEAR the player:

            int playerLayerZeroStateHash = playerAnim.GetCurrentAnimatorStateInfo(0).nameHash;
            int playerLayerOneStateHash = playerAnim.GetCurrentAnimatorStateInfo(1).nameHash;

            if (playerLayerZeroStateHash == hash.locomotionState || playerLayerOneStateHash == hash.teleportedState) //NOTE: hash.locomotionState is active when player is moving, hash.teleportedState is active for a short time after player has teleported
            {
                if (CalculatePathLength(player.transform.position) <= enemySphereCol.radius) //Enemy can hear the player!
                {
                    personalLastSighting = player.transform.position; //this enemy's last sighting of the player becomes the player's current position
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
            playerInSight = false;
    }

    private float CalculatePathLength(Vector3 targetPosition) //See Visualization at Documentation/Script_Visualization/EnemySight.pdf
    {
        NavMeshPath path = new NavMeshPath();

        if (nav.enabled)
        {
            nav.CalculatePath(targetPosition, path);
        }

        Vector3[] allWaypoints = new Vector3[path.corners.Length + 2];

        allWaypoints[0] = transform.position; //first waypoint is enemy position
        allWaypoints[allWaypoints.Length - 1] = targetPosition; //final waypoint is target position (the end of the path)

        for (int i = 0; i < path.corners.Length; i++) //for all corners in path
        {
            allWaypoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for (int i = 0; i < allWaypoints.Length - 1; i++) //for all waypoints (except last)
        {
            pathLength += Vector3.Distance(allWaypoints[i], allWaypoints[i + 1]);
        }

        return pathLength;
    }
}

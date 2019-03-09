using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashIDs : MonoBehaviour
{
    //VR Player Animation Hashes:
    public int locomotionState;
    public int teleportedState;

    //PC Player Animation Hashes:
    public int pcWalking;
    public int pcRunning;
    public int pcJumped;

    //Enemy Animation Hashes:
    public int playerInSightBool;
    public int speedFloat;
    public int angularSpeedFloat;
    public int shotFloat;
    public int aimWeightFloat;

    private void Awake()
    {
        locomotionState = Animator.StringToHash("vrLocomotionActive");
        teleportedState = Animator.StringToHash("VRPlayerAC_TeleportedLayer.Teleported");

        pcWalking = Animator.StringToHash("pcWalking");
        pcRunning = Animator.StringToHash("pcRunning");
        pcJumped = Animator.StringToHash("pcJumped");

        playerInSightBool = Animator.StringToHash("PlayerInSight");
        speedFloat = Animator.StringToHash("Speed");
        angularSpeedFloat = Animator.StringToHash("AngularSpeed");
        shotFloat = Animator.StringToHash("Shot");
        aimWeightFloat = Animator.StringToHash("AimWeight");
    }
}

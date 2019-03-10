using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System;

public class VRController : MonoBehaviour {
    
    public SteamVR_TrackedController rightController;
    public bool playerMoveEnabled;
    public bool playerMoving;

    private SteamVR_Controller.Device rightControllerDevice { get { return SteamVR_Controller.Input((int)rightController.controllerIndex); } }

    private EVRButtonId triggerButton = EVRButtonId.k_EButton_SteamVR_Trigger;
    private EVRButtonId gripButton = EVRButtonId.k_EButton_Grip;
    private EVRButtonId touchPad = EVRButtonId.k_EButton_SteamVR_Touchpad;

    private LeftControllerLocomotion leftControllerLocomotion;

    void Awake ()
    {
        playerMoveEnabled = true;
        playerMoving = false;
        leftControllerLocomotion = GetComponentInChildren<LeftControllerLocomotion>();
	}
	
	void Update ()
    {
        if (playerMoveEnabled)
        {
            playerMoving = leftControllerLocomotion.Locomotion();
        }
    }
    
}

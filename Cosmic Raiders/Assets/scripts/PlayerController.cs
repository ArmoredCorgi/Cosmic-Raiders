using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerController : MonoBehaviour {
    
    public SteamVR_TrackedController leftController;
    public SteamVR_TrackedController rightController;

    private SteamVR_Controller.Device leftControllerDevice { get { return SteamVR_Controller.Input((int)leftController.controllerIndex); } }
    private SteamVR_Controller.Device rightControllerDevice { get { return SteamVR_Controller.Input((int)rightController.controllerIndex); } }

    private EVRButtonId triggerButton = EVRButtonId.k_EButton_SteamVR_Trigger;
    private EVRButtonId gripButton = EVRButtonId.k_EButton_Grip;
    private EVRButtonId touchPad = EVRButtonId.k_EButton_SteamVR_Touchpad;
    
    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if( leftControllerDevice.GetPress(touchPad) )
        {
            print("LEFT CONTROLLER TOUCHED");
            Vector2 leftControllerAxis = leftControllerDevice.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
            transform.position += (transform.right * leftControllerAxis.x + transform.forward * leftControllerAxis.y) * Time.deltaTime;

            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            
        }
        if (rightControllerDevice.GetTouchDown(touchPad))
        {
            print("RIGHT CONTROLLER TOUCHED");
        }
    }
}

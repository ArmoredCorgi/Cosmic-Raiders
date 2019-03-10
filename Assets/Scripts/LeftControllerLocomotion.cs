using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LeftControllerLocomotion : MonoBehaviour {

    public Transform vrRig;
    public float movementSpeed = 2.0f;

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device controller;

    private EVRButtonId touchPad = EVRButtonId.k_EButton_SteamVR_Touchpad;
    private Vector2 deviceAxis = Vector2.zero;
    
	void Start ()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();

        controller = SteamVR_Controller.Input((int)trackedObject.index);
    }
    
	public bool Locomotion ()
    {
        if (controller == null)
        {
            return false;
        }

        var device = SteamVR_Controller.Input((int)trackedObject.index);

        if (controller.GetTouch(touchPad))
        {
            deviceAxis = device.GetAxis(EVRButtonId.k_EButton_Axis0);

            if (vrRig != null)
            {
                var saveY = vrRig.position.y;
                vrRig.position += (transform.right * deviceAxis.x + transform.forward * deviceAxis.y) * Time.deltaTime * movementSpeed;
                vrRig.position = new Vector3(vrRig.position.x, saveY, vrRig.position.z);
            }
            return true;
        }

        return false;
    }
}

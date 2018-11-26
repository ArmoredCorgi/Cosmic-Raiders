using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController_FirstPerson : MonoBehaviour {

    FirstPersonController fpsController;
    GameObject reticle;

	// Use this for initialization
	void Start ()
    {
        fpsController = GetComponentInParent<FirstPersonController>();
        reticle = transform.Find("Image_Reticle").gameObject;
	}
	
	void Update ()
    {
        reticle.SetActive(fpsController.isPlayerActive);
	}
}

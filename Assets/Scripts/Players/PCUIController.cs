using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PCUIController : MonoBehaviour {

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
        reticle.SetActive(fpsController.enabled);
	}
}

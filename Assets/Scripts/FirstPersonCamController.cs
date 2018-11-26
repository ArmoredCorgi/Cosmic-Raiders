﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstPersonCamController : MonoBehaviour {

    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;

    Vector2 mouseLook;
    Vector2 smoothV;
    GameObject player;
    FirstPersonController fpsController;

	// Use this for initialization
	void Start ()
    {
        player = this.transform.parent.gameObject;
        fpsController = GetComponentInParent<FirstPersonController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if( fpsController.isPlayerActive )
        {
            var mouse = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            mouse = Vector2.Scale(mouse, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smoothV.x = Mathf.Lerp(smoothV.x, mouse.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, mouse.y, 1f / smoothing);
            mouseLook += smoothV;
            mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

            transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
            player.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, player.transform.up);
        }
	}
}

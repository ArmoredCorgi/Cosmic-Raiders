﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaidersHubManager : MonoBehaviour {

    GameObject[] securityCams;
    List<RenderTexture> camFeeds = new List<RenderTexture>();

	void Start ()
    {
        //Combine the Raider's Hub and Infiltration scenes:
        if (!SceneManager.GetSceneByName("Infiltration").isLoaded)
        {
            SceneManager.LoadScene("Infiltration", LoadSceneMode.Additive);
        }

        //Delay added to allow for scene loading prior to set up:
        Invoke("SecurityCamSetup", 0.5f);
    }

    private void SecurityCamSetup ()
    {
        var count = 1;

        SceneManager.MergeScenes(SceneManager.GetSceneByName("Raider's Hub"), SceneManager.GetSceneByName("Infiltration"));

        //Get all security cameras in scene:
        securityCams = GameObject.FindGameObjectsWithTag("SecurityCam");

        //Set all cameras to correct render texture feed:
        foreach (GameObject securityCam in securityCams)
        {
            var cam = securityCam.GetComponentInChildren<Camera>();
            var texture = new RenderTexture(1024, 1024, 16);
            Material material = new Material(Shader.Find("Standard"));

            cam.targetTexture = texture;
            material.mainTexture = texture;

            print("Texture: " + texture);

            var camFeedMonitor = GameObject.Find("CamFeed" + count);

            Renderer monitorRenderer = camFeedMonitor.GetComponent<Renderer>();

            monitorRenderer.material = material;

            count++;
        }
    }
}

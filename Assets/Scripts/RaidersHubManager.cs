using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class RaidersHubManager : MonoBehaviour {

    GameObject[] securityCams;
    List<RenderTexture> camFeeds = new List<RenderTexture>();

	void Start ()
    {
        //Combine the Raider's Hub and Infiltration scenes:
        if (!SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }

        SceneManager.MergeScenes(SceneManager.GetSceneByBuildIndex(0), SceneManager.GetSceneByBuildIndex(1));

        //Get all security cameras in scene:
        securityCams = GameObject.FindGameObjectsWithTag("SecurityCam");

        //Set all cameras to correct render texture feed:
        var count = 1;

        foreach ( GameObject securityCam in securityCams )
        {
            var cam = securityCam.GetComponentInChildren<Camera>();
            var texture = new RenderTexture(1024, 1024, 16);
            Material material = new Material( Shader.Find("Specular") );

            AssetDatabase.CreateAsset(material, "Assets/Materials/CamFeed" + count + ".mat");
            
            cam.targetTexture = texture;
            material.mainTexture = texture;

            var camFeedMonitor = GameObject.Find("CamFeed" + count);

            Renderer monitorRenderer = camFeedMonitor.GetComponent<Renderer>();

            monitorRenderer.material = material;

            count++;
        }
    }

    private void OnApplicationQuit()
    {
        var count = 1;

        foreach ( GameObject securityCam in securityCams )
        {
            AssetDatabase.DeleteAsset("Assets/Materials/CamFeed" + count + ".mat");
            count++;
        }
    }
}

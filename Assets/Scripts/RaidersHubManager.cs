using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaidersHubManager : MonoBehaviour {

    [SerializeField] GameObject[] camMonitors;
    [SerializeField] Material camFeedMaterial;

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
        var count = 0;

        foreach ( GameObject securityCam in securityCams )
        {
            var cam = securityCam.GetComponentInChildren<Camera>();
            var texture = new RenderTexture(1024, 1024, 16);
            
            cam.targetTexture = texture;

            camFeedMaterial.mainTexture = texture;
        }
    }
}

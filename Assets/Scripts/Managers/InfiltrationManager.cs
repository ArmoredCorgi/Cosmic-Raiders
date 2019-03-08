using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfiltrationManager : MonoBehaviour {
    
	void Start ()
    {
        //Combine the Raider's Hub and Infiltration scenes:
        if (!SceneManager.GetSceneByName("Raider's Hub").isLoaded)
        {
            SceneManager.LoadScene("Raider's Hub", LoadSceneMode.Additive);
            SceneManager.MergeScenes(SceneManager.GetSceneByName("Raider's Hub"), SceneManager.GetSceneByName("Infiltration"));
        }
    }
}

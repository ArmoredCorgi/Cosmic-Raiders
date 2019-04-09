using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfiltrationManager : MonoBehaviour {
    
    //Last Player Sighting settings:
    public Vector3 lastSightingPosition = new Vector3(1000f, 1000f, 1000f);
    public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);

    //Alarm light settings:
    public float lightHighIntensity = 0.25f;
    public float lightLowIntensity = 0f;
    public float fadeSpeed = 7f;

    public float musicFadeSpeed = 1f;

    private AlarmLight alarm;
    private Light mainLight;
    private AudioSource normalAudio;
    private AudioSource panicAudio;
    private AudioSource[] sirens;

    private void Awake()
    {
        alarm = GameObject.FindGameObjectWithTag(Tags.alarmLight).GetComponent<AlarmLight>();
        mainLight = GameObject.FindGameObjectWithTag(Tags.mainLight).GetComponent<Light>();
        normalAudio = GetComponent<AudioSource>();
        panicAudio = transform.Find("secondaryMusic").GetComponent<AudioSource>();
        GameObject[] sirenGOs = GameObject.FindGameObjectsWithTag(Tags.siren);
        sirens = new AudioSource[sirenGOs.Length];

        for( int i = 0; i < sirens.Length; i++)
        {
            sirens[i] = sirenGOs[i].GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        SwitchAlarms();
        MusicFading();
    }

    private void SwitchAlarms()
    {
        alarm.alarmOn = lastSightingPosition != resetPosition;

        float newIntensity;

        newIntensity = alarm.alarmOn ? lightLowIntensity : lightHighIntensity;

        mainLight.intensity = Mathf.Lerp(mainLight.intensity, newIntensity, fadeSpeed * Time.deltaTime);

        for( int i = 0; i < sirens.Length; i++)
        {
            if( lastSightingPosition != resetPosition && sirens[i].isPlaying)
            {
                sirens[i].Play();
            }
            else if( lastSightingPosition == resetPosition)
            {
                sirens[i].Stop();
            }
        }
    }

    private void MusicFading()
    {
        if( lastSightingPosition != resetPosition)
        {
            normalAudio.volume = Mathf.Lerp(normalAudio.volume, 0f, musicFadeSpeed * Time.deltaTime);
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
        }
        else
        {
            normalAudio.volume = Mathf.Lerp(normalAudio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0f, musicFadeSpeed * Time.deltaTime);
        }
    }

    public void EndScene()
    {
        //End Scene, return to Raider's Hub?
    }
}

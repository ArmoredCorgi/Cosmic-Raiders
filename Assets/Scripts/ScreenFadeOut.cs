﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeOut : MonoBehaviour
{
    public float fadeSpeed = 1f;
    
    private RectTransform rectTransform;
    private Image blackScreen;
    private InfiltrationManager infiltrationManager;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        blackScreen = GetComponent<Image>();
        infiltrationManager = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<InfiltrationManager>();

        blackScreen.color = Color.clear;
    }

    private void FadeToBlack()
    {
        blackScreen.color = Color.Lerp(blackScreen.color, Color.black, fadeSpeed * Time.deltaTime);
    }

    public void EndScene()
    {
        blackScreen.enabled = true;
        FadeToBlack();

        if(blackScreen.color.a >= 0.95f)
        {
            infiltrationManager.EndScene();
        }
    }
}

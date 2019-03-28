using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float resetAfterDeathTime = 3f;

    private bool playerDead;
    private float timer;
    private VRController vrController;
    private InfiltrationManager infiltrationManager;
    private ScreenFadeOut screenFadeOut;

    private void Awake()
    {
        maxHealth = 100f;
        currentHealth = maxHealth;

        vrController = GetComponent<VRController>();
        infiltrationManager = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<InfiltrationManager>();
        screenFadeOut = GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<ScreenFadeOut>();
    }

    private void Update()
    {
        if( currentHealth <= 0f)
        {
            if (!playerDead)
            {
                PlayerDying();
            }
            else
            {
                PlayerDead();
            }
        }
    }

    private void PlayerDying()
    {
        playerDead = true;
    }

    private void PlayerDead()
    {
        vrController.playerAlive = false;
        infiltrationManager.lastSightingPosition = infiltrationManager.resetPosition;
        screenFadeOut.EndScene();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
    }
}

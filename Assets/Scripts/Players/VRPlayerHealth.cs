using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    private void Awake()
    {
        maxHealth = 100f;
        currentHealth = maxHealth;
    }
}

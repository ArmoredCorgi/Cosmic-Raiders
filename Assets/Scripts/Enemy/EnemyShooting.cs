using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public float maximumDamage = 100f;
    public float minimumDamage = 45f;
    public float flashIntensity = 3f;
    public float fadeSpeed = 10f;
    public AudioClip shotClip;

    private bool shooting;
    private float scaledDamage;
    private Animator anim;
    private HashIDs hash;
    private LineRenderer laserShotLine;
    private Light laserShotLight;
    private SphereCollider enemySphereCol;
    private Transform player;
    private VRPlayerHealth playerHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        laserShotLine = GetComponentInChildren<LineRenderer>();
        laserShotLight = laserShotLine.gameObject.GetComponent<Light>();
        enemySphereCol = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        playerHealth = player.gameObject.GetComponent<VRPlayerHealth>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();

        laserShotLine.enabled = false;
        laserShotLight.intensity = 0f;

        scaledDamage = maximumDamage - minimumDamage;
    }

    private void Update()
    {
        float shot = anim.GetFloat(hash.shotFloat);

        if (shot > 0.5f && !shooting)
        {
            Shoot();
        }
        if (shot < 0.5f)
        {
            shooting = false;
            laserShotLine.enabled = false;
        }

        laserShotLight.intensity = Mathf.Lerp(laserShotLight.intensity, 0f, fadeSpeed * Time.deltaTime);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        float aimWeight = anim.GetFloat(hash.aimWeightFloat);
        anim.SetIKPosition(AvatarIKGoal.RightHand, player.position + Vector3.up * 1.5f);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, aimWeight);
    }

    private void Shoot()
    {
        shooting = true;
        float fractionalDist = (enemySphereCol.radius - Vector3.Distance(transform.position, player.position)) / enemySphereCol.radius;
        float damage = scaledDamage * fractionalDist + minimumDamage;
        playerHealth.TakeDamage(damage);
        ShotEffects();
    }

    private void ShotEffects()
    {
        laserShotLine.SetPosition(0, laserShotLine.transform.position);
        laserShotLine.SetPosition(1, player.position + Vector3.up * 1.5f);
        laserShotLight.enabled = true;
        laserShotLight.intensity = flashIntensity;
        AudioSource.PlayClipAtPoint(shotClip, laserShotLight.transform.position);
    }
}

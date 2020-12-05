using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour{

    private CharacterController myCharacterController;
    private Animator myAnimator;
    private Rigidbody2D myRigidbody2d;
    private SoundSystemForAnimateObjects mySoundSystem;
    private CharacterStats myCharacterStats;
    private InventorySystem myInventorySystem;
    private ParticleSystem myBloodFX;

    //SerializedFields just for debug purposes
    [SerializeField]
    private float maxHealthPoints;
    [SerializeField]
    private float healthPoints;
    [SerializeField]
    private float minStability;
    [SerializeField]
    private float maxStability;
    [SerializeField]
    private float stability;

    void Start()
    {
        myCharacterStats = GetComponent<CharacterStats>();
        myInventorySystem = GetComponent<InventorySystem>();
        myCharacterController = GetComponent<CharacterController>();
        myRigidbody2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySoundSystem = GetComponent<SoundSystemForAnimateObjects>();
        myBloodFX = GetComponentInChildren<ParticleSystem>();

        InitializeStats();
        InvokeRepeating("RegenerateStability", 0, 1f);
    }

    void Update()
    {

    }

    private void InitializeStats()
    {
        maxHealthPoints = myCharacterStats.currentHealth;
        healthPoints = maxHealthPoints;
        maxStability = myCharacterStats.currentStability;
        stability = maxStability;
        minStability = myCharacterStats.minStability;
    }

    private void CheckHealthState()
    {
        if (healthPoints <= 0)
        {
            myCharacterController.SetAliveState(0);
        }
    }
    private void CheckStability()
    {
        if (myCharacterController.isAlive)
        {
            if (stability <= 0)
            {
                myCharacterController.Fall();
            }
            if (stability > 0)
            {
                myAnimator.SetBool("isFallen", false);
            }
        }
    }

    private void RegenerateStability()
    {
        if (stability < maxStability)
        {
            stability = stability + 5f;
            if (stability > maxStability)
            {
                stability = maxStability;
            }
            CheckStability();
        }
    }

    public void ProcessIncomingHit(float incomingDamage, float incomingStabilityDamage, float appliedForce, float attackVector, GameObject attacker = null)
    {
        CameraEffects.ScreenShakeAtHit();
        if (myCharacterController.isParrying)
        {
            myCharacterController.transform.localScale = new Vector3(-attackVector, transform.localScale.y, transform.localScale.z);
            myRigidbody2d.AddForce(new Vector2(attackVector * appliedForce, 0f), ForceMode2D.Impulse);
            mySoundSystem.PlayParrySounds();
            CharacterController attackersCharacterController = attacker.GetComponent<CharacterController>();
            attackersCharacterController.GetParried(appliedForce*2, -attackVector);
            CheckHealthState();
            CheckStability();
        } else
        {
            CheckHealthState();
            CheckStability();
            TakeHealthDamage(incomingDamage);
            TakeStabilityDamage(incomingStabilityDamage);
            myCharacterController.transform.localScale = new Vector3(-attackVector, transform.localScale.y, transform.localScale.z);
            myRigidbody2d.AddForce(new Vector2(attackVector * appliedForce, 0f), ForceMode2D.Impulse);
            myCharacterController.GetHit();
            mySoundSystem.PlayParrySounds();
            mySoundSystem.PlayPainSounds();
            myBloodFX.Play();
        }
    }
    private void TakeHealthDamage(float incomingDamage)
    {
        float finalDamageValue = incomingDamage - myInventorySystem.GetDefenseValue();
        if (finalDamageValue < 1f)
        {
            finalDamageValue = 1;
        }
        healthPoints -= finalDamageValue;
        CheckHealthState();
        mySoundSystem.PlayPainSounds();
    } 
    private void TakeStabilityDamage(float incomingStabilityDamage)
    {
        stability -= incomingStabilityDamage;
        if (stability < minStability)
        {
            stability = minStability;
        }
        CheckStability();
    }

    public float GetHealthAsPercentage()
    {
        float div = healthPoints/maxHealthPoints;
        //float healthPercentage = div * 100;
        //Debug.Log(healthPercentage);
        return div;
    }
}

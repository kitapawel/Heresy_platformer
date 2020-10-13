using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private CharacterController myCharacterController;
    private Animator myAnimator;
    private Rigidbody2D myRigidbody2d;
    private SoundSystemForAnimateObjects mySoundSystem;
    private CharacterStats myCharacterStats;

    //SerializedFields just for debug purposes
    [SerializeField]
    private float maxHealthPoints;
    [SerializeField]
    private float healthPoints;
    [SerializeField]
    private float maxStability;
    [SerializeField]
    private float stability;

    void Start()
    {
        myCharacterStats = GetComponent<CharacterStats>();
        myCharacterController = GetComponent<CharacterController>();
        myRigidbody2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySoundSystem = GetComponent<SoundSystemForAnimateObjects>();

        InitializeStats();
        InvokeRepeating("RegenerateStability", 0, 5f);
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
    }

    private void CheckHealthState()
    {
        if (healthPoints <= 0)
        {
            myCharacterController.Death();
        }
    }
    private void CheckStability()
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

    private void RegenerateStability()
    {
        if (stability < maxStability)
        {
            stability = stability + 5f;
            CheckStability();
        }
    }

    public void ProcessIncomingHit(float incomingDamage, float incomingStabilityDamage, float appliedForce, float attackVector)
    {
        CheckHealthState();
        CheckStability();
        TakeHealthDamage(incomingDamage);
        TakeStabilityDamage(incomingStabilityDamage);
        myCharacterController.transform.localScale = new Vector3(-attackVector, transform.localScale.y, transform.localScale.z);
        myRigidbody2d.AddForce(new Vector2(attackVector*appliedForce, 0f), ForceMode2D.Impulse);
    }
    private void TakeHealthDamage(float incomingDamage)
    {
        healthPoints -= incomingDamage;
        CheckHealthState();
        mySoundSystem.PlayPainSounds();
    } 
    private void TakeStabilityDamage(float incomingStabilityDamage)
    {
        stability -= incomingStabilityDamage;
        CheckStability();
    }
}

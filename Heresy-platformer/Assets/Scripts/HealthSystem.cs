using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private CharacterController myCharacterController;
    private Animator myAnimator;

    [SerializeField]
    private float maxHealthPoints = 100;
    [SerializeField]
    private float healthPoints = 100;
    [SerializeField]
    private float maxStability = 100;
    [SerializeField]
    private float stability = 100;

    void Start()
    {
        myCharacterController = GetComponent<CharacterController>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        RegenerateStability();
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
            stability = stability + 0.2f;
            CheckStability();
        }
    }

    public void ProcessIncomingHit(float incomingDamage, float incomingStabilityDamage)
    {
        CheckHealthState();
        CheckStability();
        TakeHealthDamage(incomingDamage);
        TakeStabilityDamage(incomingStabilityDamage);
    }
    private void TakeHealthDamage(float incomingDamage)
    {
        healthPoints -= incomingDamage;
        CheckHealthState();
    } 
    private void TakeStabilityDamage(float incomingStabilityDamage)
    {
        stability -= incomingStabilityDamage;
        CheckStability();
    }
}

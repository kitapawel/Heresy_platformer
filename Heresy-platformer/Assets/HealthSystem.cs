using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private CharacterMovement myCharacterMovement;
    private Animator myAnimator;

    [SerializeField]
    private int maxHealthPoints = 100;
    [SerializeField]
    private int healthPoints = 100;
    [SerializeField]
    private int maxStability = 100;
    [SerializeField]
    private int stability = 100;

    void Start()
    {
        myCharacterMovement = GetComponent<CharacterMovement>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        MonitorHealthState();
        MonitorStability();
    }

    private void MonitorHealthState()
    {
        if (healthPoints <= 0)
        {
            myCharacterMovement.Death();
        }
    }
    private void MonitorStability()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            stability = -3000;
        }
        if (stability <= 0)
        {
            myCharacterMovement.Fall();
        }
        if (stability < maxStability)
        {
            RegenerateStability();
        }
        if (stability > 0)
        {
            myAnimator.SetBool("isFallen", false);
        }
    }

    private void RegenerateStability()
    {
        stability = stability + 1;
    }
}

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
    private float maxEnergy;
    [SerializeField]
    private float energy;
    [SerializeField]
    private float minEnergy;

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
        StartCoroutine(RegenerateEnergy());
    }


    private void InitializeStats()
    {
        maxHealthPoints = myCharacterStats.baseHealth;
        healthPoints = maxHealthPoints;
        maxEnergy = myCharacterStats.baseEnergy;
        energy = maxEnergy;
        minEnergy = myCharacterStats.minEnergy;
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
            if (energy <= 0)
            {
                myCharacterController.Fall();
            }
        }
    }
    private void CheckIfCanGetUp()// split into fall and get up
    {
        if (myCharacterController.isAlive)
        {
            if (energy > 0)
            {
                myAnimator.SetBool("isFallen", false);
            }
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
            UseEnergy(/*parry proficiency minus*/ incomingStabilityDamage);
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
        energy -= incomingStabilityDamage;
        if (energy < minEnergy)
        {
            energy = minEnergy;
        }
        CheckStability();
    }

    public float GetHealthAsPercentage()
    {
        float div = healthPoints/maxHealthPoints;
        return div;
    }    
    public float GetEnergyAsPercentage()
    {
        float div = energy/maxEnergy;
        return div;
    }

    public bool CanUseEnergyBasedAction(float energyCost)
    {
        if (energyCost <= energy)
        {
            Debug.Log("Enough energy to use skill.");
            return true;
        } else
        {
            Debug.LogError("Tried to use ability that costs " + energyCost + 
                " but only the following number of energy points left: " + energy);
            return false;
        }
    }
    public void UseEnergy(float energyCost)
    {
        energy -= energyCost;
        Debug.Log("Used energy");
    }

    IEnumerator RegenerateEnergy()
    {
        while (myCharacterController.isAlive)
        {
            energy = energy + myCharacterStats.energyRegen;
            if (energy > maxEnergy)
            {
                energy = maxEnergy;
            }
            CheckIfCanGetUp();
            yield return new WaitForSeconds(1);
        }
    }
}

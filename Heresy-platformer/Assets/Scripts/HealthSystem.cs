using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

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
    private float healthPoints;
    [SerializeField]
    private float energy;
    [SerializeField]
    private float minEnergy;
    [SerializeField]
    private float vitality;
    [SerializeField]
    private float healthRegen;
    [SerializeField]
    private float energyRegen;
    [SerializeField]
    private float healthRegenCost;
    [SerializeField]
    private float energyRegenCost;

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
        healthPoints = myCharacterStats.maxHealth;
        energy = myCharacterStats.maxEnergy;
        minEnergy = myCharacterStats.minEnergy;
        vitality = myCharacterStats.maxVitality;
        healthRegen = myCharacterStats.healthRegen;
        energyRegen = myCharacterStats.energyRegen;
        healthRegenCost = myCharacterStats.healthRegenCost;
        energyRegenCost = myCharacterStats.energyRegenCost;

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

    public void ProcessIncomingHit(float incomingDamage, float incomingPiercingDamage, float incomingStabilityDamage, float appliedForce, float attackVector, GameObject attacker = null)
    {
        ProCamera2DShake.Instance.Shake("PlayerHit");
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
            TakeHealthDamage(incomingDamage, incomingPiercingDamage);
            TakeStabilityDamage(incomingStabilityDamage);
            myCharacterController.transform.localScale = new Vector3(-attackVector, transform.localScale.y, transform.localScale.z);
            myRigidbody2d.AddForce(new Vector2(attackVector * appliedForce, 0f), ForceMode2D.Impulse);
            myCharacterController.GetHit();
            mySoundSystem.PlayParrySounds();
            mySoundSystem.PlayPainSounds();
            myBloodFX.Play();
        }
    }
    private void TakeHealthDamage(float incomingDamage, float incomingPiercingDamage)
    {
        float damageReduction = incomingDamage * myInventorySystem.GetDefenseValue();
        float finalDamageValue = incomingDamage - damageReduction;
        if (finalDamageValue < 0f)
        {
            finalDamageValue = 0f;
        }
        finalDamageValue += incomingPiercingDamage;
        healthPoints -= finalDamageValue;
        CheckHealthState();
        mySoundSystem.PlayPainSounds();
    } 
    private void TakeStabilityDamage(float incomingStabilityDamage)
    {
        float finalDamageValue = incomingStabilityDamage - myInventorySystem.GetStabilityValue();
        if (finalDamageValue < 1f)
        {
            finalDamageValue = 1;
        }
        if (energy < minEnergy)
        {
            energy = minEnergy;
        }
        energy -= incomingStabilityDamage;
        CheckStability();
    }

    public float GetHealthAsPercentage()
    {
        float div = healthPoints/ myCharacterStats.maxHealth;
        return div;
    }    
    public float GetEnergyAsPercentage()
    {
        float div = energy/ myCharacterStats.maxEnergy;
        return div;
    }
    public float GetVitalityAsPercentage()
    {
        float div = vitality/myCharacterStats.maxVitality;
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
            if (energy < myCharacterStats.maxEnergy)
            {
                energy = energy + energyRegen;
                vitality -= energyRegenCost;
                if (energy > myCharacterStats.maxEnergy)
                {
                    energy = myCharacterStats.maxEnergy;
                }
                CheckIfCanGetUp();
                yield return new WaitForSeconds(1);
            }
            yield return new WaitForSeconds(1);
        }        
    }
    public void QuickEnergyRegen()
    {
        if (vitality >= 10 && energy < myCharacterStats.maxEnergy)//TODO parameterize
        {
            energy = energy + 20;
            if (energy > myCharacterStats.maxEnergy)
            {
                energy = myCharacterStats.maxEnergy;
                mySoundSystem.PlayEffortSounds();
            }
            vitality -= 10;
        }
    }
}

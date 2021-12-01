using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class HealthSystem : MonoBehaviour{

    [SerializeField] CharacterStats characterStats;
    private CharacterController myCharacterController;
    private Animator myAnimator;
    private Rigidbody2D myRigidbody2d;
    private SoundSystemForAnimateObjects mySoundSystem;
    private InventorySystem myInventorySystem;
    private ParticleSystem myBloodFX;

    //SerializedFields just for debug purposes
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float maxEnergy;
    [SerializeField]
    private float currentEnergy;
    [SerializeField]
    private float maxVitality;
    [SerializeField]
    private float currentVitality;
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
        maxHealth = characterStats.baseHealth;
        currentHealth = characterStats.baseHealth;
        maxEnergy = characterStats.baseEnergy;
        currentEnergy = characterStats.baseEnergy;
        maxVitality = characterStats.baseVitality;
        currentVitality = characterStats.baseVitality;
        healthRegen = characterStats.healthRegen;
        energyRegen = characterStats.energyRegen;
        healthRegenCost = characterStats.healthRegenCost;
        energyRegenCost = characterStats.energyRegenCost;

    }

    private void CheckHealthState()
    {
        if (currentHealth <= 0)
        {
            myCharacterController.SetAliveState(0);
        }
    }
    private void CheckStability()
    {
        if (myCharacterController.isAlive)
        {
            if (currentEnergy <= 0)
            {
                myCharacterController.Fall();
            }
        }
    }
    private void CheckIfCanGetUp()// split into fall and get up
    {
        if (myCharacterController.isAlive)
        {
            if (currentEnergy > 0)
            {
                myAnimator.SetBool("isFallen", false);
            }
        }
    }

    public void ProcessIncomingHit(float incomingDamage, float incomingArmorPenetration, float incomingStabilityDamage, float appliedForce, float attackVector, GameObject attacker = null)
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
            TakeHealthDamage(incomingDamage, incomingArmorPenetration);
            //TakeStabilityDamage(incomingStabilityDamage);
            myCharacterController.transform.localScale = new Vector3(-attackVector, transform.localScale.y, transform.localScale.z);
            myRigidbody2d.AddForce(new Vector2(attackVector * appliedForce, 0f), ForceMode2D.Impulse);
            myCharacterController.GetHit();
            mySoundSystem.PlayParrySounds();
            mySoundSystem.PlayPainSounds();
            myBloodFX.Play();
        }
    }
    private void TakeHealthDamage(float incomingDamage, float incomingArmorPenetration)
    {
        float defenseValue = myInventorySystem.GetDefenseValue();
        float damageReduction = defenseValue - incomingArmorPenetration;
        if (damageReduction < 0f)
        {
            damageReduction = 0f;
        }
        float finalDamageValue = incomingDamage - damageReduction;
        if (finalDamageValue < 1f)
        {
            finalDamageValue = 1f;
        }
        Debug.Log("Incoming dmg: " + incomingDamage + ", Armor Pen: " + incomingArmorPenetration + ", Defense: " + defenseValue);
        Debug.Log("Dmg reduction: " + damageReduction + ", Final damage: " + finalDamageValue);
        currentHealth -= finalDamageValue;
        CheckHealthState();
        mySoundSystem.PlayPainSounds();
    } 
/*    private void TakeStabilityDamage(float incomingStabilityDamage)
    {
        float finalDamageValue = incomingStabilityDamage - myInventorySystem.GetStabilityValue();
        if (finalDamageValue < 1f)
        {
            finalDamageValue = 1f;
        }
        if (energy < minEnergy)
        {
            energy = minEnergy;
        }
        energy -= incomingStabilityDamage;
        CheckStability();
    }*/

    public float GetHealthAsPercentage()
    {
        float div = currentHealth / maxHealth;
        return div;
    }    
    public float GetEnergyAsPercentage()
    {
        float div = currentEnergy / maxEnergy;
        return div;
    }
    public float GetVitalityAsPercentage()
    {
        float div = currentVitality / maxVitality;
        return div;
    }

    public bool CanUseEnergyBasedAction(float energyCost)
    {
        if (energyCost <= maxEnergy)
        {
            return true;
        } else
        {
            Debug.LogWarning("Tried to use ability that costs " + energyCost + 
                " but only the following number of energy points left: " + currentEnergy);
            return false;
        }
    }
    public void UseEnergy(float energyCost)
    {
        currentEnergy -= energyCost;
    }

    IEnumerator RegenerateEnergy()
    {
        while (myCharacterController.isAlive)
        {
            if (currentEnergy < maxEnergy)
            {
                currentEnergy = currentEnergy + energyRegen;
                currentVitality -= energyRegenCost;
                if (currentEnergy > maxEnergy)
                {
                    currentEnergy = maxEnergy;
                }
                CheckIfCanGetUp();
                yield return new WaitForSeconds(1);
            }
            yield return new WaitForSeconds(1);
        }        
    }
    public void QuickEnergyRegen()
    {
        if (currentVitality >= 10 && currentEnergy < maxEnergy)//TODO parameterize
        {
            currentEnergy = currentEnergy + 20;
            if (currentEnergy > maxEnergy)
            {
                currentEnergy = maxEnergy;
                mySoundSystem.PlayEffortSounds();
            }
            currentVitality -= 10;
        }
    }
}

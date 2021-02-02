using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-95)]
public class CharacterStats : MonoBehaviour
{
    [Header("Survivability stats:")]
    public float baseHealth;
    public float baseEnergy;
    public float minEnergy;
    public float baseVitality;

    public float energyRegen;
    public float healthRegen;
    
    public float energyRegenCost;
    public float healthRegenCost;


    
    [Header("Combat stats:")]
    public float baseDamageBonus;
    public float currentDamageBonus;
    public float baseCritRate;
    public float currentCritRate;
    public float baseCritBonus;
    public float currentCritBonus;

    private void Awake()
    {
        DetermineBaseStats();
    }

    private void DetermineBaseStats()
    {
        //TODO read stats from a save file
        baseHealth = Mathf.Clamp(50, 1f, 100f); // TODO in the future, clamp the incoming values within allowed ranges, using Properties get/set
        healthRegen = 1f;
        healthRegenCost = 2f;

        baseEnergy = 50f;
        minEnergy = -10f;
        energyRegen = 1f;
        energyRegenCost = 0.1f;

        healthRegen = 1f;

        baseVitality = 100f;


        baseDamageBonus = 1f;
        currentDamageBonus = baseDamageBonus;

        baseCritRate = 0.05f;
        currentCritRate = baseCritRate;

        baseCritBonus = 1.10f;
        currentCritBonus = baseCritBonus;
    }
}

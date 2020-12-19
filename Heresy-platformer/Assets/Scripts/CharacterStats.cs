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

        baseEnergy = 50;
        minEnergy = -10;

        energyRegen = 1;
        healthRegen = 1;

        baseVitality = 100;


        baseDamageBonus = 1;
        currentDamageBonus = baseDamageBonus;

        baseCritRate = 0.05f;
        currentCritRate = baseCritRate;

        baseCritBonus = 1.10f;
        currentCritBonus = baseCritBonus;
    }
}

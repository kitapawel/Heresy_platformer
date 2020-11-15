using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-95)]
public class CharacterStats : MonoBehaviour
{
    [Header("Survivability stats:")]
    public float baseHealth;
    public float currentHealth;
    public float baseEnergy;
    public float currentEnergy;
    public float baseStability;
    public float minStability;
    public float currentStability;
    public float baseVitality;
    public float currentVitality;
    
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
        currentHealth = baseHealth;

        baseEnergy = 50;
        currentEnergy = baseEnergy;

        minStability = -10;
        baseStability = 20;
        currentStability = baseStability;

        baseVitality = 100;
        currentVitality = baseVitality;

        baseDamageBonus = 1;
        currentDamageBonus = baseDamageBonus;

        baseCritRate = 0.05f;
        currentCritRate = baseCritRate;

        baseCritBonus = 1.10f;
        currentCritBonus = baseCritBonus;
    }
}

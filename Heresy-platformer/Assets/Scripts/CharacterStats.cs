using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-95)]
public class CharacterStats : MonoBehaviour
{
    [Header("General stats:")]
    public int level;

    [Header("Survivability stats:")]
    public float baseHealth;
    public float baseEnergy;
    public float baseVitality;

    public float maxHealth;
    public float maxEnergy;
    public float maxVitality;

    public float minEnergy;

    public float energyRegen;
    public float healthRegen;    
    public float energyRegenCost;
    public float healthRegenCost;

    public float attackEfficiency; // multiplier of weapon/tool use cost
    public float actionCost;
    public float runCost;

    [Header("Combat stats:")]
    public float baseDamageBonus;
    public float currentDamageBonus;
    public float baseCritRate;
    public float currentCritRate;
    public float baseCritBonus;
    public float currentCritBonus;

    public float weakAttackMultiplier;
    public float normalAttackMultiplier;
    public float strongAttackMultiplier;

    public float primaryAttackLevel;
    public float secondaryAttackLevel;

    [Header("Animations:")]
    Animator myAnimator;
    [SerializeField] AnimatorOverrideController animatorOverrideController;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        InitializeCharacter();
    }

    private void InitializeCharacter()
    {
        //TODO read stats from a save file
        baseHealth = Mathf.Clamp(50, 1f, 100f); // TODO in the future, clamp the incoming values within allowed ranges, using Properties get/set
        maxHealth = baseHealth;
        healthRegen = 1f;
        healthRegenCost = 2f;

        baseEnergy = 50f;
        maxEnergy = baseEnergy;
        minEnergy = -10f;
        energyRegen = 1f;
        energyRegenCost = 0.1f;

        attackEfficiency = 1f;
        actionCost = 5f;
        runCost = 1f;

        baseVitality = 100f;
        maxVitality = baseVitality;


        baseDamageBonus = 1f;
        currentDamageBonus = baseDamageBonus;

        baseCritRate = 0.05f;
        currentCritRate = baseCritRate;

        baseCritBonus = 1.10f;
        currentCritBonus = baseCritBonus;

        weakAttackMultiplier = 0.8f;
        normalAttackMultiplier = 1f;
        strongAttackMultiplier = 1.2f;

        primaryAttackLevel = 0;
        secondaryAttackLevel = 0;
    }


}

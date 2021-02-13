using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-95)]
public class CharacterStats : MonoBehaviour
{
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

    public float attackCost;
    public float actionCost;
    public float runCost;

    [Header("Combat stats:")]
    public float baseDamageBonus;
    public float currentDamageBonus;
    public float baseCritRate;
    public float currentCritRate;
    public float baseCritBonus;
    public float currentCritBonus;

    [Header("Animations:")]
    Animator myAnimator;
    [SerializeField] AnimatorOverrideController animatorOverrideController;
    public AnimationClip basicPrimaryAttack;
    public AnimationClip basicSecondaryAttack;
    public AnimationClip basicTiredAttack;
    public AnimationClip basicParry;
    public AnimationClip basicParryRiposte;
    public AnimationClip basicDodge;
    public AnimationClip basicRoll;

    const string ATTACK_PRIMARY = "Default_Primary_Attack";
    const string ATTACK_SECONDARY = "Default_Secondary_Attack";
    const string ATTACK_TIRED = "Default_Tired_Attack";
    const string PARRY = "Default_Parry";
    const string PARRY_RIPOSTE = "Default_Parry_Riposte"; 
    const string DODGE = "Default_Dodge";
    const string ROLL = "Default_Roll";

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

        attackCost = 5f;
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

        myAnimator.runtimeAnimatorController = animatorOverrideController;
        animatorOverrideController[ATTACK_PRIMARY] = basicPrimaryAttack;
        animatorOverrideController[ATTACK_SECONDARY] = basicSecondaryAttack;
        animatorOverrideController[ATTACK_TIRED] = basicTiredAttack;
        animatorOverrideController[PARRY] = basicParry;
        animatorOverrideController[PARRY_RIPOSTE] = basicParryRiposte;
        animatorOverrideController[DODGE] = basicDodge;
        animatorOverrideController[ROLL] = basicRoll;
    }
}

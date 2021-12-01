using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-95)]
[CreateAssetMenu(fileName = "NewCharacterStatsProfile", menuName = "ScriptableObjects/CharacterStats", order = 4)]
public class CharacterStats : ScriptableObject
{
    [Header("General stats:")]
    public int level;
    public FactionType factionType;

    [Header("Survivability stats:")]
    public float baseHealth = Mathf.Clamp(50, 1f, 100f);
    public float baseEnergy = Mathf.Clamp(50, 1f, 100f);
    public float baseVitality = Mathf.Clamp(100, 1f, 200f);

    public float healthRegen = 1f;
    public float healthRegenCost = 2f;

    public float energyRegen = 1f;
    public float energyRegenCost = .1f;


    public float attackEfficiency = 1f; // multiplier of weapon/tool use cost
    public float actionCost = 5f;
    public float runCost = 1f;

    [Header("Combat stats:")]
    public float baseDamageBonus = 0;
    public float baseCritRate = 0.05f;
    public float baseCritBonus = 1.10f;


    public float weakAttackMultiplier = 0.8f;
    public float normalAttackMultiplier = 1f;
    public float strongAttackMultiplier = 1.2f;

    public float primaryAttackLevel = 0;
    public float secondaryAttackLevel = 0;

    [Header("Animations:")]
    [SerializeField] AnimatorOverrideController defaultAnimatorOverrideController;

}

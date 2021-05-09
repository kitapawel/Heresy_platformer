﻿using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon : Item
{
    public WeaponType weaponType;
    
    public float minDamage;
    public float maxDamage;
    public float stabilityDamage;
    public float force;
    public float armorPenetration;// flat value that goes through organic armor
    public float critRateBonus;
    public float critDamageBonus;
    public float energyCost;

    public float structuralDamage; // damage reduced by rigidity of structural object
    public float structuralPenetration; // flat value that goes through structural armor

    public float GetWeaponDamage()
    {
        float weaponDamage = Mathf.Round(Random.Range(minDamage, maxDamage));
        return weaponDamage;
    }
}

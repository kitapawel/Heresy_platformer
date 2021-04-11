using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon : Item
{
    public WeaponType weaponType;

    public float damage;
    public float stabilityDamage;
    public float force;
    public float armorPenetration;// flat value that goes throug organic armor
    public float critRateBonus;
    public float critDamageBonus;
    public float energyCost;

    public float structuralDamage;
    public float impact; // flat value that goes throug structural armor
}

using UnityEngine;

[CreateAssetMenu(fileName = "NewAccessory", menuName = "ScriptableObjects/Accessory", order = 5)]
public class Accessory : Item
{
    public int inventorySlots;
    public float damage;
    public float stabilityDamage;
    public float force;
    public float armorPenetration; // flat value that goes throug organic armor
    public float critRateBonus;
    public float critDamageBonus;
    public float energyCost;

    public float structuralDamage;


}

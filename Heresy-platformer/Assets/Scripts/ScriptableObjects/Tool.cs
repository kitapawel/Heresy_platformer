using UnityEngine;

[CreateAssetMenu(fileName = "NewTool", menuName = "ScriptableObjects/Tool", order = 3)]
public class Tool : Item
{
    public ToolType toolType;

    public float damage;
    public float stabilityDamage;
    public float force;
    public float armorPenetration; // flat value that goes throug organic armor
    public float critRateBonus;
    public float critDamageBonus;
    public float energyCost;

    public float structuralDamage;
    public float impact; // flat value that goes throug structural armor
}

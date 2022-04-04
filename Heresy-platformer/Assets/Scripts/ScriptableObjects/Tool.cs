using UnityEngine;

[CreateAssetMenu(fileName = "NewTool", menuName = "ScriptableObjects/Tool", order = 3)]
public class Tool : Item
{
    public ToolType toolType;

    public float minDamage;
    public float maxDamage;
    public float force;
    public float armorPenetration; // flat value that goes throug organic armor
    public float critRateBonus;
    public float critDamageBonus;
    public float energyCost;

    public float structuralDamage;

    public float GetToolType()
    {
        float value = 0;
        if (toolType == ToolType.Axe)
            value = 0;
        else if (toolType == ToolType.Pick)
            value = 1;
        else if(toolType == ToolType.Shovel)
            value = 2;
        return value;
    }

}

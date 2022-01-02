using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "ScriptableObjects/Armor", order = 2)]
public class Armor : Item
{
    public ArmorType armorType;

    public float defense; // 1 for each point of defense
    public float poise; // reduces force applied by hit
    public float weight; // impact on user's movement

    public int projectileSlots;
    public int inventorySlots;

    //TODO Implement slots for modifiers, rarity for number of modifiers and getter methods to calculate values+mods
}

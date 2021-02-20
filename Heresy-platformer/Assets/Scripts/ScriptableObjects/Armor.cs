using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "ScriptableObjects/Armor", order = 2)]
public class Armor : Item
{
    public ArmorType armorType;

    public float defense; // reduces damage
    public float stability; // reduces stability damage
    public float poise; // reduces force applied by hit
    public float weight; // impact on user's movement

    public int projectileSlots;
    public int inventorySlots;

    //TODO Implement slots for modifiers, rarity for number of modifiers and getter methods to calculate values+mods

}

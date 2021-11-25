using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "ScriptableObjects/Armor", order = 2)]
public class Armor : Item
{
    public ArmorType armorType; // light 10, medium 20, heavy 30

    public float defense; // 1 for each point of defense
    public float stability; // reduces stability damage
    public float poise; // reduces force applied by hit
    public float weight; // impact on user's movement

    public int projectileSlots;
    public int inventorySlots;

    public override void UseItem()
    {

    }

    //TODO Implement slots for modifiers, rarity for number of modifiers and getter methods to calculate values+mods
}

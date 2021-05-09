using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "ScriptableObjects/Armor", order = 2)]
public class Armor : Item
{
    public ArmorType armorType; // light 0, medium 0.2, heavy 0.4

    public float defense; // 0.03 for each point of defense
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

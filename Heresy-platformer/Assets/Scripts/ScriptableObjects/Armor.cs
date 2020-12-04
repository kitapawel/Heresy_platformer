using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "ScriptableObjects/Armor", order = 2)]
public class Armor : ScriptableObject
{
    public string prefabName;
    public GameObject prefab;
    public ArmorType armorType;


    public float defense; // reduces damage
    public float stability; // reduces stability damage
    public float poise; // reduces force applied by hit
    public float weight; // impact on user's movement 

    public string flavourText;
}

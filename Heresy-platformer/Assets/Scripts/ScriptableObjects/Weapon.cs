using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    public string prefabName;
    public WeaponType weaponType;


    public float damage;
    public float stabilityDamage;
    public float force;
    public float armorPenetration;
    public float critRateBonus;
    public float critDamageBonus;
    public float energyCost;

    public string flavourText;
}

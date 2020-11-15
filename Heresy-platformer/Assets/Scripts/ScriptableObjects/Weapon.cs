using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    public string prefabName;
    public WeaponType weaponType;


    public float minimumDamage;
    public float maximumDamage;
    public float minimumStabilityDamage;
    public float maximumStabilityDamage;
    public float minimumForce;
    public float maximumForce;
    public float armorPenetration;
    public float critRateBonus;
    public float critDamageBonus;
    public float energyCost;

    public string flavourText;
}

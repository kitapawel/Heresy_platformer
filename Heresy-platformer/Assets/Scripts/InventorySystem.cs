using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public Weapon defaultWeapon;
    public Armor defaultArmor;

    public Weapon equippedWeapon;
    public Armor equippedArmor;
    public ProjectileRotating equippedProjectile;

    public GameObject thrownStartingPoint;

    private void Start()
    {
        InitializeInventory();
    }

    private void InitializeInventory()
    {
        if (!equippedWeapon)
        {
            equippedWeapon = defaultWeapon;
        }
        if (!equippedArmor)
        {
            equippedArmor = defaultArmor;
        }
    }

    public void SwitchItem(ScriptableObject scriptableObject)
    {
        if (scriptableObject.GetType() == typeof(Weapon))
        {
            //Vector3 position = gameObject.GetComponent<CombatSystem>().thrownStartingPoint.transform;
            GameObject droppedWeapon = Instantiate(equippedWeapon.prefab);
            droppedWeapon.transform.position = thrownStartingPoint.transform.position;
            droppedWeapon.transform.parent = null;
            equippedWeapon = scriptableObject as Weapon;
            //TODO notify CharacterController to switch animations
        } 
        else if (scriptableObject.GetType() == typeof(Armor))
        {
            //Vector3 position = gameObject.GetComponent<CombatSystem>().thrownStartingPoint.transform;
            GameObject droppedArmor = Instantiate(equippedArmor.prefab);
            droppedArmor.transform.position = thrownStartingPoint.transform.position;
            droppedArmor.transform.parent = null;
            equippedArmor = scriptableObject as Armor;
        }

    }
    public void SwitchItem(Armor newArmor)
    {
        GameObject droppedArmor = Instantiate(equippedArmor.prefab, transform.parent);
        equippedArmor = newArmor;
        //TODO notify CharacterController to switch animations
    }

    public float GetDefenseValue()
    {
        float defenseRandomValue = Mathf.Round(Random.Range(0f, equippedArmor.defense));
        Debug.Log("InventorySystem, armor value = " + defenseRandomValue);
        return defenseRandomValue;
    }
    public float GetStabilityValue()
    {
        float stability = equippedArmor.stability;
        return stability;
    }
    public float GetPoiseValue()
    {
        float poise = equippedArmor.poise;
        return poise;
    }

}

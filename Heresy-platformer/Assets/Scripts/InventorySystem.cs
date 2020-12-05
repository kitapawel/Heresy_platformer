using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public Weapon defaultWeapon;
    public Armor defaultArmor; 

    public Weapon equippedWeapon;
    public Armor equippedArmor;
    public int projectileCount;
    public int baseInventorySpace;
    public int inventorySpace;
    public PlayerCanvasController playerCanvasController;

    public ProjectileRotating equippedProjectile;

    public GameObject thrownStartingPoint;

    public List<Item> items = new List<Item>();

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
        projectileCount = equippedArmor.projectileSlots;
        inventorySpace = baseInventorySpace + equippedArmor.inventorySlots;
    }

    //Initiated from the PickableItem script, flows to the PlayerCanvasController to update inventory UI
    public void EquipItem(ScriptableObject scriptableObject)
    {
        if (scriptableObject.GetType() == typeof(Weapon))
        {            
            GameObject droppedWeapon = Instantiate(equippedWeapon.prefab);
            droppedWeapon.transform.position = thrownStartingPoint.transform.position;
            droppedWeapon.transform.parent = null;
            equippedWeapon = scriptableObject as Weapon;
            //TODO notify CharacterController to switch animations
        } 
        else if (scriptableObject.GetType() == typeof(Armor))
        {
            //TODO cannot equip if more items would overflow inventory
            GameObject droppedArmor = Instantiate(equippedArmor.prefab);
            droppedArmor.transform.position = thrownStartingPoint.transform.position;
            droppedArmor.transform.parent = null;
            equippedArmor = scriptableObject as Armor;
        } else
        {
            items.Add(scriptableObject as Item);
            playerCanvasController.UpdateInventoryPanel();
        }

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

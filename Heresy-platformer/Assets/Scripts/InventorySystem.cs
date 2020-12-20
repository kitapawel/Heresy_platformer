using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    //TODO determine which type of weapon can go to main weapon slot
    //and which weapon as secondary based on character type or class
    public Weapon defaultWeapon;
    public Armor defaultArmor;

    public Weapon equippedWeapon;
    public Armor equippedArmor;
    public int projectileCount;
    public int baseInventorySpace = 3;
    public int inventorySpace;
    public PlayerCanvasController playerCanvasController;

    public ProjectileRotating equippedThrownWeapon;

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
            if (items.Count <= inventorySpace)
            {
                items.Add(scriptableObject as Item);
                playerCanvasController.UpdateInventoryPanel();
            }
        }

    }    

    public void DropItem(Item item)
    {
        GameObject droppedItem = Instantiate(item.prefab);
        droppedItem.transform.position = thrownStartingPoint.transform.position;
        droppedItem.transform.parent = null;
        items.Remove(item);
    }

    public bool isInventoryFull()
    {
        if (inventorySpace <= items.Count)
        {
            return true;
        } else
        {
            return false;
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

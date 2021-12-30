﻿using System.Collections;
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
    public Tool equippedTool;
    public Accessory equippedAccessory;
    public int projectileCount;
    public int baseInventorySlots = 2;
    public int inventorySlots;
    public PlayerCanvasController playerCanvasController;

    public ProjectileRotating equippedThrownWeapon;

    public GameObject thrownStartingPoint;

    public List<Item> items = new List<Item>();

    private void Start()
    {
        playerCanvasController = FindObjectOfType<PlayerCanvasController>();
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
        inventorySlots = baseInventorySlots;
    }
    //Initiated from the PickableItem script, flows to the PlayerCanvasController to update inventory UI
    public void PickItemToInventory(ScriptableObject scriptableObject)
    {
        if (items.Count <= GetCurrentInventorySlots())
        {
            items.Add(scriptableObject as Item);
            playerCanvasController.UpdateInventoryPanel();
        }
    }
    //Used after item is clicked in the inventory; InventorySlot does the processing
    public void UseItemInInventory(ScriptableObject scriptableObject)
    {
        if (scriptableObject.GetType() == typeof(Weapon))
        {
            PickItemToInventory(equippedWeapon);
            RemoveItemFromInventory(scriptableObject as Item);
            equippedWeapon = scriptableObject as Weapon;

            //TODO notify CharacterController to switch animations
        }
        else if (scriptableObject.GetType() == typeof(Armor))
        {
            //TODO cannot equip if more items would overflow inventory
            PickItemToInventory(equippedArmor);
            RemoveItemFromInventory(scriptableObject as Item);
            equippedArmor = scriptableObject as Armor;
        }
        else if (scriptableObject.GetType() == typeof(Tool))
        {
            //TODO cannot equip if more items would overflow inventory
            if (equippedTool != null)
            {
                PickItemToInventory(equippedTool);
            }
            RemoveItemFromInventory(scriptableObject as Item);
            equippedTool = scriptableObject as Tool;
        }
        else if (scriptableObject.GetType() == typeof(Accessory))
        {
            //TODO cannot equip if more items would overflow inventory
            if (equippedAccessory != null)
            {
                PickItemToInventory(equippedAccessory);
            }
            RemoveItemFromInventory(scriptableObject as Item);
            equippedAccessory = scriptableObject as Accessory;
        }
        else
        {
            if (items.Count <= GetCurrentInventorySlots())
            {
                items.Add(scriptableObject as Item);
                playerCanvasController.UpdateInventoryPanel();
            }
        }
    }

    public void RemoveItemFromInventory(Item item)//item is destroyed
    {
        items.Remove(item);
    }

    public void DropItem(Item item)//item is dropped to the ground
    {
        GameObject droppedItem = Instantiate(item.prefab);
        droppedItem.transform.position = thrownStartingPoint.transform.position;
        droppedItem.transform.parent = null;
        items.Remove(item);
    }

    public bool IsInventoryFull()
    {
        if (GetCurrentInventorySlots() <= items.Count)
        {
            return true;
        } else
        {
            return false;
        }
    }
    public bool IsInventoryOverflowing()
    {
        if (GetCurrentInventorySlots() < items.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public float GetCurrentInventorySlots()
    {
        int totalInventorySlots = inventorySlots + equippedArmor.inventorySlots;
        if (equippedAccessory)
        {
            totalInventorySlots += equippedAccessory.inventorySlots;
        }
        return totalInventorySlots;
    }

    public float GetDefenseValue()
    {
        float defenseBaseValue;
        if (equippedArmor.armorType == ArmorType.Light)
        {
            defenseBaseValue = Mathf.Round(Random.Range(0f, 2f));                
        } else if (equippedArmor.armorType == ArmorType.Medium)
        {
            defenseBaseValue = Mathf.Round(Random.Range(2f, 4f));
        } else if (equippedArmor.armorType == ArmorType.Heavy)
        {
            defenseBaseValue = Mathf.Round(Random.Range(3f, 6f));
        } else
        {
            defenseBaseValue = 0f;
        }
        float defenseFinalValue = defenseBaseValue + equippedArmor.defense;
        return defenseFinalValue;
    }
    public float GetPoiseValue()
    {
        float poise = equippedArmor.poise;
        return poise;
    }

}

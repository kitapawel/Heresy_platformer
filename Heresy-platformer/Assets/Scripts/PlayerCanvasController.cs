using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasController : MonoBehaviour
{
    public Image healthBar;
    HealthSystem myHealthSystem;
    InventorySystem myInventorySystem;
    
    public Transform inventoryWindow;
    bool isInventoryWindowActive = false;

    public InventorySlot inventorySlotPrefab;

    private void Start()
    {
        healthBar = GameObject.Find("PlayerHealth").GetComponent<Image>();
        myHealthSystem = GetComponentInParent<HealthSystem>();
        myInventorySystem = GetComponentInParent<InventorySystem>();
        inventoryWindow.gameObject.SetActive(false);
    }

    private void Update()
    {
        healthBar.fillAmount = myHealthSystem.GetHealthAsPercentage();
        ShowUIElements();
    }

    public void UpdateInventoryPanel()
    {
        //clear inventory window to not duplicate icons
        foreach (Transform child in inventoryWindow)
        {
            Destroy(child.gameObject);
        }
        //
        foreach (Item item in myInventorySystem.items)
        {
            InventorySlot inventorySlot = Instantiate(inventorySlotPrefab, inventoryWindow);
            inventorySlot.item = item;
            inventorySlot.icon.sprite = item.icon;
        }
    }

    public void ShowUIElements()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isInventoryWindowActive == false)
            {
                isInventoryWindowActive = true;
                inventoryWindow.gameObject.SetActive(true);
            }
            else
            {
                isInventoryWindowActive = false;
                inventoryWindow.gameObject.SetActive(false);
            }
        }
    }

}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCanvasController : MonoBehaviour
{
    HealthSystem myHealthSystem;
    InventorySystem myInventorySystem;
    CharacterController myCharacterController;

    public Image healthBar;
    public Image energyBar;
    public Image vitalityBar;
    public Transform inventoryWindow;
    public Transform inventoryBag;
    public EquippedItemSlot equippedArmor;
    public EquippedItemSlot equippedWeapon;
    public EquippedItemSlot equippedTool;
    public EquippedItemSlot equippedAccessory;

    bool isInventoryWindowActive = false;

    public InventorySlot inventorySlotPrefab;
    public TMP_Text inventorySlotTrackerText;
    public TMP_Text equippedStats;
    public TMP_Dropdown primaryAttackDropDown;
    public TMP_Dropdown secondaryAttackDropDown;
    public TMP_Dropdown comboAttackDropDown;


    private void Start()
    {
        //first three lines automatically find images provided that gameobject names are correct
        healthBar = GameObject.Find("PlayerHealth").GetComponent<Image>();
        energyBar = GameObject.Find("PlayerEnergy").GetComponent<Image>();
        vitalityBar = GameObject.Find("PlayerVitality").GetComponent<Image>();
        myHealthSystem = FindObjectOfType<PlayerInput>().GetComponent<HealthSystem>();
        myInventorySystem = FindObjectOfType<PlayerInput>().GetComponentInParent<InventorySystem>();
        myCharacterController = FindObjectOfType<PlayerInput>().GetComponentInParent<CharacterController>();
        inventoryWindow.gameObject.SetActive(false);
        UpdateInventoryPanel();
    }

    private void Update()
    {
        healthBar.fillAmount = myHealthSystem.GetHealthAsPercentage();
        energyBar.fillAmount = myHealthSystem.GetEnergyAsPercentage();
        vitalityBar.fillAmount = myHealthSystem.GetVitalityAsPercentage();
        ShowUIElements();
    }

    public void UpdateInventoryPanel()
    {
        //clear inventory window to not duplicate icons
        foreach (Transform child in inventoryBag)
        {
            Destroy(child.gameObject);
        }
        foreach (Item item in myInventorySystem.items)
        {
            InventorySlot inventorySlot = Instantiate(inventorySlotPrefab, inventoryBag);
            inventorySlot.item = item;
            inventorySlot.icon.sprite = item.icon;
        }

        equippedArmor.item = myInventorySystem.equippedArmor;
        equippedArmor.icon.sprite = myInventorySystem.equippedArmor.icon;
        equippedWeapon.item = myInventorySystem.equippedWeapon;
        equippedWeapon.icon.sprite = myInventorySystem.equippedWeapon.icon;
        if (myInventorySystem.equippedTool)
        {
            equippedTool.item = myInventorySystem.equippedTool;
            equippedTool.icon.sprite = myInventorySystem.equippedTool.icon;
        }
        if (myInventorySystem.equippedAccessory)
        {
            equippedAccessory.item = myInventorySystem.equippedAccessory;
            equippedAccessory.icon.sprite = myInventorySystem.equippedAccessory.icon;
        }
        UpdateEquipmentText();
        UpdateBagText();

    }

    private void UpdateEquipmentText()
    {
        string newString = string.Format("Damage: " + myInventorySystem.equippedWeapon.damage + "\n" +
                                          "Defense: " + myInventorySystem.equippedArmor.defense + "\n");
        equippedStats.text = newString;
    }

    private void UpdateBagText()
    {
        string newString = (myInventorySystem.items.Count + "/" + myInventorySystem.GetCurrentInventorySlots());
        inventorySlotTrackerText.text = newString;
    }

    public void ShowUIElements()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleInventoryWindow();
        }
    }

    public void ToggleInventoryWindow()
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

    public void SelectAttack()
    {
        float primValue = primaryAttackDropDown.value;        
        float secValue = secondaryAttackDropDown.value;
        myCharacterController.primaryAttackLevel = primValue;
        myCharacterController.secondaryAttackLevel = secValue;
        /*if (primValue == 0f || primValue == 1f || primValue == 2f || primValue == 3f || primValue == 4f)
        {
            myCharacterStats.primaryAttackLevel = primValue;
        } else
        {
            myCharacterStats.primaryAttackLevel = 0;
        }
        if (secValue == 0f || secValue == 1f || secValue == 2f || secValue == 3f || secValue == 4f)
        {
            myCharacterStats.secondaryAttackLevel = secValue;
        }
        else
        {
            myCharacterStats.secondaryAttackLevel = 0;
        }       */
    }
}
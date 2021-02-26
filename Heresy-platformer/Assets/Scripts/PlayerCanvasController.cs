using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCanvasController : MonoBehaviour
{
    HealthSystem myHealthSystem;
    InventorySystem myInventorySystem;
    CharacterStats myCharacterStats;

    public Image healthBar;
    public Image energyBar;
    public Image vitalityBar;
    public Transform inventoryWindow;

    bool isInventoryWindowActive = false;

    public InventorySlot inventorySlotPrefab;
    public TMP_Dropdown primaryAttackDropDown;
    public TMP_Dropdown secondaryAttackDropDown;
    public TMP_Dropdown comboAttackDropDown;


    private void Start()
    {
        healthBar = GameObject.Find("PlayerHealth").GetComponent<Image>();
        energyBar = GameObject.Find("PlayerEnergy").GetComponent<Image>();
        vitalityBar = GameObject.Find("PlayerVitality").GetComponent<Image>();
        myHealthSystem = FindObjectOfType<PlayerInput>().GetComponent<HealthSystem>();
        myInventorySystem = FindObjectOfType<PlayerInput>().GetComponentInParent<InventorySystem>();
        myCharacterStats = FindObjectOfType<PlayerInput>().GetComponentInParent<CharacterStats>();
        inventoryWindow.gameObject.SetActive(false);


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

    public void SelectAttack()
    {
        float primValue = primaryAttackDropDown.value;
        myCharacterStats.primaryAttackLevel = primValue;
        float secValue = secondaryAttackDropDown.value;
        myCharacterStats.secondaryAttackLevel = secValue;
/*        if (dropDownValue == 0)
        {
            myCharacterStats.primaryAttackLevel = 0;
        } else if (dropDownValue == 1)
        {
            myCharacterStats.primaryAttackLevel = 1;
        } else if (dropDownValue == 2)
        {
            myCharacterStats.primaryAttackLevel = 2;
        }*/
    }
}
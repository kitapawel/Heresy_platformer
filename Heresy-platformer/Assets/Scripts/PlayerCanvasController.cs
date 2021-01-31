using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasController : MonoBehaviour
{
    public Image healthBar;
    public Image energyBar;
    public Image vitalityBar;
    HealthSystem myHealthSystem;
    InventorySystem myInventorySystem;
    
    public Transform inventoryWindow;
    bool isInventoryWindowActive = false;

    public InventorySlot inventorySlotPrefab;

    private void Start()
    {
        healthBar = GameObject.Find("PlayerHealth").GetComponent<Image>();
        energyBar = GameObject.Find("PlayerEnergy").GetComponent<Image>();
        vitalityBar = GameObject.Find("PlayerVitality").GetComponent<Image>();
        myHealthSystem = FindObjectOfType<PlayerInput>().GetComponent<HealthSystem>();
        myInventorySystem = FindObjectOfType<PlayerInput>().GetComponentInParent<InventorySystem>();
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

}
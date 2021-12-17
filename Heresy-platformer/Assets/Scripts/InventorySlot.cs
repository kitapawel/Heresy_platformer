using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouse_over = false;
    public Image icon;
	public Button removeButton;

	public Item item;  // Current item in the slot

    void Update()
    {
        if (mouse_over)
        {
            string tooltipText = GetComponent<InventorySlot>().item.name;
            FindObjectOfType<TooltipController>().ShowToolTip(tooltipText);

        }
    }
    public void RemoveItemFromInventory()
    {
        FindObjectOfType<PlayerInput>().GetComponent<InventorySystem>().DropItem(item);
        FindObjectOfType<PlayerCanvasController>().UpdateInventoryPanel();
    }
    public void UseClickedItem()
    {
        FindObjectOfType<PlayerInput>().GetComponent<InventorySystem>().UseItemInInventory(item);
        FindObjectOfType<PlayerCanvasController>().UpdateInventoryPanel();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        Debug.Log("Mouse enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        Debug.Log("Mouse exit");
    }

    /*    // Add item to the slot
        public void AddItem(Item newItem)
        {
            item = newItem;

            icon.sprite = item.icon;
            icon.enabled = true;
            removeButton.interactable = true;
        }

        // Clear the slot
        public void ClearSlot()
        {
            item = null;

            icon.sprite = null;
            icon.enabled = false;
            removeButton.interactable = false;
        }


        // Use the item
        public void UseItem()
        {

        }*/
}

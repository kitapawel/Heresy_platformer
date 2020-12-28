
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	public Image icon;
	public Button removeButton;

	public Item item;  // Current item in the slot

    private void Start()
    {
        
    }
    public void RemoveItemFromInventory()
    {
        FindObjectOfType<PlayerInput>().GetComponent<InventorySystem>().DropItem(item);
        FindObjectOfType<PlayerCanvasController>().UpdateInventoryPanel();

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

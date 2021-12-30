using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquippedItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private bool mouse_over = false;

	public Image icon;
	public Item item;

    void Update()
    {
        if (mouse_over)
        {
            string tooltipText = item.name;
            FindObjectOfType<TooltipController>().ShowToolTip(tooltipText);

        }
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

}

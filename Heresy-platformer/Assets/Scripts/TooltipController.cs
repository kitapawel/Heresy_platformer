using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipController : MonoBehaviour
{
    public TMP_Text tooltip;
    public Vector3 offset;

    void Start()
    {
        tooltip = GetComponent<TextMeshProUGUI>();
        tooltip.text = "";
    }

    public void ShowToolTip(string txt) //tooltip for objects in "Inspect" mode
    {
        if (tooltip != null && FindObjectOfType<PlayerInput>().GetComponent<CharacterController>().isInspecting)
        {
            tooltip.text = txt;
        }
    }
    public void ShowNotification(string txt) //notifications for actions, e.g. if some action fails, not related to "Inspect" mode
    {
        if (tooltip != null)
        {
            tooltip.text = txt;            
        }
    }

}


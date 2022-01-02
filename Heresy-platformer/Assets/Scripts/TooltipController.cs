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

    public void ShowToolTip(string txt)
    {
        if (tooltip != null && FindObjectOfType<PlayerInput>().GetComponent<CharacterController>().isInspecting)
        {
            tooltip.text = txt;
        }
    }
}


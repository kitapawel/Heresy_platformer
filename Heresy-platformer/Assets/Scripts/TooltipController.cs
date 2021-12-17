using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipController : MonoBehaviour
{
    public TMP_Text tooltip;

    // Start is called before the first frame update
    void Start()
    {
        tooltip = GetComponent<TextMeshProUGUI>();
        tooltip.text = "";
    }

    public void ShowToolTip(string txt)
    {
        if (tooltip != null)
        {
            tooltip.text = txt;
        }
    }
}


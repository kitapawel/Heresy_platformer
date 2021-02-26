using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public string prefabName;
    public Sprite icon = null;
    public GameObject prefab;

    public string flavourText;

    public virtual void UseItem()
    {
        Debug.Log("Used: " + prefabName);
    }


}

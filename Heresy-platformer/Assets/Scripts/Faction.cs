using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Faction
{
    public FactionType factionType;

    public Faction(FactionType factionType)
    {
        this.factionType = factionType;
        Debug.Log("Created faction " + this.factionType);
    }

}

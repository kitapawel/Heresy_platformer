using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoBehaviour //singleton pattern
{
    private static FactionManager _instance;
    public static FactionManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);//this and not gameObject to remove only script and not the gameobject the script is located on
        }
        else
        {
            _instance = this;
        }
    }

    [SerializeField]
    public Faction expedition;
    public Faction army;
    public Faction bandits;

    void Start()
    {
        InitializeFactions();
    }

    void Update()
    {

    }

    void InitializeFactions()
    {
        expedition = new Faction(FactionType.Expedition);
        army = new Faction(FactionType.Army);
        bandits = new Faction(FactionType.Bandits);
    }

    public bool IsEnemyHostile(GameObject current, GameObject target)
    {
        if (current.GetComponent<CharacterStats>() && target.GetComponent<CharacterStats>()) {
            if (current.GetComponent<CharacterStats>().factionType == target.GetComponent<CharacterStats>().factionType)
            {
                return false;
            }          
        }
        return true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCResourceBars : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject energyBar;
    HealthSystem myHealthSystem;
    // Start is called before the first frame update
    void Start()
    {
        myHealthSystem = GetComponentInParent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBars();
    }

    void UpdateBars()
    {
        healthBar.transform.localScale = new Vector3 (myHealthSystem.GetHealthAsPercentage(), 1f, 1f);
        energyBar.transform.localScale = new Vector3(Mathf.Clamp(myHealthSystem.GetEnergyAsPercentage(), 0f, 1f), 1f, 1f);
    }
}

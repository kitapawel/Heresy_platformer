using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPerception : MonoBehaviour
{
    bool isTargetInRage;

    public bool IsTargetInRange()
    {
        if (isTargetInRage)
        {
            return true;
        } else
        {
            return false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //TODO - check for faction alignment
        if (collision.gameObject.GetComponent<HealthSystem>())
        {
            isTargetInRage = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<HealthSystem>())
        {
            isTargetInRage = false;
        }
    }
}

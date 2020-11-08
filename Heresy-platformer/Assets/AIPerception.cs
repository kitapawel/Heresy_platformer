using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPerception : MonoBehaviour
{
    bool isPlayerInRange;

    public bool IsPlayerInRange()
    {
        if (isPlayerInRange)
        {
            return true;
        } else
        {
            return false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = false;
        }
    }
}

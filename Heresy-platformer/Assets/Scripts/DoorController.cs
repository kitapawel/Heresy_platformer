using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    [SerializeField] Transform thisDoorLeadsTo;

    bool isPlayerInRange;
    public KeyCode interactionKey = KeyCode.F;
    public UnityEvent interactAction;

    private bool isClosed = true;

    private void Update()
    {
        if (isPlayerInRange)
        {
            if (Input.GetKeyDown(interactionKey))
            {
                interactAction.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
    public void GoThrough()
    {
        FindObjectOfType<PlayerInput>().transform.position = thisDoorLeadsTo.position;
    }
}

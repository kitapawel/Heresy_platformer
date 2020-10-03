using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour
{

    [SerializeField] Sprite openState;
    [SerializeField] Sprite closedState;
    SpriteRenderer mySpriteRenderer;

    public bool isPlayerInRange;
    public KeyCode interactionKey;
    public UnityEvent interactAction;

    private bool isClosed = true;

    private void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

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
        if(collision.gameObject.tag == "Player")
        {
            isPlayerInRange = true;
            Debug.Log("Player is in range.");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isPlayerInRange = false;
            Debug.Log("Player is no longer in range.");
        }
    }
    public void ToggleChest()
    {
        if (isClosed)
        {
            isClosed = false;
            mySpriteRenderer.sprite = openState;
        } else
        {
            isClosed = true;
            mySpriteRenderer.sprite = closedState;
        }
    }

}

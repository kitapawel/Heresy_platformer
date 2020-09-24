using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    SpriteRenderer mySpriteRenderer;

    [SerializeField] Sprite openState;
    [SerializeField] Sprite closedState;
    [SerializeField] GameObject intactObject;
    [SerializeField] GameObject destroyedObjectParts;

    private bool isClosed = true;

    private void Start()
    {
        mySpriteRenderer = intactObject.GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
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

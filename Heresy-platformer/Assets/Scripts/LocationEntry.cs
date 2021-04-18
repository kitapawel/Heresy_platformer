using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEntry : MonoBehaviour
{
    SpriteRenderer mySpriteRenderer;
    public Sprite closedState;
    public Sprite openState;
    public bool isClosed = true;

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        if (isClosed)
        {
            mySpriteRenderer.sprite = closedState;
        } else
        {
            mySpriteRenderer.sprite = openState;
        }
    }
}

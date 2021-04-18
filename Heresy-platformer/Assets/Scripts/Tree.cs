using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    SpriteRenderer mySpriteRenderer;
    SoundSystemForInanimateObjects mySoundSystem;

    Rigidbody2D myRigidbody2D;

    [SerializeField]
    private float maxStructurePoints = 50;
    [SerializeField]
    private float structurePoints = 50;


    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        mySoundSystem = GetComponent<SoundSystemForInanimateObjects>();
    }

    private void CheckStructureState()
    {
        if (structurePoints <= 0)
        {
            DestroyObject();
        }
    }

    public void ProcessIncomingHit(float incomingDamage)
    {
        CheckStructureState();

        TakeDamage(incomingDamage);
    }
    private void TakeDamage(float incomingDamage)
    {
        mySoundSystem.PlayOnHitSounds();
        structurePoints -= incomingDamage;
        CheckStructureState();
    }

    private void DestroyObject()
    {
        myRigidbody2D.isKinematic = false;
    }

}

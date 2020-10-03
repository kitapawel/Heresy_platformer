using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureSystem : MonoBehaviour
{
    SpriteRenderer mySpriteRenderer;
    SoundSystemForInanimateObjects mySoundSystem;

    [SerializeField] GameObject intactObject;
    [SerializeField] GameObject destroyedObjectParts;

    [SerializeField]
    private float maxStructurePoints = 50;
    [SerializeField]
    private float structurePoints = 50;


    void Start()
    {
        mySpriteRenderer = intactObject.GetComponent<SpriteRenderer>();
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

    private void DestroyObject() //TODO remove this scropt from parent object once it is destroyed?
    {
        intactObject.SetActive(false);
        destroyedObjectParts.SetActive(true);
        //destroyedObjectParts.transform.parent = null;
    }
}

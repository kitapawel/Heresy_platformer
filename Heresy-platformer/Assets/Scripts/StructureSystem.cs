using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundSystemForInanimateObjects))]
public class StructureSystem : MonoBehaviour
{
    public StructuralType structuralType;

    Rigidbody2D myRigidbody2D;
    SpriteRenderer mySpriteRenderer;
    SoundSystemForInanimateObjects mySoundSystem;

    [SerializeField] GameObject intactObject; // used when intactobject is replaced by destroyed objects
    [SerializeField] GameObject destroyedObjectParts; // parent object under which child objects for destroyed parts should be put

    [SerializeField]
    private float maxStructurePoints = 50;
    [SerializeField]
    private float structurePoints = 50;
    [SerializeField]
    private float rigidity = 0;


    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        mySoundSystem = GetComponent<SoundSystemForInanimateObjects>();

        if (structuralType == StructuralType.Container)
        {
            mySpriteRenderer = intactObject.GetComponent<SpriteRenderer>();
        } else if (structuralType == StructuralType.Tree)
        {
            myRigidbody2D.isKinematic = true;
        }        
    }

    private void CheckStructureState()
    {
        if (structurePoints <= 0)
        {
            DestroyObject();
        }
    }

    public void ProcessIncomingHit(float incomingDamage, float impact)
    {
        CheckStructureState();
        float damageReduction = incomingDamage - rigidity;
        if (damageReduction < 0)
        {
            damageReduction = 0;
        }
        TakeDamage(damageReduction + impact);
    }
    private void TakeDamage(float incomingDamage)
    {
        mySoundSystem.PlayOnHitSounds();
        structurePoints -= incomingDamage;
        CheckStructureState();
    }

    private void DestroyObject() //TODO remove this scropt from parent object once it is destroyed?
    {
        mySoundSystem.PlayOnDestroySounds();
        if (structuralType == StructuralType.Container)
        {
            intactObject.SetActive(false);
            destroyedObjectParts.SetActive(true);
        }
        else if (structuralType == StructuralType.Tree)
        {
            myRigidbody2D.isKinematic = false;
        }
        else if (structuralType == StructuralType.Building)
        {

        }
        else if (structuralType == StructuralType.Rock)
        {
 
        }


        //destroyedObjectParts.transform.parent = null;
    }
}

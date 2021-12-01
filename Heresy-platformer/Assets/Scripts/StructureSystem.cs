using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SoundSystemForInanimateObjects))]
public class StructureSystem : MonoBehaviour
{
    public StructuralType structuralType;

    Rigidbody2D myRigidbody2D;
    SpriteRenderer mySpriteRenderer;
    SoundSystemForInanimateObjects mySoundSystem;

    [SerializeField] GameObject intactObject; // used when intactobject is replaced by destroyed objects
    [SerializeField] GameObject destroyedObjectParts; // parent object under which child objects for destroyed parts should be put

    [SerializeField] Sprite intactSprite; // used when sprite is replaced with another sprite on destroy
    [SerializeField] Sprite destroyedSprite;

    [SerializeField]
    private float maxStructurePoints = 2;
    [SerializeField]
    private float structurePoints = 2;
    [SerializeField]
    private float hardness = 5;


    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        mySoundSystem = GetComponentInParent<SoundSystemForInanimateObjects>(); // needs a soundsystemforinanimateobjects on gameobject or parent
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        if (mySoundSystem == null)
        {
            Debug.LogError("No soundsystem for " + this);
        }

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

    public void ProcessIncomingHit(float incomingDamage)
    {
        CheckStructureState();
        float damageReduction = Mathf.Round(Random.Range(0f, hardness)); ;
        float damageToDeal = (incomingDamage - damageReduction);
        if (damageToDeal < 0f)
        {
            damageToDeal = 0f;
        }
        TakeDamage(damageToDeal);
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
            //TODO add gathering component >> enables resource gathering OR make gathering part of structure system
            //TODO remove this structure system
        }
        else if (structuralType == StructuralType.DoorToAnotherLocation)
        {
            GetComponentInParent<MapLocation>().OpenDoor();
        }
        else if (structuralType == StructuralType.Building)
        {

        }
        else if (structuralType == StructuralType.Rock || structuralType == StructuralType.Door)
        {
            mySpriteRenderer.sprite = destroyedSprite;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }


        //destroyedObjectParts.transform.parent = null;
    }
}

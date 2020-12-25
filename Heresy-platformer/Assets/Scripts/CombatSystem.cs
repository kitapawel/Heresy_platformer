using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class CombatSystem : MonoBehaviour
{
    Animator myAnimator;
    CharacterController myCharacterController;
    CharacterStats myCharacterStats;
    InventorySystem myInventorySystem;


    HitCollisionChecker hitCollisionChecker;
    [SerializeField] private float damageBonus;
    [SerializeField] private float critRate;
    [SerializeField] private float critDamage;

    public GameObject thrownStartingPoint;


    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCharacterController = GetComponent<CharacterController>();
        hitCollisionChecker = GetComponentInChildren<HitCollisionChecker>();
        myCharacterStats = GetComponent<CharacterStats>();
        myInventorySystem = GetComponent<InventorySystem>();

        UpdateCharacterStats();
    }

    void Update()
    {
        
    }
    private void UpdateCharacterStats()
    {
        damageBonus = myCharacterStats.currentDamageBonus;
        critRate = myCharacterStats.currentCritRate;
        critDamage = myCharacterStats.currentCritBonus;
    }

    public void DealDamage()
    {
        foreach (GameObject hitTarget in hitCollisionChecker.hitTargets)
        {
            GameObject attackerObject = transform.gameObject;//get information about the attacking object and pass to the damaged object
            float damageToDeal = myInventorySystem.equippedWeapon.damage + damageBonus;
            float stabilityDamageToDeal = myInventorySystem.equippedWeapon.stabilityDamage; //TODO maybe spice this up a bit
            float appliedForce = myInventorySystem.equippedWeapon.force; //TODO randomize this and maybe tie this somehow to stabilitydamage
            float attackVector = myCharacterController.GetSpriteDirection();
            if (hitTarget.GetComponentInParent<HealthSystem>())
            {
                Debug.Log(hitTarget + " organic target was hit for " + damageToDeal + " dmg + " +stabilityDamageToDeal + " stability damage.");
                hitTarget.GetComponentInParent<HealthSystem>().ProcessIncomingHit(damageToDeal, stabilityDamageToDeal, appliedForce, attackVector, attackerObject);
            } 
            if (hitTarget.GetComponentInParent<StructureSystem>())
            {
                Debug.Log(hitTarget + " structural target was hit for " + damageToDeal + " dmg.");
                hitTarget.GetComponentInParent<StructureSystem>().ProcessIncomingHit(damageToDeal);
            }
        }
    }

    private float CalculateCriticalDamage(float damage)
    {
        float critChance = Mathf.Clamp(critRate + myInventorySystem.equippedWeapon.critRateBonus, 0, 1f);
        if (Random.Range(0f, 1f) < critChance)
        {
            float bonusCriticalDamage = critDamage + myInventorySystem.equippedWeapon.critDamageBonus;
            damage = damage * bonusCriticalDamage;
            Debug.Log("Critical hit: " + damage);
            return Mathf.Round(damage);//TODO decide how to do rounding and at which point to round
        } else
        {
            Debug.Log("Normal hit: " + damage);
            return Mathf.Round(damage);
        }
    }

    public void ThrowItem()
    {
        if (myInventorySystem.equippedThrownWeapon != null)
        {
            //TODO use mousetoscreenposition to determine vertical force of throw
            ProjectileRotating thrownW = Instantiate(myInventorySystem.equippedThrownWeapon, thrownStartingPoint.transform.position, thrownStartingPoint.transform.rotation);
            thrownW.throwingEntity = transform.gameObject;
            thrownW.GetComponent<Rigidbody2D>().AddForce(new Vector2(20f * myCharacterController.GetSpriteDirection(), 5f), ForceMode2D.Impulse);
        }        
    }
    
}

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
    InteractionChecker interactionChecker;
    public float damageBonus;
    public float critRate;
    public float critDamage;

    const int WEAK_ATTACK = 1;
    float weakAttackMultiplier = 0.8f;
    const int NORMAL_ATTACK = 0;
    float normalAttackMultiplier = 1f;
    const int STRONG_ATTACK = 2;
    float strongAttackMultiplier = 1.2f;

    public GameObject thrownStartingPoint;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCharacterController = GetComponent<CharacterController>();
        hitCollisionChecker = GetComponentInChildren<HitCollisionChecker>();
        interactionChecker = GetComponentInChildren<InteractionChecker>();
        myCharacterStats = GetComponent<CharacterStats>();
        myInventorySystem = GetComponent<InventorySystem>();

        UpdateCharacterStats();
    }

    private void UpdateCharacterStats()
    {
        damageBonus = myCharacterStats.currentDamageBonus;
        critRate = myCharacterStats.currentCritRate;
        critDamage = myCharacterStats.currentCritBonus;
    }

    public void DealDamage(int attackMode)
    {
        foreach (GameObject hitTarget in hitCollisionChecker.hitTargets)
        {
            //Assign basic values to damage calculation
            GameObject attackerObject = transform.gameObject;//get information about the attacking object and pass to the damaged object
            float damageToDeal = myInventorySystem.equippedWeapon.damage + damageBonus;
            float stabilityDamageToDeal = myInventorySystem.equippedWeapon.stabilityDamage; //TODO maybe spice this up a bit
            float appliedForce = myInventorySystem.equippedWeapon.force; //TODO randomize this and maybe tie this somehow to stabilitydamage
            float attackVector = myCharacterController.GetSpriteDirection();

            //Modify damage value depending on whether the attack was strong or weak
            if (attackMode == 1)
            {
                damageToDeal = damageToDeal * weakAttackMultiplier;
                stabilityDamageToDeal = stabilityDamageToDeal * weakAttackMultiplier;
                appliedForce = appliedForce * weakAttackMultiplier;
            } else if (attackMode == 2)
            {
                damageToDeal = damageToDeal * strongAttackMultiplier;
                stabilityDamageToDeal = stabilityDamageToDeal * strongAttackMultiplier;
                appliedForce = appliedForce * strongAttackMultiplier;
            } else
            {
                damageToDeal = damageToDeal * normalAttackMultiplier;
                stabilityDamageToDeal = stabilityDamageToDeal * normalAttackMultiplier;
                appliedForce = appliedForce * normalAttackMultiplier;
            }

            //Send data to target
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
    public void FinishOff()
    {
        foreach (GameObject finishOffTarget in interactionChecker.finishOffTargets)
        {            
            float damageToDeal = 5f;
            float stabilityDamageToDeal = myInventorySystem.equippedWeapon.stabilityDamage; //TODO maybe spice this up a bit
            float appliedForce = myInventorySystem.equippedWeapon.force; //TODO randomize this and maybe tie this somehow to stabilitydamage
            float attackVector = myCharacterController.GetSpriteDirection();
            if (finishOffTarget.GetComponentInParent<HealthSystem>())
            {
                Debug.Log(finishOffTarget + " organic target was hit for " + damageToDeal + " dmg + " + stabilityDamageToDeal + " stability damage.");
                finishOffTarget.GetComponentInParent<HealthSystem>().ProcessIncomingHit(damageToDeal, stabilityDamageToDeal, appliedForce, attackVector);
            }
        }
    }

}

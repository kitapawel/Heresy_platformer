using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] CharacterStats characterStats;
    public FactionType factionType;

    CharacterController myCharacterController;
    InventorySystem myInventorySystem;

    HitCollisionChecker hitCollisionChecker;
    InteractionChecker interactionChecker;
    public float damageBonus;
    public float critRate;
    public float critDamageBonus;
    public float weakAttackMultiplier;
    public float normalAttackMultiplier;
    public float strongAttackMultiplier;

    const int WEAK_ATTACK = 1;
    const int NORMAL_ATTACK = 0;
    const int STRONG_ATTACK = 2;

    public GameObject thrownStartingPoint;

    void Awake()
    {
        myCharacterController = GetComponent<CharacterController>();
        hitCollisionChecker = GetComponentInChildren<HitCollisionChecker>();
        interactionChecker = GetComponentInChildren<InteractionChecker>();
        myInventorySystem = GetComponent<InventorySystem>();

        InitializeCharacterStats();
    }

    private void InitializeCharacterStats()
    {
        damageBonus = characterStats.baseDamageBonus;
        critRate = characterStats.baseCritRate;
        critDamageBonus = characterStats.baseCritBonus;
        weakAttackMultiplier = characterStats.weakAttackMultiplier;
        normalAttackMultiplier = characterStats.normalAttackMultiplier;
        strongAttackMultiplier = characterStats.strongAttackMultiplier;

        factionType = characterStats.factionType;
    }

    public void DealDamage(int attackMode)
    {
        foreach (GameObject hitTarget in hitCollisionChecker.hitTargets)
        {
            //Assign basic values to damage calculation
            GameObject attackerObject = transform.gameObject;//get information about the attacking object and pass to the damaged object
            float damageToDeal = myInventorySystem.equippedWeapon.damage + damageBonus;
            float armorPenetration = myInventorySystem.equippedWeapon.armorPenetration;
            float stabilityDamageToDeal = myInventorySystem.equippedWeapon.stabilityDamage; //TODO maybe spice this up a bit
            float appliedForce = myInventorySystem.equippedWeapon.force; //TODO randomize this and maybe tie this somehow to stabilitydamage
            float attackVector = myCharacterController.GetSpriteDirection();
            float structuralDamageToDeal = myInventorySystem.equippedWeapon.structuralDamage;

            //Modify damage value depending on whether the attack was strong or weak
            if (attackMode == WEAK_ATTACK)
            {
                damageToDeal = damageToDeal * weakAttackMultiplier;
                stabilityDamageToDeal = stabilityDamageToDeal * weakAttackMultiplier;
                appliedForce = appliedForce * weakAttackMultiplier;
            } else if (attackMode == STRONG_ATTACK)
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
            damageToDeal = CalculateCriticalDamage(damageToDeal);
            //Send data to target
            if (hitTarget.GetComponentInParent<HealthSystem>())
            {                
                hitTarget.GetComponentInParent<HealthSystem>().ProcessIncomingHit(damageToDeal, armorPenetration, stabilityDamageToDeal, appliedForce, attackVector, attackerObject);
            } 
            if (hitTarget.GetComponentInParent<StructureSystem>())
            {
                hitTarget.GetComponentInParent<StructureSystem>().ProcessIncomingHit(structuralDamageToDeal);
            }
        }
    }

    public void DealDamageTool (int attackMode) //used to attack with tools (due to the fact that animation events only take 1 parameter)
    {
        foreach (GameObject hitTarget in hitCollisionChecker.hitTargets)
        {
            //Assign basic values to damage calculation
            GameObject attackerObject = transform.gameObject;//get information about the attacking object and pass to the damaged object
            float damageToDeal = myInventorySystem.equippedTool.damage + damageBonus;
            float piercingDamage = myInventorySystem.equippedTool.armorPenetration + damageBonus;
            float stabilityDamageToDeal = myInventorySystem.equippedTool.stabilityDamage; //TODO maybe spice this up a bit
            float appliedForce = myInventorySystem.equippedTool.force; //TODO randomize this and maybe tie this somehow to stabilitydamage
            float attackVector = myCharacterController.GetSpriteDirection();
            float structuralDamageToDeal = myInventorySystem.equippedTool.structuralDamage;

            //Modify damage value depending on whether the attack was strong or weak
            if (attackMode == WEAK_ATTACK)
            {
                damageToDeal = damageToDeal * weakAttackMultiplier;
                stabilityDamageToDeal = stabilityDamageToDeal * weakAttackMultiplier;
                appliedForce = appliedForce * weakAttackMultiplier;
            }
            else if (attackMode == STRONG_ATTACK)
            {
                damageToDeal = damageToDeal * strongAttackMultiplier;
                stabilityDamageToDeal = stabilityDamageToDeal * strongAttackMultiplier;
                appliedForce = appliedForce * strongAttackMultiplier;
            }
            else
            {
                damageToDeal = damageToDeal * normalAttackMultiplier;
                stabilityDamageToDeal = stabilityDamageToDeal * normalAttackMultiplier;
                appliedForce = appliedForce * normalAttackMultiplier;
            }

            damageToDeal = CalculateCriticalDamage(damageToDeal);

            //Send data to target
            if (hitTarget.GetComponentInParent<HealthSystem>())
            {
                Debug.Log(hitTarget + " organic target was hit for " + damageToDeal + " dmg + " + stabilityDamageToDeal + " stability damage.");
                hitTarget.GetComponentInParent<HealthSystem>().ProcessIncomingHit(damageToDeal, piercingDamage, stabilityDamageToDeal, appliedForce, attackVector, attackerObject);
            }
            if (hitTarget.GetComponentInParent<StructureSystem>())
            {
                Debug.Log(hitTarget + " structural target was hit for " + damageToDeal + " dmg.");
                hitTarget.GetComponentInParent<StructureSystem>().ProcessIncomingHit(structuralDamageToDeal);
            }
        }
    }

    private float CalculateCriticalDamage(float damage)
    {
        float critChance = Mathf.Clamp(critRate + myInventorySystem.equippedWeapon.critRateBonus, 0f, .9f);
        float critRoll = Random.Range(0f, 1f);
        if (critRoll < critChance)
        {
            float bonusCriticalDamage = critDamageBonus + myInventorySystem.equippedWeapon.critDamageBonus;
            damage = damage * bonusCriticalDamage;
            Debug.Log("Critical hit roll: " + critRoll + ", dealt critical damage:" + damage);
            return Mathf.Round(damage);//TODO decide how to do rounding and at which point to round
        } else
        {
            Debug.Log("Critical hit roll: " + critRoll + ", dealt normal damage:" + damage);
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

    public FactionType GetCurrentFactionType()
    {
        return factionType;
    }

}

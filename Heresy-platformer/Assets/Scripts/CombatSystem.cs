using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class CombatSystem : MonoBehaviour
{
    [SerializeField]
    private Weapon equippedWeapon;

    Animator myAnimator;
    CharacterController myCharacterController;
    private CharacterStats myCharacterStats;

    private HitCollisionChecker hitCollisionChecker;
    [SerializeField] private float damageBonus;
    [SerializeField] private float critRate;
    [SerializeField] private float critDamage;

    [SerializeField] private float minWeaponDamage;
    [SerializeField] private float maxWeaponDamage;
    [SerializeField] private float minWeaponStabilityDamage;
    [SerializeField] private float maxWeaponStabilityDamage;
    [SerializeField] private float minWeaponForce;
    [SerializeField] private float maxWeaponForce;
    [SerializeField] private float weaponCritRateBonus;
    [SerializeField] private float weaponCritDamageBonus;


    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCharacterController = GetComponent<CharacterController>();
        hitCollisionChecker = GetComponentInChildren<HitCollisionChecker>();
        myCharacterStats = GetComponent<CharacterStats>();

        InitializeStats();
    }

    void Update()
    {
        
    }
    private void InitializeStats()
    {
        //character-based stats
        damageBonus = myCharacterStats.currentDamageBonus;
        critRate = myCharacterStats.currentCritRate;
        critDamage = myCharacterStats.currentCritBonus;

        //weapon-based stats
        minWeaponDamage = equippedWeapon.minimumDamage;
        maxWeaponDamage = equippedWeapon.maximumDamage;        
        minWeaponStabilityDamage = equippedWeapon.minimumStabilityDamage;
        maxWeaponStabilityDamage = equippedWeapon.maximumStabilityDamage;
        minWeaponForce = equippedWeapon.minimumForce;
        maxWeaponForce = equippedWeapon.maximumForce;
        weaponCritRateBonus = equippedWeapon.critRateBonus;
        weaponCritDamageBonus = equippedWeapon.critDamageBonus;

    }

    public void DealDamage()
    {
        foreach (GameObject hitTarget in hitCollisionChecker.hitTargets)
        {
            float damageToDeal = CalculateDamageToDeal(minWeaponDamage+damageBonus, maxWeaponDamage+damageBonus);
            float stabilityDamageToDeal = Random.Range(minWeaponStabilityDamage, maxWeaponStabilityDamage); //TODO maybe spice this up a bit
            float appliedForce = Random.Range(minWeaponForce, maxWeaponForce); //TODO randomize this and maybe tie this somehow to stabilitydamage
            float attackVector = myCharacterController.GetSpriteDirection();
            if (hitTarget.GetComponentInParent<HealthSystem>())
            {
                Debug.Log(hitTarget + " organic target was hit for " + damageToDeal + " dmg + " +stabilityDamageToDeal + " stability damage.");
                hitTarget.GetComponentInParent<HealthSystem>().ProcessIncomingHit(damageToDeal, stabilityDamageToDeal, appliedForce, attackVector);
            } 
            if (hitTarget.GetComponentInParent<StructureSystem>())
            {
                Debug.Log(hitTarget + " structural target was hit for " + damageToDeal + " dmg.");
                hitTarget.GetComponentInParent<StructureSystem>().ProcessIncomingHit(damageToDeal);
            }
        }
    }

    private float CalculateDamageToDeal(float min, float max)
    {
        float damageToDeal = Random.Range(min, max);
        return CalculateCriticalDamage(damageToDeal);
    }
    private float CalculateCriticalDamage(float damage)
    {
        float critChance = Mathf.Clamp(critRate + weaponCritRateBonus, 0, 1f);
        if (Random.Range(0f, 1f) < critChance)
        {
            float bonusCriticalDamage = critDamage + weaponCritDamageBonus;
            damage = damage * bonusCriticalDamage;
            Debug.Log("Critical hit: " + damage);
            return Mathf.Round(damage);//TODO decide how to do rounding and at which point to round
        } else
        {
            Debug.Log("Normal hit: " + damage);
            return Mathf.Round(damage);
        }
    }
    
}

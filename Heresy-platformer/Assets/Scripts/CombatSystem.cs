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

    [SerializeField] private float weaponDamage;
    [SerializeField] private float stabilityDamage;
    [SerializeField] private float force;
    [SerializeField] private float weaponCritRateBonus;
    [SerializeField] private float weaponCritDamageBonus;

    [SerializeField] private ProjectileRotating thrownWeapon;
    [SerializeField] private GameObject thrownStartingPoint;


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
        weaponDamage = equippedWeapon.damage;    
        stabilityDamage = equippedWeapon.stabilityDamage;
        force = equippedWeapon.force;
        weaponCritRateBonus = equippedWeapon.critRateBonus;
        weaponCritDamageBonus = equippedWeapon.critDamageBonus;

    }

    public void DealDamage()
    {
        foreach (GameObject hitTarget in hitCollisionChecker.hitTargets)
        {
            GameObject attackerObject = transform.gameObject;//get information about the attacking object and pass to the damaged object
            float damageToDeal = weaponDamage+damageBonus;
            float stabilityDamageToDeal = stabilityDamage; //TODO maybe spice this up a bit
            float appliedForce = force; //TODO randomize this and maybe tie this somehow to stabilitydamage
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

    public void ThrowItem()
    {
        if (thrownWeapon != null)
        {
            //TODO use mousetoscreenposition to determine vertical force of throw
            ProjectileRotating thrownW = Instantiate(thrownWeapon, thrownStartingPoint.transform.position, thrownStartingPoint.transform.rotation);
            thrownW.throwingEntity = transform.gameObject;
            thrownW.GetComponent<Rigidbody2D>().AddForce(new Vector2(20f * myCharacterController.GetSpriteDirection(), 5f), ForceMode2D.Impulse);
        }        
    }
    
}

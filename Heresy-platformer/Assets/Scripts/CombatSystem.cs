using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    Animator myAnimator;

    private HitCollisionChecker hitCollisionChecker;
    [SerializeField] private float minDamage = 10f;
    [SerializeField] private float maxDamage = 20f;
    [SerializeField] private float minStabilityDamage = 20f;
    [SerializeField] private float maxStabilityDamage = 30f;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        hitCollisionChecker = GetComponentInChildren<HitCollisionChecker>();
    }

    void Update()
    {

    }

    public void DealDamage()
    {
        foreach (GameObject hitTarget in hitCollisionChecker.hitTargets)
        {
            float damageToDeal = CalculateDamageToDeal(minDamage, maxDamage);
            float stabilityDamageToDeal = CalculateDamageToDeal(minStabilityDamage, maxStabilityDamage);
            if (hitTarget.GetComponentInParent<HealthSystem>())
            {
                Debug.Log(hitTarget + " organic target was hit for " + damageToDeal + " dmg + " +stabilityDamageToDeal + " stability damage.");
                hitTarget.GetComponentInParent<HealthSystem>().ProcessIncomingHit(damageToDeal, stabilityDamageToDeal);
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
        return damageToDeal;
    }
}

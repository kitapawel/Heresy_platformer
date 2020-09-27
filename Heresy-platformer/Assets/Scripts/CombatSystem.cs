using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    Animator myAnimator;

    private HitCollisionChecker hitCollisionChecker;
    [SerializeField] private float minDamage = 10f;
    [SerializeField] private float maxDamage = 20f;

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
            float hit = CalculateDamageToDeal();
            Debug.Log(hitTarget+ " hit for " + hit + " dmg");
            hitTarget.GetComponentInParent<HealthSystem>().ProcessIncomingHit(hit);
        }
    }

    private float CalculateDamageToDeal()
    {
        float damageToDeal = Random.Range(minDamage, maxDamage);
        return damageToDeal;
    }
}

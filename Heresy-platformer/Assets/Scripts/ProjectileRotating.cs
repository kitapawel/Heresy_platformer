using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRotating : MonoBehaviour
{
    Rigidbody2D myRigidbody2D;
    bool flyingAtTarget = true;
    public GameObject throwingEntity = null;

    const int THROWN_LAYER = 24;
    const int PICKABLE_LAYER = 25;

    [Header("Weapon Stats")]
    [SerializeField]
    float damage;
    [SerializeField]
    float piercingDamage;
    [SerializeField]
    float stabilitydamage;
    [SerializeField]
    float force;

    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (flyingAtTarget)
        {
            float angle = Mathf.Atan2(myRigidbody2D.velocity.y, myRigidbody2D.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (flyingAtTarget)
        {         
            GameObject target = collision.gameObject;
            DealDamageToTarget(target);
            StopOnHittingTarget();
        }
    }

    void StopOnHittingTarget()
    {
        flyingAtTarget = false;
        throwingEntity = null;
        gameObject.layer = PICKABLE_LAYER;
        //Destroy(gameObject.GetComponent<Rigidbody2D>());
        //Destroy(gameObject.GetComponent<PolygonCollider2D>());
        //Destroy(this.gameObject, 2f);
    }

    void DealDamageToTarget(GameObject target) //TODO pull data from user
    {
        if (target.GetComponent<HealthSystem>())
        {
            float attackVector = 1f * throwingEntity.GetComponent<CharacterController>().GetSpriteDirection();
            HealthSystem targetHealthSystem = target.GetComponent<HealthSystem>();
            targetHealthSystem.ProcessIncomingHit(damage, piercingDamage, force, attackVector);
        }
    }

}

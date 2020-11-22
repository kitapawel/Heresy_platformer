using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRotating : MonoBehaviour
{
    Rigidbody2D myRigidbody2D;
    bool thrown = true;
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (thrown)
        {
            float angle = Mathf.Atan2(myRigidbody2D.velocity.y, myRigidbody2D.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.transform.parent = collision.gameObject.transform;
        StopOnHittingTarget();
        GameObject target = collision.gameObject;
        DealDamageToTarget(target);
    }

    void StopOnHittingTarget()
    {
        thrown = false;
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        Destroy(gameObject.GetComponent<PolygonCollider2D>());
        Destroy(this.gameObject, 2f);
    }

    void DealDamageToTarget(GameObject target) //TODO pull data from user
    {
        if (target.GetComponent<HealthSystem>())
        {
            HealthSystem targetHealthSystem = target.GetComponent<HealthSystem>();
            targetHealthSystem.ProcessIncomingHit(20f, 50f, 50f, 1f);
        }
    }

}

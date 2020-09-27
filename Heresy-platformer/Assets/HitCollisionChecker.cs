using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollisionChecker : MonoBehaviour
{    
    public List<GameObject> hitTargets = new List<GameObject>();
    public GameObject GetHitTargets() 
    {
        foreach (GameObject hitTarget in hitTargets)
        {
            return hitTarget;
        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision) //TODO maybe mark the whole object with a script or interface, e.g. Damagable?
    {
        if (collision.gameObject.GetComponent<HealthSystem>() || collision.gameObject.GetComponentInParent<HealthSystem>())
        {
            hitTargets.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<HealthSystem>() || collision.gameObject.GetComponentInParent<HealthSystem>())
        {
            hitTargets.Remove(collision.gameObject);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionChecker : MonoBehaviour
{
    const int ACTORNONCOLLIDABLE_LAYER = 23;
    public List<GameObject> finishOffTargets = new List<GameObject>();
    public List<GameObject> interactionTargets = new List<GameObject>();
    public GameObject GetFinishOffTargets() //TODO does not work, does not properly clear the list
    {
        foreach (GameObject finishOffTarget in finishOffTargets)
        {
            return finishOffTarget;
        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision) //TODO maybe mark the whole object with a script or interface, e.g. Damagable?
    {
        if (collision.gameObject.GetComponent<HealthSystem>() ||
            collision.gameObject.GetComponentInParent<HealthSystem>())
        {
            if (collision.gameObject.layer == ACTORNONCOLLIDABLE_LAYER && !collision.isTrigger)
            {
                finishOffTargets.Add(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<HealthSystem>() ||
            collision.gameObject.GetComponentInParent<HealthSystem>())
        {
            if (collision.gameObject.layer == ACTORNONCOLLIDABLE_LAYER && !collision.isTrigger)
            {
                finishOffTargets.Remove(collision.gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float activationChance;
    public List<GameObject> objectsToSpawn = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        if (ItActivates())
        {
            int randomListItem = Random.Range(0, objectsToSpawn.Count);
            GameObject item = Instantiate(objectsToSpawn[randomListItem]);
            item.transform.position = this.transform.position;
        }
    }

    bool ItActivates()
    {
        float activationRoll = Random.Range(0f, 1f);
        Debug.Log(gameObject.name + "'s activation roll: " + activationRoll);
        if (activationRoll <= activationChance)
            return true;
        else
            return false;

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation : MonoBehaviour
{
    [SerializeField]
    GameObject lockedDoor;
    [SerializeField]
    GameObject unlockedDoor;

    [SerializeField]
    bool isLocked = false;

    private void Awake()
    {
        lockedDoor.SetActive(false);
        unlockedDoor.SetActive(false);
        if (isLocked)
        {
            lockedDoor.SetActive(true);
        }
        else
        {
            unlockedDoor.SetActive(true);
        }
    }
    void Start()
    {

    }

    public void OpenDoor()
    {
        lockedDoor.SetActive(false);
        unlockedDoor.SetActive(true);
    }

}

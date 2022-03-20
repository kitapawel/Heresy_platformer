using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    public float time;

    // Update is called once per frame
    private void OnMouseDown()
    {
        Destroy(gameObject, time);
    }
}

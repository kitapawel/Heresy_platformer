using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach to each parent background image
//solution inspired by tutorial: https://www.youtube.com/watch?v=zit45k6CUMk
public class ParallaxBackground : MonoBehaviour
{
    private float bckgrImageLength;
    private float bckgrImageStartPosition;
    public GameObject myCamera;
    public float parallaxEffectAmount; //0 for no movement, 1 for speed equal to camera

    // Start is called before the first frame update
    void Start()
    {
        bckgrImageStartPosition = transform.position.x;
        bckgrImageLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float offsetRelativeToCamera = myCamera.transform.position.x * (1 - parallaxEffectAmount);
        float dist = myCamera.transform.position.x * parallaxEffectAmount;
        
        transform.position = new Vector3(bckgrImageStartPosition + dist, transform.position.y, transform.position.z);

        if (offsetRelativeToCamera > bckgrImageStartPosition + bckgrImageLength)
        {
            bckgrImageStartPosition += bckgrImageLength;
        } else if (offsetRelativeToCamera < bckgrImageStartPosition - bckgrImageLength)
        {
            bckgrImageStartPosition -= bckgrImageLength;
        }

    }
}

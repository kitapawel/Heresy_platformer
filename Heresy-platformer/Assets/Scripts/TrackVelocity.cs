using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackVelocity : MonoBehaviour
{
    Rigidbody2D parentRigidBody2D;
    public Text velocity;

    void Start()
    {
        parentRigidBody2D = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity.text = "Velocity x: " + System.Math.Round(parentRigidBody2D.velocity.x, 0) + " || Velocity y: " + System.Math.Round(parentRigidBody2D.velocity.y, 0);
    }
}

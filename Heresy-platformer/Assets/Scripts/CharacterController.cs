using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class CharacterController : MonoBehaviour
{
    [Header("Component references")]
    Rigidbody2D myRigidBody2D;
    SpriteRenderer mySpriteRenderer;
    Animator myAnimator;
    BoxCollider2D groundChecker;

    [Header("Physical properties")]
    Vector2 myVelocity;

    [Header("Boolean values")]
    bool isAlive = true;
    bool canWalk = true;
    bool isWalking = false;
    bool isMoving = false;
    bool isGrounded = true;
    bool isTouchingGround = true;
    bool shiftPressed = false;


    void Start()
    {

    }

    void FixedUpdate()
    {
        CheckIfGrounded();
        MonitorVelocity();
    }

    private void Update()
    {
        IsShiftPressed();

        if (isAlive)
        {
            if (isGrounded)
            {
                ProcessAdvancedMovement();
                ProcessMovement();
                ProcessAttacks();
            }
        }
        if (!isAlive)
        {
            Death();
        }
    }

    private void ProcessAdvancedMovement()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded && canWalk && shiftPressed)
        {
            SetCanWalk(0);
            myAnimator.SetTrigger("Roll");
            myRigidBody2D.velocity = new Vector2(0f, 0f);
            myRigidBody2D.AddForce(new Vector2(500f * GetSpriteDirection(), 0f), ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded && canWalk)
        {
            SetCanWalk(0);
            myAnimator.SetTrigger("Dodge");
            myRigidBody2D.velocity = new Vector2(0f, 0f);
            myRigidBody2D.AddForce(new Vector2(-320f * GetSpriteDirection(), 0f), ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.W) && isGrounded && canWalk)
        {
            SetCanWalk(0);
            //myAnimator.SetTrigger("Jump");
            myRigidBody2D.AddForce(new Vector2(0f, 320f), ForceMode2D.Impulse);
        }
    }

    void ProcessMovement() {

        if (Input.GetKey(KeyCode.A) && canWalk && shiftPressed)
        {
            isWalking = true;
            myAnimator.SetBool("isMoving", true);
            myAnimator.SetFloat("MoveBlendValue", 1);
            myRigidBody2D.velocity = new Vector2(-3.6f, myRigidBody2D.velocity.y);
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
        }
        else if (Input.GetKey(KeyCode.D) && canWalk && shiftPressed)
        {
            isWalking = true;
            myAnimator.SetBool("isMoving", true);
            myAnimator.SetFloat("MoveBlendValue", 1);
            myRigidBody2D.velocity = new Vector2(3.6f, myRigidBody2D.velocity.y);
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        }
        else if (Input.GetKey(KeyCode.A) && canWalk)
        {
            isWalking = true;
            myAnimator.SetBool("isMoving", true);
            myAnimator.SetFloat("MoveBlendValue", 0);
            myRigidBody2D.velocity = new Vector2(-1.8f, myRigidBody2D.velocity.y);
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
        }
        else if (Input.GetKey(KeyCode.D) && canWalk)
        {
            isWalking = true;
            myAnimator.SetBool("isMoving", true);
            myAnimator.SetFloat("MoveBlendValue", 0);
            myRigidBody2D.velocity = new Vector2(1.8f, myRigidBody2D.velocity.y);
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            isAlive = false;
        }
        else
        {
            isWalking = false;
            myAnimator.SetBool("isMoving", false);
        }
    }

    void ProcessAttacks()
    {
        if (Input.GetMouseButtonDown(0) && shiftPressed)
        {
            myAnimator.SetTrigger("Stab");            
        }
        if (Input.GetMouseButtonDown(0))
        {
            myAnimator.SetTrigger("Attack");
        }
    }


    void MonitorVelocity() 
    {
        myVelocity = myRigidBody2D.velocity;
    }

    void SetCanWalk(int value)
    {
        if (value == 0)
        {
            canWalk = false;
        }
        else if (value == 1)
        {
            canWalk = true;
        }
    }

    void IsShiftPressed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftPressed = true;
        }
        else
        {
            shiftPressed = false;
        }
    }

    void Death() 
    {
        isAlive = false;
        canWalk = false;
        myAnimator.Play("Hero_Death");
    }

    void CheckIfGrounded() 
    {
        if ((myRigidBody2D.velocity.y > 0.05f || myRigidBody2D.velocity.y < -0.05f) && !isTouchingGround)
        {
            isGrounded = false;
            myAnimator.SetBool("isGrounded", false);
        }
        else
        {
            isGrounded = true;
            myAnimator.SetBool("isGrounded", true);
        }
    }

    public void CheckGroundCollision(bool value)
    {
        if (value == true)
        {
            isTouchingGround = true;
            UnityEngine.Debug.Log(isTouchingGround);
        }
        if (value == false)
        {
            isTouchingGround = false;
            UnityEngine.Debug.Log(isTouchingGround);
        }
    }

    private float GetSpriteDirection()
    {
        return Mathf.Sign(transform.localScale.x);
    }

}

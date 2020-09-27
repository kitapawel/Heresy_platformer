using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Component references")]
    PlayerInput myPlayerInput;
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
    bool isFallen = false;
    bool isGrounded = true;
    bool isTouchingGround = true;
    bool shiftPressed = false;

    [Header("Character's movement stats")]
    [SerializeField] float moveSpeed = 1.8f;
    [SerializeField] float runSpeed = 3.6f;
    [SerializeField] float jumpForce = 320f;
    [SerializeField] float rollForce = 500f;
    [SerializeField] float dodgeForce = 320f;

    void Start()
    {
        myPlayerInput = GetComponent<PlayerInput>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        groundChecker = GetComponentInChildren<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        CheckIfGrounded();
        MonitorVelocity();
        ProcessAttacks();
        ProcessMovement();

        if (isAlive)
        {
            if (isGrounded && canWalk)
            {
                
            }
        }
        if (!isAlive)
        {
            Death();
        }
    }
    void ProcessMovement()
    {
        if (myPlayerInput.jump && isGrounded && canWalk)
        {
            SetCanWalk(0);
            myRigidBody2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isWalking = false;
            //Set the SetCanWalk parameter in other animations, so that after jump the value is reset
        }
        else if (myPlayerInput.roll && isGrounded && canWalk)
        {
            SetCanWalk(0);
            isWalking = false;
            myAnimator.SetTrigger("Roll");
            myRigidBody2D.velocity = new Vector2(0f, 0f);
            myRigidBody2D.AddForce(new Vector2(rollForce * GetSpriteDirection(), 0f), ForceMode2D.Impulse);
        }
        else if (myPlayerInput.dodge && isGrounded && canWalk)
        {               
            SetCanWalk(0);
            isWalking = false;
            myAnimator.SetTrigger("Dodge");
            myRigidBody2D.velocity = new Vector2(0f, 0f);
            myRigidBody2D.AddForce(new Vector2(-dodgeForce * GetSpriteDirection(), 0f), ForceMode2D.Impulse);
        }
        else if (myPlayerInput.shiftPressed && myPlayerInput.horizontal != 0 && canWalk)
        {
            isWalking = true;
            myAnimator.SetBool("isMoving", true);
            myAnimator.SetFloat("MoveBlendValue", 1);
            transform.localScale = new Vector3(GetMoveDirection(), transform.localScale.y, transform.localScale.z);
            float xVelocity = runSpeed * myPlayerInput.horizontal;
            myRigidBody2D.velocity = new Vector2(xVelocity, myRigidBody2D.velocity.y);
        }
        else if (myPlayerInput.shiftPressed == false && myPlayerInput.horizontal != 0 && canWalk)
        {
            isWalking = true;
            myAnimator.SetBool("isMoving", true);
            myAnimator.SetFloat("MoveBlendValue", 0);
            transform.localScale = new Vector3(GetMoveDirection(), transform.localScale.y, transform.localScale.z);
            float xVelocity = moveSpeed * myPlayerInput.horizontal;
            myRigidBody2D.velocity = new Vector2(xVelocity, myRigidBody2D.velocity.y);
        }
        else
        {
            isWalking = false;
            myAnimator.SetBool("isMoving", false);
        }
    }
    void ProcessAttacks()
    {
        if (isGrounded && canWalk)
        {
            if (myPlayerInput.basicAttack && myPlayerInput.shiftPressed)
            {
                myAnimator.SetTrigger("Stab");
            }
            if (myPlayerInput.basicAttack)
            {
                myAnimator.SetTrigger("Attack");
            }
        }
    }

    public void MonitorVelocity()
    {
        myVelocity = myRigidBody2D.velocity;
    }

    public void SetCanWalk(int value)
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

    public void Death()
    {
        isAlive = false;
        canWalk = false;
        myAnimator.Play("Hero_Death");
    }

    public void Fall()
    {
        myAnimator.Play("Sword_Hero_fall");
        myAnimator.SetBool("isFallen", true);
    }

    public void CheckIfGrounded()
    {
        if ((myRigidBody2D.velocity.y != 0) && !isTouchingGround)
        {
            isGrounded = false;
            SetCanWalk(0);
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
            SetCanWalk(0);
            UnityEngine.Debug.Log(isTouchingGround);
        }
    }

    private float GetSpriteDirection()
    {
        return Mathf.Sign(transform.localScale.x);
    }
    private float GetMoveDirection()
    {
        return Mathf.Sign(myPlayerInput.horizontal);
    }

}

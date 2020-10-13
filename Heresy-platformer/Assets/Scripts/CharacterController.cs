using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Component references")]
    PlayerInput myPlayerInput;
    Rigidbody2D myRigidBody2D;
    SpriteRenderer mySpriteRenderer;
    Animator myAnimator;
    BoxCollider2D groundChecker;
    AudioSource myAudioSource;

    [Header("Physical properties")]
    Vector2 myVelocity;

    [Header("Boolean values")]
    bool isAlive = true;
    bool canWalk = true;
    bool canClimb = false;
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

    private void Awake()
    {
        gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        myPlayerInput = GetComponent<PlayerInput>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        groundChecker = GetComponentInChildren<BoxCollider2D>();
        myAudioSource = GetComponent<AudioSource>();
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
        else if (myPlayerInput.climb && isGrounded && canWalk)
        {
            CheckIfCanClimb();
            if (canClimb)
            {
                StartCoroutine("Climbing");
            }
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
    IEnumerator Climbing()
    {
        SetCanWalk(0);
        isWalking = false;
        myRigidBody2D.bodyType = RigidbodyType2D.Static;
        myAnimator.Play("Sword_Hero_Climb");
        yield return new WaitForSeconds(0.2f);
        transform.position = transform.position + new Vector3(0.2f * GetSpriteDirection(), 0f, 0f); 
        yield return new WaitForSeconds(0.2f);
        transform.position = transform.position + new Vector3(0.1f * GetSpriteDirection(), 0.5f, 0f);
        yield return new WaitForSeconds(0.2f);
        transform.position = transform.position + new Vector3(0f * GetSpriteDirection(), 0.2f, 0f);
        yield return new WaitForSeconds(0.2f);
        transform.position = transform.position + new Vector3(0.2f * GetSpriteDirection(), 0.3f, 0f);
        yield return new WaitForSeconds(0.1f);
        CheckIfCanClimb();
        myRigidBody2D.bodyType = RigidbodyType2D.Dynamic;
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
        if (isGrounded)
        {
            myAnimator.Play("Sword_Hero_fall");
            myAnimator.SetBool("isFallen", true);
        }
    }

    // The options "Queries hit triggers" and "Queries start in colliders" need to be disabled,
    //  as the raycast first hits all the colliders on the gameobject itself
    public void CheckIfCanClimb()
    {   
        RaycastHit2D overheadRaycastHit = Physics2D.Raycast(new Vector2 (transform.position.x, transform.position.y + 1.05f), Vector2.right * GetSpriteDirection(), 0.5f);
        RaycastHit2D eyeRaycastHit = Physics2D.Raycast(new Vector2 (transform.position.x, transform.position.y + 0.85f), Vector2.right * GetSpriteDirection(), 0.3f);

        if (!overheadRaycastHit && eyeRaycastHit)
        {
            canClimb = true;
        } else
        {
            canClimb = false;
        }
    }

    public void CheckIfGrounded()
    {
        if ((myRigidBody2D.velocity.y != 0) && !isTouchingGround && !isFallen)
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

    public float GetSpriteDirection()
    {
        return Mathf.Sign(transform.localScale.x);
    }
    private float GetMoveDirection()
    {
        return Mathf.Sign(myPlayerInput.horizontal);
    }

}

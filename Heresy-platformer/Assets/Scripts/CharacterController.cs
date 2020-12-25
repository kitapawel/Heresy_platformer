using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Component references")]
    ControlInput myInput;
    Rigidbody2D myRigidBody2D;
    SpriteRenderer mySpriteRenderer;
    Animator myAnimator;
    BoxCollider2D groundChecker;
    AudioSource myAudioSource;
    HealthSystem myHealthSystem;

    [Header("Physical properties")]
    Vector2 myVelocity;

    [Header("Boolean values")]
    public bool isAlive = true;
    bool canWalk = true;
    public bool canClimb = false;
    bool isWalking = false;
    public bool isParrying = false;
    bool isGrounded = true;
    bool isTouchingGround = true;

    [Header("Character's movement stats")]
    [SerializeField] float moveSpeed = 1.8f;
    [SerializeField] float runSpeed = 3.6f;
    [SerializeField] float jumpForce = 320f;
    [SerializeField] float rollForce = 500f;
    [SerializeField] float dodgeForce = 320f;

    const int ACTOR_LAYER = 22;
    const int ACTORNONCOLLIDABLE_LAYER = 23;

    private void Awake()
    {
        gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        myInput = GetComponent<ControlInput>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        groundChecker = GetComponentInChildren<BoxCollider2D>();
        myAudioSource = GetComponent<AudioSource>();
        myHealthSystem = GetComponent<HealthSystem>();
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            CheckIfGrounded();
            MonitorVelocity();
            ProcessMovement();
            ProcessAttacks();
        }
    }
    private void Update()
    {
        if (!isAlive)
        {
            Death();
        }
    }
    void ProcessMovement()
    {
        if (myInput.jump && isGrounded && canWalk)
        {            
            SetCanWalk(0);
            myRigidBody2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isWalking = false;
            //Set the SetCanWalk parameter in other animations, so that after jump the value is reset
        }
        else if (myInput.roll && isGrounded && canWalk)
        {
            SetCanWalk(0);
            isWalking = false;
            myAnimator.SetTrigger("Roll");
            myRigidBody2D.velocity = new Vector2(0f, 0f);
            myRigidBody2D.AddForce(new Vector2(rollForce * GetSpriteDirection(), 0f), ForceMode2D.Impulse);
        }
        else if (myInput.dodge && isGrounded && canWalk)
        {               
            SetCanWalk(0);
            isWalking = false;
            myAnimator.SetTrigger("Dodge");
            myRigidBody2D.velocity = new Vector2(0f, 0f);
            myRigidBody2D.AddForce(new Vector2(-dodgeForce * GetSpriteDirection(), 0f), ForceMode2D.Impulse);
        }
        else if (myInput.climb && isGrounded && canWalk)
        {
            CheckIfCanClimb();
            if (canClimb)
            {
                StartCoroutine("Climbing");
            }
        }
        else if (myInput.shiftPressed && myInput.horizontal != 0 && canWalk)
        {
            isWalking = true;
            myAnimator.SetBool("isMoving", true);
            myAnimator.SetFloat("MoveBlendValue", 1);
            transform.localScale = new Vector3(GetMoveDirection(), transform.localScale.y, transform.localScale.z);
            float xVelocity = runSpeed * myInput.horizontal;
            myRigidBody2D.velocity = new Vector2(xVelocity, myRigidBody2D.velocity.y);
        }
        else if (myInput.shiftPressed == false && myInput.horizontal != 0 && canWalk)
        {
            isWalking = true;
            myAnimator.SetBool("isMoving", true);
            myAnimator.SetFloat("MoveBlendValue", 0);
            transform.localScale = new Vector3(GetMoveDirection(), transform.localScale.y, transform.localScale.z);
            float xVelocity = moveSpeed * myInput.horizontal;
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
            if (myInput.basicAttack && myHealthSystem.CanUseEnergyBasedAction(10))
            {
                myAnimator.SetTrigger("Attack");
                myHealthSystem.UseEnergy(10);
            }
            if (myInput.advancedAttack && myHealthSystem.CanUseEnergyBasedAction(10))
            {
                myAnimator.SetTrigger("Stab");
                myHealthSystem.UseEnergy(10);
            }
            if (myInput.basicAttack && !myHealthSystem.CanUseEnergyBasedAction(10))
            {
                myAnimator.SetTrigger("AttackSlow");
            }
            if (myInput.advancedAttack && !myHealthSystem.CanUseEnergyBasedAction(10))
            {
                myAnimator.SetTrigger("AttackSlow");
            }
            if (myInput.parry)
            {
                myAnimator.SetTrigger("Parry");
            }
            if (myInput.throwItem)
            {
                myAnimator.SetTrigger("Throw");
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
/*        yield return new WaitForSeconds(0.1f);
        CheckIfCanClimb();*/
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
            SetCollisionLayer(1);//Precaution
        }
    }
    public void SetisParrying(int value)
    {
        if (value == 0)
        {
            isParrying = false;
        }
        else if (value == 1)
        {
            isParrying = true;
        }
    }
    public void SetCollisionLayer(int value)
    {
        if (value == 1)
        {
            gameObject.layer = ACTOR_LAYER;
        }
        else if (value == 0)
        {
            gameObject.layer = ACTORNONCOLLIDABLE_LAYER;
        }
    }
    public void SetAliveState(int value)
    {
        if (value == 0)
        {
            isAlive = false;
        }
        else if (value == 1)
        {
            isAlive = true;
        }
    }
    public void Death()
    {
        DisableChildComponents();
        SetCollisionLayer(0);
        canWalk = false;
        myAnimator.Play("Hero_Death");
        this.enabled = false;
    }

    private void DisableChildComponents()
    {
        //Loop through and disable all child objects
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            if (child != null)
                child.SetActive(false);
        }
        myInput.enabled = false;
    }

    public void Fall()
    {
        if (isAlive && isGrounded && myAnimator.GetBool("isFallen") == false)
        {
            myAnimator.Play("Sword_Hero_fall");
            myAnimator.SetBool("isFallen", true);
            SetCollisionLayer(0);
        }
    }

    public void GetHit()
    {
        if (isGrounded && myAnimator.GetBool("isFallen") == false)
        {
            myAnimator.Play("Sword_Hero_GetHit");
        }
    }
    public void GetParried(float appliedForce, float attackVector)
    {
        if (isGrounded && myAnimator.GetBool("isFallen") == false)
        {
            myAnimator.Play("Sword_Hero_GetParried");
            myRigidBody2D.AddForce(new Vector2(attackVector * appliedForce, 0f), ForceMode2D.Impulse);
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
        if ((myRigidBody2D.velocity.y != 0) && !isTouchingGround && !myAnimator.GetBool("isFallen"))
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
            //UnityEngine.Debug.Log(isTouchingGround);
        }
        if (value == false)
        {
            isTouchingGround = false;
            SetCanWalk(0);
            //UnityEngine.Debug.Log(isTouchingGround);
        }
    }
    public float GetSpriteDirection()
    {
        return Mathf.Sign(transform.localScale.x);
    }
    public float GetMoveDirection()
    {
        return Mathf.Sign(myInput.horizontal);
    }

    //Used for adding movement to animations
    public void BoostVelocity(float boostValue)
    {
        myRigidBody2D.AddForce(new Vector2(boostValue * GetSpriteDirection(), 0f), ForceMode2D.Impulse);
    }
    public void LookTheOtherWay()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
    }

}

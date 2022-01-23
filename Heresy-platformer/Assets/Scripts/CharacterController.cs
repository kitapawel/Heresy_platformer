﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterController : MonoBehaviour
{
    [SerializeField] CharacterStats characterStats;
    [Header("Component references")]
    ControlInput myInput;
    Rigidbody2D myRigidBody2D;
    Animator myAnimator;
    HealthSystem myHealthSystem;
    InventorySystem myInventorySystem;

    [Header("Physical properties")]

    [Header("Boolean values")]
    public bool isInspecting = false;
    public bool isAlive = true;
    bool canWalk = true;
    public bool canClimb = false;
    bool isWalking = false;
    public bool isParrying = false;
    bool isGrounded = true;
    bool isTouchingGround = true;

    [Header("Character's movement stats")]
    public float moveSpeed;
    public float runSpeed;
    public float jumpForce;
    public float rollForce;
    public float dodgeForce;
    public float runCost;

    [Header("Character's action stats")]
    public float attackEfficiency;
    public float actionCost;
    public float primaryAttackLevel;
    public float secondaryAttackLevel;

    const int ACTOR_LAYER = 22;
    const int ACTORNONCOLLIDABLE_LAYER = 23;
    const int DEAD_LAYER = 27;

    private void Awake()
    {
        gameObject.AddComponent<AudioSource>();

    }

    private void Start()
    {
        InitializeStats();
        myInput = GetComponent<ControlInput>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myHealthSystem = GetComponent<HealthSystem>();
        myInventorySystem = GetComponent<InventorySystem>();
    }

    private void FixedUpdate()
    {
        if (!isAlive)
        {
            Death();
        }
        if (isAlive)
        {
            CheckIfGrounded();
            if (!myInventorySystem.IsInventoryOverflowing())
            {
                ProcessMovement();
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    ProcessAttacks();
                }
            }
        }
    }
    private void InitializeStats()
    {
        moveSpeed = characterStats.baseMoveSpeed;
        runSpeed = characterStats.baseRunSpeed;
        jumpForce = characterStats.baseJumpForce;
        rollForce = characterStats.baseRollForce;
        dodgeForce = characterStats.baseDodgeForce;
        attackEfficiency = characterStats.baseAttackEfficiency;
        actionCost = characterStats.baseActionCost;
        runCost = characterStats.BaseRunCost;
        primaryAttackLevel = characterStats.basePrimaryAttackLevel;
        secondaryAttackLevel = characterStats.baseSecondaryAttackLevel;
    }
    void ProcessMovement()
    {        
        if (myInput.inspect && myInput.horizontal==0)
        {
            if (isInspecting == true)
            {
                FindObjectOfType<TooltipController>().ShowToolTip("");
                isInspecting = false;
                myAnimator.SetBool("isInspecting", false);

            }
            else
            {
                isInspecting = true;
                myAnimator.SetBool("isInspecting", true);
            }
        }

        if (myInput.inventory)
        {
            FindObjectOfType<PlayerCanvasController>().ToggleInventoryWindow();
        }

        if (!isInspecting)
        {
            if (myInput.jump && isGrounded && canWalk && myHealthSystem.CanUseEnergyBasedAction(actionCost))
            {
                myHealthSystem.UseEnergy(actionCost);
                SetCanWalk(0);
                myRigidBody2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                isWalking = false;
                //Set the SetCanWalk parameter in other animations, so that after jump the value is reset
            }
            else if (myInput.roll && isGrounded && canWalk && myHealthSystem.CanUseEnergyBasedAction(actionCost))
            {
                myHealthSystem.UseEnergy(actionCost);
                SetCanWalk(0);
                isWalking = false;
                myAnimator.SetTrigger("Roll");
                myRigidBody2D.velocity = new Vector2(0f, 0f);
                myRigidBody2D.AddForce(new Vector2(rollForce * GetSpriteDirection(), 0f), ForceMode2D.Impulse);
            }
            else if (myInput.dodge && isGrounded && canWalk && myHealthSystem.CanUseEnergyBasedAction(actionCost))
            {
                myHealthSystem.UseEnergy(actionCost);
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
            else if (myInput.shiftPressed && myInput.horizontal != 0 && canWalk && myHealthSystem.CanUseEnergyBasedAction(runCost))
            {
                myHealthSystem.UseEnergy(runCost * Time.deltaTime);

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
            else if (myInput.energyBoost == true)
            {
                myHealthSystem.QuickEnergyRegen();
            }
            else
            {
                isWalking = false;
                myAnimator.SetBool("isMoving", false);
            }
        }        
    }
    void ProcessAttacks()
    {
        if (isGrounded && canWalk && !isInspecting)
        {
            if (myInput.basicAttack && myHealthSystem.CanUseEnergyBasedAction(GetWeaponStaminaCost()))
            {
                myAnimator.SetTrigger("PrimaryAttack");
                myAnimator.SetFloat("PrimaryAttackBlendValue", primaryAttackLevel);                
                myHealthSystem.UseEnergy(GetWeaponStaminaCost());
            }
            if (myInput.advancedAttack && myHealthSystem.CanUseEnergyBasedAction(GetWeaponStaminaCost()))
            {
                myAnimator.SetTrigger("SecondaryAttack");
                myAnimator.SetFloat("SecondaryAttackBlendValue", secondaryAttackLevel);
                myHealthSystem.UseEnergy(GetWeaponStaminaCost());
            }
            if (myInput.combo && myHealthSystem.CanUseEnergyBasedAction(GetWeaponStaminaCost()))
            {
                myAnimator.SetTrigger("Combo");
                //myAnimator.SetFloat("SecondaryAttackBlendValue", myCharacterStats.primaryAttackLevel);
                myHealthSystem.UseEnergy(GetWeaponStaminaCost());
            }
            if ((myInput.basicAttack|| myInput.advancedAttack||myInput.combo) && !myHealthSystem.CanUseEnergyBasedAction(GetWeaponStaminaCost()))
            {
                myAnimator.SetTrigger("AttackSlow");
                myHealthSystem.UseEnergy(1f);
            }
            if (myInput.parry && myHealthSystem.CanUseEnergyBasedAction(attackEfficiency))
            {
                myAnimator.SetTrigger("Parry");
                myHealthSystem.UseEnergy(attackEfficiency);
            }
            if (myInput.useTool && myHealthSystem.CanUseEnergyBasedAction(GetToolStaminaCost()))
            {
                if (myInventorySystem.equippedTool != null)
                {
                    myAnimator.SetTrigger("Tool");
                    myAnimator.SetFloat("ToolBlendValue", myInventorySystem.equippedTool.GetToolType());
                    myHealthSystem.UseEnergy(myInventorySystem.equippedTool.energyCost * attackEfficiency);
                } else
                {
                    Debug.Log("No tool equipped.");
                }

            }
            if (myInput.throwItem && myHealthSystem.CanUseEnergyBasedAction(actionCost))
            {
                myAnimator.SetTrigger("Throw");
                myHealthSystem.UseEnergy(actionCost);
            }
        }
    }
    private float GetWeaponStaminaCost()
    {
        return myInventorySystem.equippedWeapon.energyCost * attackEfficiency;
    }
    private float GetToolStaminaCost()
    {
        return myInventorySystem.equippedTool.energyCost * attackEfficiency;
    }

    IEnumerator Climbing()
    {
        SetCanWalk(0);
        isWalking = false;
        myRigidBody2D.bodyType = RigidbodyType2D.Static;
        myAnimator.Play("Climb");
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
        myAnimator.SetBool("isGrounded", true);//Dirty trick... stops falling animation when death occurs mid-air
        myAnimator.Play("Death");
        DisableChildComponents();
        Destroy(myHealthSystem);
        gameObject.layer = DEAD_LAYER;
        //SetCollisionLayer(0);
        canWalk = false;
        this.enabled = false;
    }

    private void DisableChildComponents()
    {
        //Loop through and disable all child objects
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            if (child != null)
            {
                if (!child.GetComponent<ParticleSystem>())
                {
                    child.SetActive(false);
                }
            }                
        }
        myInput.enabled = false;
    }

    public void Fall()
    {
        if (isAlive && myAnimator.GetBool("isFallen") == false)
        {
            myAnimator.Play("Fall");
            myAnimator.SetBool("isFallen", true);
            SetCollisionLayer(0);
        }
    }

    public void GetHit()
    {
        if (isInspecting)
        {
            myAnimator.SetBool("isInspecting", false);
            isInspecting = false;
        }
        if (isGrounded && myAnimator.GetBool("isFallen") == false)
        {
            myAnimator.Play("GetHit");
        }
    }
    public void GetParried(float appliedForce, float attackVector)
    {
        if (isGrounded && myAnimator.GetBool("isFallen") == false)
        {
            myAnimator.Play("GetParried");
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
        }
        if (value == false)
        {
            isTouchingGround = false;
            SetCanWalk(0);
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

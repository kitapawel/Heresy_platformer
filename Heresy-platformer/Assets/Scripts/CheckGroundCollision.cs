using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGroundCollision : MonoBehaviour
{
    CharacterController parentCharacterMovementController;
    SoundSystemForAnimateObjects mySoundPlayerAnimate;
    private void Awake()
    {
        parentCharacterMovementController = GetComponentInParent<CharacterController>();
        mySoundPlayerAnimate = GetComponentInParent<SoundSystemForAnimateObjects>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            mySoundPlayerAnimate.PlayFootsteps();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            parentCharacterMovementController.CheckGroundCollision(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            parentCharacterMovementController.CheckGroundCollision(false);
        }
    }
}

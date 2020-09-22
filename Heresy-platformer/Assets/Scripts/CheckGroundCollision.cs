using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGroundCollision : MonoBehaviour
{
    CharacterMovement parentCharacterMovementController;
    private void Awake()
    {
        parentCharacterMovementController = GetComponentInParent<CharacterMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        parentCharacterMovementController.CheckGroundCollision(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        parentCharacterMovementController.CheckGroundCollision(false);
    }
}

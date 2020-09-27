using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGroundCollision : MonoBehaviour
{
    CharacterController parentCharacterMovementController;
    private void Awake()
    {
        parentCharacterMovementController = GetComponentInParent<CharacterController>();
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

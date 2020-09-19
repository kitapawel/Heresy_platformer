using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGroundCollision : MonoBehaviour
{
    CharacterController parentCharacterController;
    private void Awake()
    {
        parentCharacterController = GetComponentInParent<CharacterController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        parentCharacterController.CheckGroundCollision(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        parentCharacterController.CheckGroundCollision(false);
    }
}

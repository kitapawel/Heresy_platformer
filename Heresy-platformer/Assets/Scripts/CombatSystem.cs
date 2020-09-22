using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    Animator myAnimator;
    PlayerInput myPlayerInput;

    void Start()
    {
        myPlayerInput = GetComponent<PlayerInput>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        ProcessAttacks();
    }

    void ProcessAttacks()
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

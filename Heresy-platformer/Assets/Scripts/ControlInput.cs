using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlInput : MonoBehaviour
{
    [HideInInspector] public float horizontal;
    [HideInInspector] public bool jump;
    [HideInInspector] public bool roll;
    [HideInInspector] public bool dodge;
    [HideInInspector] public bool climb;
    [HideInInspector] public bool basicAttack;
    [HideInInspector] public bool shiftPressed;
}

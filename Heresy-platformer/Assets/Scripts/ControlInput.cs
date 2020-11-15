﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlInput : MonoBehaviour
{
	protected bool readyToClear;

	[HideInInspector] public float horizontal;
    [HideInInspector] public bool jump;
    [HideInInspector] public bool roll;
    [HideInInspector] public bool dodge;
    [HideInInspector] public bool climb;
    [HideInInspector] public bool basicAttack;
    [HideInInspector] public bool parry;
    [HideInInspector] public bool shiftPressed;

	protected void ClearInput()
	{
		//If we're not ready to clear input, exit
		if (!readyToClear)
			return;

		//Reset all inputs
		horizontal = 0f;
		jump = false;
		roll = false;
		dodge = false;
		climb = false;
		basicAttack = false;
		parry = false;
		shiftPressed = false;

		readyToClear = false;
	}
}

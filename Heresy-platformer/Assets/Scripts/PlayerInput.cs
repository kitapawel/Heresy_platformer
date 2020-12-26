﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerInput : ControlInput
{
	[SerializeField] Texture2D normalCursor = null;
	[SerializeField] Texture2D useCursor = null;
	[SerializeField] Vector2 cursorHotSpot = new Vector2(0, 0);

	public bool isInCombatMode = true;

	void Update()
	{
		ProcessInputs();
		IsShiftPressed();
		ClearInput();

		//Clamp the horizontal input to be between -1 and 1
		horizontal = Mathf.Clamp(horizontal, -1f, 1f);

	}

	void FixedUpdate()
	{
		readyToClear = true;
	}

/*	void SwitchPlayerMode()
    {
		if (Input.GetKeyUp(KeyCode.Tab))
		{
			if (isInCombatMode)
			{
				isInCombatMode = false;
				Cursor.SetCursor(useCursor, cursorHotSpot, CursorMode.Auto);
			}
			else
			{
				isInCombatMode = true;
				Cursor.SetCursor(normalCursor, cursorHotSpot, CursorMode.Auto);
			}
		}
	}*/
	void ProcessInputs()
	{
		//Accumulate horizontal axis input
		horizontal += Input.GetAxis("Horizontal");

		//Accumulate button inputs
		jump = jump || Input.GetKey(KeyCode.W);
		dodge = dodge || Input.GetKey(KeyCode.S);
		basicAttack = basicAttack || Input.GetMouseButton(0);
		advancedAttack = advancedAttack || Input.GetMouseButton(2);
		parry = parry || Input.GetMouseButton(1);
		roll = roll || Input.GetKey(KeyCode.Space);
		climb = climb || Input.GetKey(KeyCode.Z);
		throwItem = throwItem || Input.GetKey(KeyCode.Q);

		//Interface inputs

	}

	void IsShiftPressed()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			shiftPressed = true;
		}
		else
		{
			shiftPressed = false;
		}
	}	
}

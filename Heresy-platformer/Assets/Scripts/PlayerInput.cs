using System.Collections;
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
		ClearInput();//clear inputs befor process inputs
		IsShiftPressed();
		ProcessInputs();

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

	void ProcessMove()
    {
		//Accumulate horizontal axis input
		//horizontal += Input.GetAxis("Horizontal");
	}
	void ProcessInputs()
	{
		//Accumulate horizontal axis input
		horizontal += Input.GetAxis("Horizontal");

		//Accumulate button inputs
		jump = jump || Input.GetKeyDown(KeyCode.W);
		dodge = dodge || Input.GetKeyDown(KeyCode.S);
		basicAttack = basicAttack || Input.GetMouseButtonDown(0);
		advancedAttack = advancedAttack || Input.GetMouseButtonDown(1);
		combo = combo || Input.GetMouseButtonDown(2);
		finisher = finisher || Input.GetKeyDown(KeyCode.R);
		parry = parry || Input.GetKeyDown(KeyCode.Tab);
		roll = roll || Input.GetKeyDown(KeyCode.Space);
		climb = climb || Input.GetKeyDown(KeyCode.Z);
		throwItem = throwItem || Input.GetKeyDown(KeyCode.Q);
		energyBoost = energyBoost || Input.GetKeyDown(KeyCode.V);

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

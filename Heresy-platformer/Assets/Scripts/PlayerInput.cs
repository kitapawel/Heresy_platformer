using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlayerInput : ControlInput
{

	void Update()
	{
		//Clear out existing input values, used for synching Update with FixedUpdate
		ClearInput();
		IsShiftPressed();
		ProcessInputs();

		//Clamp the horizontal input to be between -1 and 1
		horizontal = Mathf.Clamp(horizontal, -1f, 1f);
	}

	void FixedUpdate()
	{
		readyToClear = true;
	}



	void ProcessInputs()
	{
		//Accumulate horizontal axis input
		horizontal += Input.GetAxis("Horizontal");

		//Accumulate button inputs
		jump = jump || Input.GetKeyDown(KeyCode.W);
		dodge = dodge || Input.GetKeyDown(KeyCode.S);
		basicAttack = basicAttack || Input.GetMouseButton(0);
		parry = parry || Input.GetMouseButton(1);
		roll = roll || Input.GetKeyDown(KeyCode.Space);
		climb = climb || Input.GetKeyDown(KeyCode.Z);
		throwItem = throwItem || Input.GetKeyDown(KeyCode.E);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class EnemyAIBasic : MonoBehaviour
{
	GameObject target;

	
	bool readyToClear;

	[HideInInspector] public float horizontal;
	[HideInInspector] public bool jump;
	[HideInInspector] public bool roll;
	[HideInInspector] public bool dodge;
	[HideInInspector] public bool climb;
	[HideInInspector] public bool basicAttack;
	[HideInInspector] public bool shiftPressed;

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

	void ClearInput()
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
		shiftPressed = false;

		readyToClear = false;
	}

	void ProcessInputs()
	{
		//Accumulate horizontal axis input
		horizontal += Input.GetAxis("Horizontal");

		//Accumulate button inputs
		jump = jump || Input.GetKeyDown(KeyCode.W);
		dodge = dodge || Input.GetKeyDown(KeyCode.S);
		basicAttack = basicAttack || Input.GetMouseButton(0);
		roll = roll || Input.GetKeyDown(KeyCode.Space);
		climb = climb || Input.GetKeyDown(KeyCode.Z);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public abstract class ControlInput : MonoBehaviour
{
	protected bool readyToClear;

	[HideInInspector] public float horizontal;
    [HideInInspector] public bool jump;
    [HideInInspector] public bool roll;
    [HideInInspector] public bool dodge;
    [HideInInspector] public bool climb;
    [HideInInspector] public bool basicAttack;
    [HideInInspector] public bool advancedAttack;
    [HideInInspector] public bool combo;
    [HideInInspector] public bool finisher;
    [HideInInspector] public bool useTool;
    [HideInInspector] public bool parry;
    [HideInInspector] public bool shiftPressed;
    [HideInInspector] public bool throwItem;
    [HideInInspector] public bool energyBoost;
	[HideInInspector] public bool inspect;
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
		advancedAttack = false;
		combo = false;
		finisher = false;
		useTool = false;
		parry = false;
		shiftPressed = false;
		throwItem = false;
		energyBoost = false;
		inspect = false;

		readyToClear = false;
	}
}

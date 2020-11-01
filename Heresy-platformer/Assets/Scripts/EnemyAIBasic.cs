using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class EnemyAIBasic : ControlInput
{
	CharacterController myCharacterController;
	GameObject target;
	
	[SerializeField]
	float sightRange = 5f;

	bool readyToClear;

	private void Start()
	{
		myCharacterController = GetComponent<CharacterController>();
	}
    void Update()
	{
		LookForTargets();
		if (target != null)
        {
			float step = 1 * Time.deltaTime; // calculate distance to move
			transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
		}
	}

	void FixedUpdate()
	{
		readyToClear = true;
	}

	void ClearInput()
	{

	}

	void ProcessInputs()
	{

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

	public void LookForTargets()
	{
		RaycastHit2D eyeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.85f), Vector2.right * myCharacterController.GetSpriteDirection(), sightRange);

		if (eyeRaycastHit)
		{
			target = eyeRaycastHit.transform.gameObject;
		}
	}

}

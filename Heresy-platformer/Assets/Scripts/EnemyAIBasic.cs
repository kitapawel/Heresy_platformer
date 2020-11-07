using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class EnemyAIBasic : ControlInput
{
	[SerializeField]
	AIState aiState = AIState.Searching;

	CharacterController myCharacterController;
	[SerializeField]
	GameObject target;
	
	[SerializeField]
	float sightRange = 5f;
	[SerializeField]
	float moveSpeed = .8f;
	[SerializeField]
	float lookAroundIntervalBase = 3f;
	[SerializeField]
	float lookAroundInterval = 3f;

	bool readyToClear;

	void Start()
	{
		myCharacterController = GetComponent<CharacterController>();
	}
	void Update()
	{
		ClearInput();
		CheckState();
		PerformStateActions();
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

		//Reset all inputs each FixedUpdate
		horizontal = 0f;
		jump = false;
		roll = false;
		dodge = false;
		climb = false;
		basicAttack = false;
		shiftPressed = false;

		readyToClear = false;
	}

	void CheckState()
	{
		if (target == null)
        {
			aiState = AIState.Searching;
        }
		else
        {
			aiState = AIState.Chasing;
        }
	}
	void PerformStateActions()
	{
		switch (aiState)
		{
			case AIState.Passive:
				break;
			case AIState.Searching:
				LookAround();
				LookForTargets();
				break;
			case AIState.Chasing:
				LookForTargets();
				ChaseTarget();
				break;
			case AIState.Fighting:
				break;
			default:
				break;
		}
	}

	void IsShiftPressed()
	{

	}

	void LookAround()
	{
		if (lookAroundInterval <= 0)
        {
			transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
			lookAroundInterval = lookAroundIntervalBase;
		} else
        {
			lookAroundInterval -= 1f * Time.deltaTime;
        }
    }

	void LookForTargets()
	{
		RaycastHit2D eyeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.85f), Vector2.right * myCharacterController.GetSpriteDirection(), sightRange);

		if (!eyeRaycastHit)
		{
			target = null;
		}

		if (eyeRaycastHit)
		{
			if (eyeRaycastHit.transform.tag == "Player")
            {
				target = eyeRaycastHit.transform.gameObject;
			}			
		}
	}

    void ChaseTarget()
	{
			//float step = 1 * Time.deltaTime; // calculate distance to move
			//transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
			if (transform.position.x < target.transform.position.x)
            {
				horizontal = moveSpeed;
			}
			else if (transform.position.x > target.transform.position.x)
			{
				horizontal = -moveSpeed;
			}
	}
}

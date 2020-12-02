using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class EnemyAIBasic : ControlInput
{
	[SerializeField]
	AIState aiState = AIState.Searching;

	CharacterController myCharacterController;
	Animator myAnimator;
	AIPerception myAIPerception;

	[SerializeField]
	GameObject target;
	
	[SerializeField]
	float sightRange = 5f;
	[SerializeField]
	float meleeRange = 1.2f;
	[SerializeField]
	float moveSpeed = .8f;
	[SerializeField]
	float lookAroundIntervalBase = 3f;
	[SerializeField]
	float lookAroundInterval = 3f;

	bool isTargetInMeleeRange;

	void Start()
	{
		myCharacterController = GetComponent<CharacterController>();
		myAnimator = GetComponent<Animator>();
		myAIPerception = GetComponentInChildren<AIPerception>();
	}
	void Update()
	{
		ClearInput();
	}

	/*perform all AI states in FixedUpdate to not overwhelm AI with multiple commands,
	but perform ClearInput in Update to clear unwanted commands to the AI all the time*/
	void FixedUpdate()
	{
		CheckState();
		PerformStateActions();
		readyToClear = true;
	}

	void CheckState()
	{
		if (target == null)
        {
			aiState = AIState.Searching;
        } else if (isTargetInMeleeRange)
        {
			aiState = AIState.Fighting;
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
				LookForTargets();
				FightTarget();
				break;
			default:
				break;
		}
	}

	void LookAround()
	{
        if (myAnimator.GetBool("isFallen") == false)
		{
			if (lookAroundInterval <= 0)
			{
				transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
				lookAroundInterval = lookAroundIntervalBase;
			}
			else
			{
				lookAroundInterval -= 1f * Time.deltaTime;
			}
		}
    }

	void LookForTargets()
	{
		RaycastHit2D eyeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.85f), Vector2.right * myCharacterController.GetSpriteDirection(), sightRange);
		RaycastHit2D meleeRangeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.85f), Vector2.right * myCharacterController.GetSpriteDirection(), meleeRange);
		if (target) // removes NullReference exceptions
        {
			if (eyeRaycastHit.transform.tag != "Player")
			{
				if (!myAIPerception.IsPlayerInRange())
				{
					target = null;
				}
			}
		}

		if (eyeRaycastHit)
		{
			if (eyeRaycastHit.transform.tag == "Player")
            {
				target = eyeRaycastHit.transform.gameObject;
			}			
		}
		if (!meleeRangeRaycastHit)
		{
			isTargetInMeleeRange = false;
		}
		if (meleeRangeRaycastHit)
		{
			isTargetInMeleeRange = true;
		}
	}

    void ChaseTarget()
	{
		if (IsTargetToTheRight())//(transform.position.x < target.transform.position.x)
			{
				horizontal = moveSpeed;
			}
			else if (!IsTargetToTheRight())//(transform.position.x > target.transform.position.x)
			{
				horizontal = -moveSpeed;
			}
	}
	void FightTarget()
	{		
		int randomValue = Random.Range(0, 3);
		switch(randomValue)
		{
			case 0:
				dodge = true;
				break;
			case 1:
			case 2:
				basicAttack = true;				
				break;
			default:
				break;
		}
	}

	bool IsTargetToTheRight()
    {
		if (transform.position.x < target.transform.position.x)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}

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

	LayerMask meleeTargetLayers;

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
		meleeTargetLayers = LayerMask.GetMask("Actor", "ActorNonCollidable");
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
				FightTarget();
				LookForTargets();
				break;
			default:
				break;
		}
	}

	void LookAround()
	{
		if (myAnimator.GetBool("isFallen") == false && !target)
		{
			if (lookAroundInterval <= 0)
			{
				myCharacterController.LookTheOtherWay();
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
		IsTargetInLineOfSight();
		IsTargetInMeleeRange();
	}
	void IsTargetInLineOfSight()
	{
		RaycastHit2D eyeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.85f), Vector2.right * myCharacterController.GetSpriteDirection(), sightRange);

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
	}
	void IsTargetInMeleeRange()
	{
		RaycastHit2D meleeRangeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.85f), Vector2.right * myCharacterController.GetSpriteDirection(), meleeRange, meleeTargetLayers);
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
			case 3:
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

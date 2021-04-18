using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class EnemyAIAggressive : ControlInput
{
	[SerializeField]
	AIState aiState = AIState.Searching;

	CharacterController myCharacterController;
	Animator myAnimator;
	AIPerception myAIPerception;
	LayerMask meleeTargetLayers = LayerMask.GetMask("Actor, ActorNonCollidable");

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
		CheckState();
		PerformStateActions();
		ClearInput();
	}

	/*perform all AI states in FixedUpdate to not overwhelm AI with multiple commands,
	but perform ClearInput in Update to clear unwanted commands to the AI all the time*/
	void FixedUpdate()
	{
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
		RaycastHit2D eyeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.85f), Vector2.right * myCharacterController.GetSpriteDirection(), sightRange, meleeTargetLayers);
		RaycastHit2D meleeRangeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.85f), Vector2.right * myCharacterController.GetSpriteDirection(), meleeRange, meleeTargetLayers);
		if (target)
		{
			if (eyeRaycastHit.transform.tag != "Player")
			{
				if (!myAIPerception.IsTargetInRange())
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
		if (meleeRangeRaycastHit.transform.tag != "Player")
		{
			isTargetInMeleeRange = false;
		}
		if (meleeRangeRaycastHit.transform.tag == "Player")
		{
			isTargetInMeleeRange = true;
		}
	}

    void ChaseTarget()
	{
		//float step = 1 * Time.deltaTime; // calculate distance to move
		//transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
		int randomValue = Random.Range(0, 101);
		if (randomValue > 1)
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
		else
		{
			throwItem = true;
		}

	}
	void FightTarget()
	{		
		int randomValue = Random.Range(0, 4);
		switch(randomValue)
		{
			case 0:
				dodge = true;
				break;
			case 1:
				roll = true;
				break;
			case 2:
				basicAttack = true;				
				break;
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

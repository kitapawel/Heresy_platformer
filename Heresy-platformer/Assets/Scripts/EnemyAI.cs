using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class EnemyAI : ControlInput
{
	[SerializeField]
	AIState aiState = AIState.Searching;
	[SerializeField]
	AIType aiType = AIType.Basic;

	CharacterController myCharacterController;
	Animator myAnimator;
	AIPerception myAIPerception;

	public LayerMask meleeTargetLayers;
	public LayerMask meleeIgnoreTargetLayers;

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
	[SerializeField]
	float reflexes = .2f;//TODO parameterize based on AI type

	bool isTargetInMeleeRange;
	bool turnAroundInProgess = false;

	void Start()
	{
		myCharacterController = GetComponent<CharacterController>();
		myAnimator = GetComponent<Animator>();
		myAIPerception = GetComponentInChildren<AIPerception>();
		meleeTargetLayers = LayerMask.GetMask("Actor", "ActorNonCollidable");
		meleeIgnoreTargetLayers = LayerMask.GetMask("Pickable");
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
        DidTargetGoBehind();
    }
	void IsTargetInLineOfSight()
	{
		RaycastHit2D eyeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.85f), Vector2.right * myCharacterController.GetSpriteDirection(), sightRange, ~meleeIgnoreTargetLayers);

		if (target) // removes NullReference exceptions
		{
			if (eyeRaycastHit.transform != target.transform)
            {
				if (!myAIPerception.IsPlayerInRange())
				{
					target = null;
				}
			}	
		}
		if (eyeRaycastHit)
		{
			if (eyeRaycastHit.transform.CompareTag("Player"))
			{
				target = eyeRaycastHit.transform.gameObject;
			}
		}
	}
	void IsTargetInMeleeRange()
	{
		isTargetInMeleeRange = false; // clear, so that the AI does not keep attacking without player in range
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
	void DidTargetGoBehind()
	{	if (!turnAroundInProgess)
        {
			RaycastHit2D backRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.85f), Vector2.left * myCharacterController.GetSpriteDirection(), 0.5f, ~meleeIgnoreTargetLayers);
			if (backRaycastHit)
			{
				if (backRaycastHit.transform.CompareTag("Player"))
				{
					StartCoroutine(TurnAroundAfterTimeElapsed(reflexes));
				}
			}
		}
	}
	IEnumerator TurnAroundAfterTimeElapsed(float value)
	{
		turnAroundInProgess = true;
		yield return new WaitForSeconds(value);
		myCharacterController.LookTheOtherWay();
		turnAroundInProgess = false;		
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
		int randomValue = Random.Range(0, 6);
		switch(randomValue)
		{
			case 0:
				dodge = true;
				break;
			case 1:
			case 2:
			case 3:
			case 4:
				basicAttack = true;				
				break;
			case 5:
				roll = true;
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

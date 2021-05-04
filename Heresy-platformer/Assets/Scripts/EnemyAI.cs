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
	CharacterStats myCharacterStats;

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
	float reflexes = .8f;//TODO parameterize based on AI type

	bool isTargetInMeleeRange;
	bool turnAroundInProgess = false;

	void Start()
	{
		myCharacterController = GetComponent<CharacterController>();
		myCharacterStats = GetComponent<CharacterStats>();
		myAnimator = GetComponent<Animator>();
		myAIPerception = GetComponentInChildren<AIPerception>();
		meleeTargetLayers = LayerMask.GetMask("Actor", "ActorNonCollidable");
		meleeIgnoreTargetLayers = LayerMask.GetMask("Pickable", "Dead");
		InitializeAIMode();
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

	private void InitializeAIMode()
    {
		if (aiType == AIType.Basic)
        {
			if (myCharacterStats.level == 1)
            {
				myCharacterStats.primaryAttackLevel = 0;
				myCharacterStats.secondaryAttackLevel = 0;
			} else if (myCharacterStats.level == 2)
            {
				myCharacterStats.primaryAttackLevel = 1;
				myCharacterStats.secondaryAttackLevel = 1;
			} else if (myCharacterStats.level == 3)
            {
				myCharacterStats.primaryAttackLevel = 2;
				myCharacterStats.secondaryAttackLevel = 2;
			}
		} else if (aiType == AIType.Aggressive)
        {	
				myCharacterStats.primaryAttackLevel = RandomAttackLevelValue();
				myCharacterStats.secondaryAttackLevel = RandomAttackLevelValue();
		} else
        {
			myCharacterStats.primaryAttackLevel = 0;
			myCharacterStats.secondaryAttackLevel = 0;
		}
    }

	private float RandomAttackLevelValue()
    {
		int result = Random.Range(0, myCharacterStats.level + 2);
		return result;
    }

	void CheckState()
	{
		if (target == null)
        {
			aiState = AIState.Searching;
		}
		else if (target && !IsTargetHostile())
		{
			aiState = AIState.Searching;
		}
		else if (isTargetInMeleeRange && IsTargetHostile())
        {
			aiState = AIState.Fighting;
			return;
		}
		else if (IsTargetHostile())
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
				if (!myAIPerception.IsTargetInRange())
				{
					target = null;
				}
			}	
		}
		if (eyeRaycastHit)
		{
			//if (eyeRaycastHit.transform.CompareTag("Player"))
			if (eyeRaycastHit.transform.GetComponent<HealthSystem>())
			{
				target = eyeRaycastHit.transform.gameObject;
			}
		}
	}
	bool IsTargetHostile()
    {
		if (myCharacterStats.factionType == target.GetComponent<CharacterStats>().factionType){
			return false;
        } else
		return true;

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
	{	if (!turnAroundInProgess && myAnimator.GetBool("isFallen") == false)
        {
			RaycastHit2D backRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.85f), Vector2.left * myCharacterController.GetSpriteDirection(), 0.5f, ~meleeIgnoreTargetLayers);
			if (backRaycastHit)
			{
				//if (backRaycastHit.transform.CompareTag("Player"))
				if (backRaycastHit.transform.GetComponent<HealthSystem>())
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
		int randomValue = Random.Range(0, 10);
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
			case 6:
			case 7:				
			case 8:
			case 9:
				advancedAttack = true;
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

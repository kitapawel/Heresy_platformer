using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class EnemyAI : ControlInput
{
	[SerializeField] 
	CharacterStats characterStats;
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
	float reflexes = .8f;//TODO parameterize based on AI type

	bool isTargetInMeleeRange;
	bool turnAroundInProgess = false;

	void Start()
	{
		myCharacterController = GetComponent<CharacterController>();
		myAnimator = GetComponent<Animator>();
		myAIPerception = GetComponentInChildren<AIPerception>();
		meleeTargetLayers = LayerMask.GetMask("Actor", "ActorNonCollidable");
		meleeIgnoreTargetLayers = LayerMask.GetMask("Pickable", "Dead", "Interactable");
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
			if (characterStats.level == 1)
            {
				characterStats.primaryAttackLevel = 0;
				characterStats.secondaryAttackLevel = 0;
			} else if (characterStats.level == 2)
            {
				characterStats.primaryAttackLevel = 1;
				characterStats.secondaryAttackLevel = 1;
			} else if (characterStats.level == 3)
            {
				characterStats.primaryAttackLevel = 2;
				characterStats.secondaryAttackLevel = 2;
			}
		} else if (aiType == AIType.Aggressive)
        {	
				characterStats.primaryAttackLevel = RandomAttackLevelValue();
				characterStats.secondaryAttackLevel = RandomAttackLevelValue();
		} else
        {
			characterStats.primaryAttackLevel = 0;
			characterStats.secondaryAttackLevel = 0;
		}
    }

	private float RandomAttackLevelValue()
    {
		int result = Random.Range(0, characterStats.level + 2);
		return result;
    }

	void CheckState()
	{
		if (target == null)
        {
			aiState = AIState.Searching;
		}
		else if (!IsTargetHostile())
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
		if (myAnimator.GetBool("isFallen") == false)
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
			//if (eyeRaycastHit.transform.GetComponent<HealthSystem>() && IsTargetHostile())
			if (eyeRaycastHit.transform.GetComponent<CombatSystem>().GetCurrentFactionType() != GetComponent<CombatSystem>().GetCurrentFactionType())
			{
				target = eyeRaycastHit.transform.gameObject;
			}
		}
	}
	bool IsTargetHostile()
    {
		if (GetComponent<CombatSystem>().GetCurrentFactionType() == target.GetComponent<CombatSystem>().GetCurrentFactionType()){
			return false;
        }
        else
        {
			return true;
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
	{	if (!turnAroundInProgess && myAnimator.GetBool("isFallen") == false)
        {
			RaycastHit2D backRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.85f), Vector2.left * myCharacterController.GetSpriteDirection(), 0.5f, ~meleeIgnoreTargetLayers);
			if (backRaycastHit)
			{				
				//if (backRaycastHit.transform.GetComponent<HealthSystem>())
				if (backRaycastHit.transform.GetComponent<CombatSystem>().GetCurrentFactionType() != GetComponent<CombatSystem>().GetCurrentFactionType())
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

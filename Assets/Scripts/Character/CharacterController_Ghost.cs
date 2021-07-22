using UnityEngine;

public class CharacterController_Ghost : CharacterController_Base
{
	[Header ("Random Movement Count")]
	[SerializeField] protected int movementCount;
	[SerializeField] protected int minMovementCount, maxMovementCount;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void UpdateDirection(MovementDirection _dir)
	{
		base.UpdateDirection(_dir);
	}

	protected override void FixedUpdate()
	{
		// Game is not running!!!
		if (MazeController.IsGamePaused)
		{
			Move(false);
			return;
		}

		// Line Cast to next grid for blockage
		if (IsNextGridValid(direction))
		{
			Vector2 pos = Vector2.MoveTowards(transform.position, dest, character.Speed);
			rgBody.MovePosition(pos);
		}
		// Random the ghost movement if hit the wall
		else
		{
			UpdateDirection((MovementDirection)Random.Range(0, 4));
			return;
		}

		if (UpdateDestination())
		{
			if (UpdateRandomMovement())
			{
				UpdateDirection((MovementDirection)Random.Range(0, 4));
			}
		}

		Move(true);
	}

	protected override bool IsNextGridValid(Vector2 _dir)
	{
		return base.IsNextGridValid(_dir);
	}

	protected override void Move(bool _move)
	{
		base.Move(_move);
	}

	protected override void OnTriggerEnter2D(Collider2D _col)
	{
		base.OnTriggerEnter2D(_col);
	}

	protected override void RotationUpdate()
	{
		base.RotationUpdate();
	}

	/// <summary>
	/// Simple random movement by movement count
	/// </summary>
	/// <returns></returns>
	protected virtual bool UpdateRandomMovement()
	{
		movementCount++;

		bool canRandom = Random.Range(minMovementCount, maxMovementCount) < movementCount;

		if (canRandom)
		{
			movementCount = 0;
			return true;
		}
		else
			return false;
	}

	public override void SetCharacter(Character _char)
	{
		base.SetCharacter(_char);
	}
}

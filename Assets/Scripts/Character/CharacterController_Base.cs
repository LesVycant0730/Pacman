using UnityEngine;

public class CharacterController_Base : MonoBehaviour
{
	[Header ("Component")]
	[SerializeField] protected Character character;
	[SerializeField] protected SpriteRenderer spriteRen;
	[SerializeField] protected Animator anim;
	[SerializeField] protected BoxCollider2D col;
	[SerializeField] protected Rigidbody2D rgBody;

	[Header ("Gameplay")]
	[SerializeField] protected bool isPlayer;
	[SerializeField] protected bool isMoving = false;
	[SerializeField] protected bool isUsingOrientationUpdate = false;
	[SerializeField] protected MovementDirection currentDirection = MovementDirection.Right;
	[SerializeField] protected Vector2 direction = Vector2.zero;
	[SerializeField] protected Vector2 dest = Vector2.zero;

	#region Mono
	protected virtual void Awake()
	{
		isPlayer = character.IsPlayer;
		isUsingOrientationUpdate = character.IsUsingOrientationUpdate;
		isMoving = false;
		dest = transform.position;

		if (isUsingOrientationUpdate)
		{
			spriteRen.sprite = character.GetSprite(MovementDirection.Idle);
		}
	}

	protected virtual void FixedUpdate()
	{
		// Game is not running!!!
		if (MazeController.IsGamePaused)
			return;

		// Line Cast to next grid for blockage
		if (IsNextGridValid(direction))
		{
			Vector2 pos = Vector2.MoveTowards(transform.position, dest, character.Speed);
			rgBody.MovePosition(pos);
		}
		// Stop Character if the grid is not valid
		else
		{
			UpdateDirection(isPlayer ? MovementDirection.Idle : (MovementDirection)Random.Range(0, 4));
		}

		UpdateDestination();
	}
	#endregion

	#region Position Update
	protected virtual bool UpdateDestination()
	{
		// Compare position to target direction and then enable player to change direction if reached
		if ((Vector2)transform.position == dest)
		{
			// Update next destination from latest direction
			dest = (Vector2)transform.position + direction;

			// Update rotation
			if (isUsingOrientationUpdate)
				spriteRen.sprite = character.GetSprite(currentDirection);
			else
				RotationUpdate();

			return true;
		}

		return false;
	}

	/// <summary>
	/// Raycast to the grid based on the direction to check if it's valid to move there
	/// </summary>
	/// <returns></returns>
	protected virtual bool IsNextGridValid(Vector2 _dir)
	{
		RaycastHit2D hit = Physics2D.Linecast((Vector2)transform.position + _dir, transform.position);

		return hit == col && !hit.collider.CompareTag("Wall");
	}

	protected virtual void Move(bool _move)
	{
		isMoving = _move;

		if (!isUsingOrientationUpdate)
			anim.SetBool("IsMoving", isMoving);
	}

	/// <summary>
	/// Direction update from player input
	/// </summary>
	/// <param name="_dir"></param>
	protected virtual void UpdateDirection(MovementDirection _dir)
	{
		currentDirection = _dir;

		direction = GetDirection(_dir);
	}

	protected Vector2 GetDirection(MovementDirection _dir)
	{
		Vector2 dirVec;

		switch (_dir)
		{
			case MovementDirection.Up:
				dirVec = Vector2.up;
				break;
			case MovementDirection.Down:
				dirVec = Vector2.down;
				break;
			case MovementDirection.Left:
				dirVec = Vector2.left;
				break;
			case MovementDirection.Right:
				dirVec = Vector2.right;
				break;
			default:
				dirVec = Vector2.zero;
				break;
		}

		// Make sure the direction remain consistent with maze size
		return dirVec *= MazeMap.SIZE_MULTIPLIER;
	}

	/// <summary>
	/// Rotation update from current direction
	/// </summary>
	protected virtual void RotationUpdate()
	{
		switch (currentDirection)
		{
			case MovementDirection.Up:
				transform.rotation = Quaternion.Euler(0, 0, 90);
				break;
			case MovementDirection.Down:
				transform.rotation = Quaternion.Euler(0, 0, -90);
				break;
			case MovementDirection.Left:
				transform.rotation = Quaternion.Euler(0, 180, 0);
				break;
			case MovementDirection.Right:
				transform.rotation = Quaternion.Euler(0, 0, 0);
				break;
		}
	}
	#endregion

	#region Value Update
	public virtual void SetCharacter(Character _char)
	{
		character = _char;

		if (isUsingOrientationUpdate)
		{
			spriteRen.sprite = character.GetSprite(MovementDirection.Idle);
		}
	}
	#endregion

	#region Collision
	// General Collision Update
	protected virtual void OnTriggerEnter2D(Collider2D _col) 
	{
		if (_col.CompareTag(MazeGrid.Tag.TAG_WARP))
		{
			Vector2 warp = MazeController.GetWarpPoint(_col.transform.position);

			transform.position = warp;
			dest = warp;
		}
	}
	#endregion
}

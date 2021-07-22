using UnityEngine;

public class CharacterController_PacMan : CharacterController_Base
{
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void UpdateDirection(MovementDirection _dir)
	{
		base.UpdateDirection(_dir);
	}

	// Input must be in Update, and must not tied to physics in FixedUpdate
	protected void Update()
	{
		Move(!MazeController.IsGamePaused);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}

	protected override void Move(bool _move)
	{
		base.Move(_move);

		// Game is not running!!!
		if (MazeController.IsGamePaused)
			return;

		// Up
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
			UpdateDirection(MovementDirection.Up);
		// Down
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
			UpdateDirection(MovementDirection.Down);
		// Left
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			UpdateDirection(MovementDirection.Left);
		// Right
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			UpdateDirection(MovementDirection.Right);
	}

	protected override bool IsNextGridValid(Vector2 _dir)
	{
		return base.IsNextGridValid(direction);
	}

	protected override void OnTriggerEnter2D(Collider2D _col)
	{
		base.OnTriggerEnter2D(_col);

		// Eat Food
		if (_col.CompareTag(MazeGrid.Tag.TAG_FOOD))
		{
			Destroy(_col.gameObject);

			MazeController.EatFood();

			ParticleManager.Particle_Chomp(_col.transform.position);
		}
		// Eat Power
		else if (_col.CompareTag(MazeGrid.Tag.TAG_POWER))
		{
			Destroy(_col.gameObject);

			MazeController.EatPower();

			ParticleManager.Particle_BigChomp(_col.transform.position);
		}
		else if (_col.CompareTag(MazeGrid.Tag.TAG_ENEMY))
		{
			// End Maze
			MazeController.Lose();
		}
	}

	public override void SetCharacter(Character _char)
	{
		base.SetCharacter(_char);
	}
}

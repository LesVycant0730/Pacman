using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct MazeGrid
{
	[SerializeField] private int posX, posY;
	[SerializeField] private Vector2 pos;
	[SerializeField] private MazeGridState state;
	[SerializeField] private bool hasFood;
	[SerializeField] private GameObject grid;
	[SerializeField] private SpriteRenderer spriteRen;
	[SerializeField] private BoxCollider2D col;

	public int PosX => posX;
	public int PosY => posY;
	public MazeGridState State => state;
	public Vector2 Pos => pos;
	public bool HasFood => HasFood;
	public bool HasPower => hasFood && state == MazeGridState.Power;
	public bool HasWarp => state == MazeGridState.Warp;
	public GameObject Grid => grid;
	public SpriteRenderer SpriteRen => spriteRen;
	public BoxCollider2D Col => col;

	public MazeGrid(int x, int y, MazeGridState _state, GameObject _grid, MazeGridSetting.Data _data)
	{
		posX = x;
		posY = y;
		pos = new Vector2(x, y) * MazeMap.SIZE_MULTIPLIER;
		state = _state;
		hasFood = _state == MazeGridState.Food || _state == MazeGridState.Power;
		grid = _grid;

		if (_grid)
		{
			// Set Name
			_grid.name = $"Grid [{x}, {y}] - {_state}";

			// Set position for the grid
			_grid.transform.position = pos;

			// Set size for the grid
			_grid.transform.localScale *= MazeMap.SIZE_MULTIPLIER;

			// Update sprite component
			spriteRen = _grid.GetComponent<SpriteRenderer>();
			spriteRen.sprite = _data.GridSprite;
			spriteRen.color = _data.GridColor;

			// Update collider component
			col = _grid.GetComponent<BoxCollider2D>();
			col.size *= _data.GridColliderMultiplier;

			// Trigger status
			col.isTrigger = state != MazeGridState.Wall && state != MazeGridState.Enemy_House && state != MazeGridState.Enemy_House_Idle;
		}
		else
		{
			spriteRen = null;
			col = null;
		}
	}

	public void UpdateWallColor(Color _color)
	{
		if (spriteRen)
		{
			spriteRen.color = _color;
		}
	}

	public void ToggleGrid(bool _active)
	{
		if (grid)
		{
			grid.SetActive(_active);
		}
	}

	public void Eaten()
	{
		hasFood = false;
	}

	public bool CanWarp(Vector2 _gridPoint, out Vector2 _warp)
	{
		bool hasSameCol = Mathf.Approximately(pos.x, _gridPoint.x) && !Mathf.Approximately(pos.y, _gridPoint.y);

		// Check line Y / Column
		if (hasSameCol)
		{
			// If collided grid is higher than
			_warp = _gridPoint.y > pos.y ? pos + (Vector2.up * MazeMap.SIZE_MULTIPLIER) : pos + (Vector2.down * MazeMap.SIZE_MULTIPLIER);
			return true;
		}

		bool hasSameRow = Mathf.Approximately(pos.y, _gridPoint.y) && !Mathf.Approximately(pos.x, _gridPoint.x);

		// Check line X / Row
		if (hasSameRow)
		{
			// If collided grid is further than
			_warp = _gridPoint.x > pos.x ? pos + (Vector2.right * MazeMap.SIZE_MULTIPLIER) : pos + (Vector2.left * MazeMap.SIZE_MULTIPLIER);
			return true;
		}

		_warp = Vector2.zero;
		return false;
	}

	#region Tag
	public struct Tag
	{
		public const string TAG_FOOD = "Food";
		public const string TAG_POWER = "Power";
		public const string TAG_ENEMY = "Enemy";
		public const string TAG_WARP = "Warp";
	}
	#endregion
}

public enum MazeGridState
{
	Wall = 0,
	Food = 1,
	Player_Spawn = 2,
	Enemy_Spawn = 3,
	Warp = 4,
	Enemy_House = 5,
	Enemy_House_Idle = 6,
	Power = 7,
	Empty = 8
}

public struct MazeMap
{
	public const int SIZE_X = 21;
	public const int SIZE_Y = 27;

	public const float SIZE_MULTIPLIER = 0.5f;

	public static int[] Map = new int[]
	{
									   /*Warp*/
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 7, 0,
			0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0,
			0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0,
			0, 1, 1, 1, 3, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 1, 1, 1, 1, 0,
			0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0,
			0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0,
			0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0,
			0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
			0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0,
			0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0,
    /*Warp*/4, 2, 1, 1, 1, 1, 1, 1, 1, 1, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4,/*Warp*/
			0, 1, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 1, 0,
			0, 1, 0, 1, 1, 0, 1, 1, 7, 1, 1, 1, 7, 1, 1, 0, 1, 1, 0, 1, 0,
			0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 7, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0,
			0, 1, 1, 1, 7, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 7, 1, 1, 1, 0,
			0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0, 0, 0,
			0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0,
			0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0,
			0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0,
			0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0,
			0, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0,
			0, 1, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 1, 0, 0, 1, 0,
			0, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 1, 0,
			0, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 1, 0,
			0, 1, 1, 1, 7, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 7, 1, 1, 3, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
									   /*Warp*/
	};

	public MazeGrid[,] MazeGrids { get; private set; }
	public MazeGrid PlayerSpawnPoint { get; private set; }
	public List<MazeGrid> GhostSpawnPoints { get; private set; }
	public List<MazeGrid> Warps { get; private set; }
	public int TotalFood { get; private set; } 
	public int TotalPower { get; private set; }
	
	public MazeMap(MazeGrid[,] _grid, MazeGrid _player, List<MazeGrid> _ghosts, List<MazeGrid> _warps, int _food, int _power)
	{
		MazeGrids = _grid;
		PlayerSpawnPoint = _player;
		GhostSpawnPoints = _ghosts;
		Warps = _warps;
		TotalFood = _food;
		TotalPower = _power;
	}

	public Vector2 GetWarpPoint(Vector2 _gridPos)
	{
		Vector2 warpPoint = Vector2.zero;
		MazeGrid grid = Warps.Find(x => x.CanWarp(_gridPos, out warpPoint));

		return warpPoint;
	}
}

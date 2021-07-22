using System;
using UnityEngine;
using System.Collections.Generic;

public class MazeDrawer : MonoBehaviour
{
	[Header("Maze Grid")]
	[SerializeField] private MazeGridSetting mazeGridSetting;
	private MazeGrid[,] mazeGrid;

	[Header("Maze Object")]
	// Map reference
	[SerializeField] private Transform mazeMapParent;

	// Prefab reference
	[SerializeField] private GameObject mazeWall;
	[SerializeField] private Color mazeWallColor;

	/// <summary>
	/// Draw the base maze
	/// </summary>
	public MazeMap DrawMaze()
	{
		// Always clear maze before draw first
		DeleteMaze();

		// New Maze Grid 2D array
		mazeGrid = new MazeGrid[MazeMap.SIZE_X, MazeMap.SIZE_Y];

		// Get Map template
		int[] mazeMapTemplate = MazeMap.Map;

		// Array Count
		int count = 0;

		// Player Spawn
		MazeGrid playerSpawn = new MazeGrid();

		// Enemies Spawn
		List<MazeGrid> enemySpawns = new List<MazeGrid>();

		// Warps
		List<MazeGrid> warps = new List<MazeGrid>();

		// Food Count
		int food = 0;

		// Power Count
		int power = 0;

		// From Top to Bottom
		for (int j = MazeMap.SIZE_Y - 1; j >= 0; j--)
		{
			// From Left to Right
			for (int i = 0; i < MazeMap.SIZE_X; i++)
			{
				// Get the state
				MazeGridState state = (MazeGridState)mazeMapTemplate[count];
				count++;

				// Spawn grid
				GameObject grid = mazeGridSetting.GetGridObject(state, mazeMapParent);

				// Get grid data based on the SO reference
				MazeGridSetting.Data data = mazeGridSetting.GetData(state);

				// New grid
				mazeGrid[i, j] = new MazeGrid(i, j, state, grid, data);

				// Add reference
				switch (state)
				{
					case MazeGridState.Food:
						food++;
						break;
					case MazeGridState.Player_Spawn:
						playerSpawn = mazeGrid[i, j];
						break;
					case MazeGridState.Enemy_Spawn:
						enemySpawns.Add(mazeGrid[i, j]);
						break;
					case MazeGridState.Warp:
						warps.Add(mazeGrid[i, j]);
						break;
					case MazeGridState.Power:
						food++;
						power++;
						break;
				}
			}
		}

		return new MazeMap(mazeGrid, playerSpawn, enemySpawns, warps, food, power);
	}

	/// <summary>
	/// Delete the maze
	/// </summary>
	public void DeleteMaze()
	{
		mazeGrid = null;

		for (int i = mazeMapParent.childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(mazeMapParent.GetChild(i).gameObject);
		}
	}

	/// <summary>
	/// Change the color of the wall
	/// </summary>
	public void ChangeWallColor()
	{
		UpdateGrid((grid) =>
		{
			grid.UpdateWallColor(mazeWallColor);
		});
	}

	private void UpdateGrid(Action<MazeGrid> _action)
	{
		for (int i = 0; i < MazeMap.SIZE_X; i++)
		{
			for (int j = 0; j < MazeMap.SIZE_Y; j++)
			{
				_action.Invoke(mazeGrid[i, j]);
			}
		}
	}

#if UNITY_EDITOR
	/// <summary>
	/// Draw the maze as gizmos
	/// </summary>
	private void OnDrawGizmosSelected()
	{
		if (mazeGrid != null)
		{
			Gizmos.color = Color.blue;

			foreach (MazeGrid grid in mazeGrid)
			{
				Gizmos.DrawWireCube(grid.Pos, Vector3.one);
			}
		}
	}
#endif
}

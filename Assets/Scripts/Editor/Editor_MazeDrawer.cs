using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeDrawer))]
public class Editor_MazeDrawer : Editor
{
	public override void OnInspectorGUI()
	{
		MazeDrawer drawer = (MazeDrawer)target;

		if (GUILayout.Button ("Draw Maze"))
		{
			drawer.DrawMaze();
		}

		if (GUILayout.Button ("Delete Maze"))
		{
			drawer.DeleteMaze();
		}

		if (GUILayout.Button ("Change Wall Color"))
		{
			drawer.ChangeWallColor();
		}

		GUILayout.Space(30);

		base.OnInspectorGUI();
	}
}

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Maze Grid Setting", menuName = "ScriptableObjects/Maze/Grid Setting", order = 1)]
public class MazeGridSetting : ScriptableObject
{
    [Serializable]
    public class Data
	{
		[Header ("State/Tag")]
		[SerializeField] private string tag;
		[SerializeField] private MazeGridState state;

		[Header ("Visual")]
		[SerializeField] private Sprite gridSprite;
		[SerializeField] private Color gridColor;

		[Header ("Collider")]
		[SerializeField, Range (0.0f, 1.0f)] private float gridColliderMultiplier = 1.0f;

		public string Tag => tag;
		public MazeGridState State => state;
		public Sprite GridSprite => gridSprite;
		public Color GridColor => gridColor;
		public float GridColliderMultiplier => gridColliderMultiplier;

		public bool IsSameState(MazeGridState _state) => state == _state;
	}

	[SerializeField] private GameObject gridObject;
	[SerializeField] private Data[] gridDatas;

	/// <summary>
	/// Get grid object based on the Maze Grid State
	/// </summary>
	/// <param name="_state"></param>
	/// <param name="_parent"></param>
	/// <returns></returns>
	public GameObject GetGridObject(MazeGridState _state, Transform _parent)
	{
		GameObject go = Instantiate(gridObject, _parent);
		go.tag = GetData(_state)?.Tag;
		return go;
	}

	/// <summary>
	/// Get grid data reference based on Maze Grid State
	/// </summary>
	/// <param name="_state"></param>
	/// <returns></returns>
	public Data GetData(MazeGridState _state)
	{
		return Array.Find(gridDatas, x => x.IsSameState(_state));
	}

	/// <summary>
	/// Get sprite reference based on Maze Grid State
	/// </summary>
	/// <param name="_state"></param>
	/// <returns></returns>
	public Sprite GetSprite(MazeGridState _state)
	{
		return GetData(_state)?.GridSprite;
	}

	/// <summary>
	/// Get color reference based on Maze Grid State
	/// </summary>
	/// <param name="_state"></param>
	/// <returns></returns>
	public Color GetColor(MazeGridState _state)
	{
		return GetData(_state).GridColor;
	}
}

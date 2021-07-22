using UnityEngine;
using System;

// Base class for all characters include Pac-Man and Ghost
[CreateAssetMenu(fileName = "Character Setting", menuName = "ScriptableObjects/Character/Character", order = 1)]
public class Character : ScriptableObject
{
	[Serializable]
	public class Orientation
	{
		[SerializeField] private MovementDirection direction;
		[SerializeField] private Sprite sprite;

		public MovementDirection Direction => direction;
		public Sprite Sprite => sprite;
	}

	// Setting reference
	[Header("Setting")]
	[SerializeField] private string characterName;
	[SerializeField] private bool isPlayer;

	[SerializeField, Tooltip ("Tick if the sprite is update through sprite swapping on direction instead of animation.")] 
	private bool isUsingOrientationUpdate;

	[SerializeField, Range(0.01f, 0.2f)] private float speed;

	[SerializeField, Tooltip("Tick if the character has speed scaling on cumulative rounds, basically difficulty setting")]
	private bool hasSpeedScaling = true;
	[SerializeField, Range(0.001f, 0.01f)] private float speedScaling;

	[Header("Orientation")]
	[SerializeField] private Orientation[] orientations;

	public string CharacterName => characterName;
	public bool IsPlayer => isPlayer;
	public bool IsUsingOrientationUpdate => isUsingOrientationUpdate;
	public float Speed => Mathf.Clamp(speed + AdditionalSpeed, 0.01f, 0.2f);
	public float AdditionalSpeed => hasSpeedScaling && MazeController.Round > 1 ? speedScaling * (MazeController.Round - 1): 0.0f;

	#region Orientation
	public Orientation GetOrientation(MovementDirection _dir)
	{
		return Array.Find(orientations, x => x.Direction == _dir);
	}

	public Sprite GetSprite(MovementDirection _dir)
	{
		return GetOrientation(_dir)?.Sprite;
	}
	#endregion
}

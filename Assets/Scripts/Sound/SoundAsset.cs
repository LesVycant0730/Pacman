using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Asset", menuName = "ScriptableObjects/Sound/Sound Asset", order = 1)]
public class SoundAsset : ScriptableObject
{
	[Serializable]
    public class Clip
	{
		[SerializeField] private PacManAction paction;
		[SerializeField] private AudioClip audio;
		[SerializeField, Range(0.0f, 1.0f)] private float volume = 1.0f;

		public PacManAction Paction => paction;
		public AudioClip Audio => audio;
		public float Volume => volume;
	}

	[SerializeField] private Clip[] soundActions;

	public Clip GetClipAsset(PacManAction _action)
	{
		return Array.Find(soundActions, x => x.Paction == _action);
	}

	public AudioClip GetClip(PacManAction _action)
	{
		return GetClipAsset(_action)?.Audio;
	}
}

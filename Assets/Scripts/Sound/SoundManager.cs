using UnityEngine;

/// <summary>
/// The main sound manager, only one instance allowed
/// </summary>
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    [SerializeField] private SoundAsset asset;
    private AudioSource source;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
		{
            Destroy(gameObject);
		}
        else
		{
            instance = this;
            DontDestroyOnLoad(gameObject);
            source = GetComponent<AudioSource>();

            // Maze Events
            MazeController.OnCountdown += OnGameplayAudioUpdate;
            MazeController.OnGamePaused += OnAudioPaused;
            MazeController.OnNextRound += OnGameplayExit;
            MazeController.OnGameLose += OnGameplayLose;

            // Scene Event
            SceneController.OnGameplayExit += OnGameplayExit;
        }
    }

    void OnDestroy()
	{
        // Maze Events
        MazeController.OnCountdown -= OnGameplayAudioUpdate;
        MazeController.OnGamePaused -= OnAudioPaused;
        MazeController.OnNextRound -= OnGameplayExit;
        MazeController.OnGameLose -= OnGameplayLose;

        // Scene Event
        SceneController.OnGameplayExit -= OnGameplayExit;
    }

    private void OnGameplayAudioUpdate(int _count)
	{
        // On Intro
        if (_count == 3)
		{
            SoundAsset.Clip clip = UpdateClip(PacManAction.Intro);

            // Dudududududud
            source.loop = false;
            source.PlayOneShot(clip.Audio);
		}
        // On Game Start
        else if (_count == -1)
		{
            SoundAsset.Clip clip = UpdateClip(PacManAction.Eat);

            // Waka Waka
            source.loop = true;
            source.Play();
		}
	}

    private void OnAudioPaused(bool _isPaused)
	{
        if (_isPaused)
            source.Pause();
        else
            source.UnPause();
	}

    private void OnGameplayLose()
	{
        SoundAsset.Clip clip = UpdateClip(PacManAction.Death);

        source.loop = false;
        source.PlayOneShot(clip.Audio);
    }

    private void OnGameplayExit()
	{
        source.Stop();
        source.clip = null;
	}

    private SoundAsset.Clip UpdateClip(PacManAction _action)
	{
        SoundAsset.Clip clip = asset.GetClipAsset(_action);

        source.clip = clip.Audio;
        source.volume = clip.Volume;

        return clip;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Controller for the entire maze gameplay
/// </summary>
public class MazeController : MonoBehaviour
{
	#region Global Variables
	public static MazeMap Maze { get; private set; }

    // Round
    /// <summary>
    /// Current gameplay round, the higher the value, the higher the difficulty
    /// </summary>
    public static int Round { get; private set; } = 0;

    // Status
    public static bool IsGamePaused { get; private set; } = true;
    public static bool IsInputAllowed { get; private set; } = false;

    // Objective (Food)
    public static int TotalScore { get; private set; } = 0;
    public static int TotalFood => Maze.TotalFood;
    public static int EatenFood { get; private set; } = 0;

    // Start game action
    public static event Action<int> OnCountdown;

    // Next Round action
    public static event Action OnNextRound;

    // Game lose action
    public static event Action OnGameLose;

    // Game State action -> Pause/Resume
    public static event Action<bool> OnGamePaused;

    // Eat Food Action
    public static event Action<int, int> OnFoodEaten;

    // Eat Power Action
    public static event Action<bool> OnPowerEaten;
    #endregion

    // Drawer component
    private MazeDrawer mazeDrawer;

    // Prefabs
    [SerializeField] private GameObject pacManPrefab;
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private Character[] ghostCharacters;

    // Spawned characters
    [SerializeField] private Transform characterParent;
    private CharacterController_PacMan pacMan;
    private List<CharacterController_Ghost> ghosts = new List<CharacterController_Ghost>();

	#region Global Method
    public static void EatFood(int _score = 1)
	{
        EatenFood++;
        TotalScore += _score * Round;

        // Stop Game and Win it
        if (EatenFood == TotalFood)
        {
            IsInputAllowed = false;
            IsGamePaused = true;

            // Go next round
            EatenFood = 0;

            OnNextRound?.Invoke();
        }

		OnFoodEaten?.Invoke(TotalScore, Round);
    }

    public static void EatPower()
	{
        EatFood(20);
        OnPowerEaten?.Invoke(true);
	}

    public static void Lose()
	{
        OnGameLose?.Invoke();
	}

    public static MazeGrid GetMazeGrid(int x, int y)
	{
        return Maze.MazeGrids[x, y];
	}

    public static Vector2 GetWarpPoint(Vector2 _pos)
	{
        return Maze.GetWarpPoint(_pos);
    }
    #endregion

    #region Game Status
    private void Setup()
	{
        // Draw the maze
        Maze = mazeDrawer.DrawMaze();

        // Destroy pac man if existed
        if (pacMan)
            Destroy(pacMan.gameObject);

        // Destroy all ghosts if existed
        foreach (var go in ghosts)
            Destroy(go.gameObject);

        ghosts.Clear();

        // Setup Pac Man
        pacMan = Instantiate(pacManPrefab, Maze.PlayerSpawnPoint.Pos, Quaternion.identity, characterParent).GetComponent<CharacterController_PacMan>();
        
        // Setup Ghosts
        for (int i = 0; i < Maze.GhostSpawnPoints.Count && i < ghostCharacters.Length; i++)
		{
            // Instantiate ghost
            CharacterController_Ghost ghost = Instantiate(ghostPrefab, Maze.GhostSpawnPoints[i].Pos, Quaternion.identity, characterParent).GetComponent<CharacterController_Ghost>();

            ghost.SetCharacter(ghostCharacters[i]);

            ghosts.Add(ghost);
		}
    }

    private void ChangeGameState(bool _isPaused)
	{
        IsGamePaused = _isPaused;
	}
    #endregion

    #region Mono
    void Awake()
	{
        mazeDrawer = GetComponent<MazeDrawer>();

        Setup();

        // Game Pause
        OnGamePaused += ChangeGameState;

        // Next Round
        OnNextRound += NextRound;

        // Lose Game
        OnGameLose += LoseRestart;
	}

	// Start is called before the first frame update
	void Start()
    {
        StartCoroutine(StartCountdown());
    }

    void Update()
	{
        if (Input.GetKeyDown(KeyCode.Escape) && IsInputAllowed)
		{
            OnGamePaused?.Invoke(!IsGamePaused);
		}
	}

    private void OnDestroy()
	{
        OnGamePaused -= ChangeGameState;
        OnNextRound -= NextRound;
        OnGameLose -= LoseRestart;

        IsInputAllowed = false;

        Round = 0;
        TotalScore = 0;
        EatenFood = 0;
    }

    private void NextRound()
	{
        StartCoroutine(Restart());
	}

    private void LoseRestart()
	{
        // Reset everything
        IsInputAllowed = false;
        IsGamePaused = true;

        Round = 0;
        TotalScore = 0;
        EatenFood = 0;
	}

    private IEnumerator Restart()
	{
        yield return new WaitForSeconds(2.0f);

        Setup();

        yield return StartCountdown();
	}

    private IEnumerator StartCountdown()
	{
        Round++;

        // Default 0
        OnFoodEaten?.Invoke(TotalScore, Round);

        int count = 3;
        WaitForSeconds countdown = new WaitForSeconds(1.0f);

        // 4 sec delay
        while (count >= 0)
		{
            OnCountdown?.Invoke(count--);
            yield return countdown;
        }

        yield return new WaitForSeconds(0.5f);

        // Means Start
        OnCountdown?.Invoke(-1);

        // Start game
        IsGamePaused = false;

        // Enable input
        IsInputAllowed = true;
	}
	#endregion
}

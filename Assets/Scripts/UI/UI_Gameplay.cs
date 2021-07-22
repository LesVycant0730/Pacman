using UnityEngine;
using UnityEngine.UI;

public class UI_Gameplay : MonoBehaviour
{
	[Header ("Main")]
	[SerializeField] private Text text_Countdown;
	[SerializeField] private GameObject bg_Countdown;

	[Header ("Objective")]
    [SerializeField] private Text[] texts_Score;
    [SerializeField] private Text[] texts_Round;

	[Header("Menu")]
	[SerializeField] private GameObject menu_Gameplay;
	[SerializeField] private GameObject menu_Pause;
	[SerializeField] private GameObject menu_Result;

	private void Awake()
	{
		MazeController.OnCountdown += UpdateCountdown;
		MazeController.OnFoodEaten += UpdateScore;
		MazeController.OnGamePaused += UpdatePauseMenu;
		MazeController.OnGameLose += UpdateResultMenu;
	}

	private void OnDestroy()
	{
		MazeController.OnCountdown -= UpdateCountdown;
		MazeController.OnFoodEaten -= UpdateScore;
		MazeController.OnGamePaused -= UpdatePauseMenu;
		MazeController.OnGameLose -= UpdateResultMenu;
	}

	private void UpdateScore(int _score, int _round)
	{
		foreach (Text score in texts_Score)
			score.text = $"Score: {_score}";

		foreach (Text round in texts_Round)
			round.text = $"Round: {_round}";
	}

	private void UpdateCountdown(int _count)
	{
		if (_count >= 0)
		{
			text_Countdown.text = _count == 0 ? "Start" : $"{_count}";
		}
		else
		{
			text_Countdown.gameObject.SetActive(false);
			bg_Countdown.SetActive(false);
		}
	}

	private void UpdatePauseMenu(bool _enabled)
	{
		menu_Pause.SetActive(_enabled);
	}

	private void UpdateResultMenu()
	{
		menu_Gameplay.SetActive(false);
		menu_Result.SetActive(true);
	}
}

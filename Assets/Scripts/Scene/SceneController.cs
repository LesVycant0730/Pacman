using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneController : MonoBehaviour
{
	private const string SCENE_MAIN_MENU = "MainMenu";
	private const string SCENE_GAMEPLAY_01 = "Gameplay_01";

	public static event Action OnGameplayExit;

	public void ToMainMenu()
	{
		OnGameplayExit?.Invoke();

		SceneManager.LoadScene(SCENE_MAIN_MENU);
	}

	public void ToGameplay01()
	{
		SceneManager.LoadScene(SCENE_GAMEPLAY_01);
	}

	public void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}

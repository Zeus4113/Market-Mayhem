using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private string desiredLevel;

	private Scene m_loadingScreenScene;

	public void StartLoad()
	{
		StartCoroutine(Loading());
	}

    private IEnumerator Loading()
    {
		ToggleLoadingScreen();

		yield return new WaitForSeconds(1f);

		Debug.Log("Wait Over 1");
		LoadDesiredLevel();

		yield return new WaitForSeconds(1f);

		Debug.Log("Wait Over 2");
		UnloadAllLevels();

		yield return null;

    }

	private void ToggleLoadingScreen()
	{
		if (!SceneManager.GetSceneByName("LoadingScreen").isLoaded)
		{
			SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
		}
	}

	private void UnloadAllLevels ()
	{
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene myScene = SceneManager.GetSceneAt(i);

			if (myScene.name != "LoadingScreen" && myScene.name != desiredLevel) SceneManager.UnloadSceneAsync(myScene);
		}
	}

	private void LoadDesiredLevel()
	{
		SceneManager.LoadSceneAsync(desiredLevel, LoadSceneMode.Additive);
	}
}

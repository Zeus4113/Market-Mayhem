using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private string[] desiredLevels;

    private Vector2 m_startPosition;
    private Vector2 m_endPosition;

    private Image m_loadingScreenImage;
    private float m_loadingScreenSpeed;
    private Vector2 m_loadingScreenPositon;
    private float m_duration = 5f;


    public void LoadLevelScreen()
    {
        StartCoroutine(DrawLoadingScreen(desiredLevels));
    }




    private IEnumerator DrawLoadingScreen(string[] desiredLevels)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LoadingScreen");

        while (!asyncLoad.isDone)
        {
            yield return new WaitForFixedUpdate();
        }

        float time = 0;

        while (time < m_duration)
        {
            time += Time.fixedDeltaTime;
            m_loadingScreenImage.transform.position = Vector2.Lerp(m_startPosition, m_endPosition, time / m_duration);
            yield return new WaitForFixedUpdate();

        }

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene myScene = SceneManager.GetSceneAt(i);
            if (myScene.name != "LoadingScreen") UnloadLevels(myScene);
        }

        for(int i = 0; i < desiredLevels.Length; i++)
        {
            LoadLevels(desiredLevels[i]);
        }

        Scene loadingScene = SceneManager.GetSceneByName("LoadingScreen");
        UnloadLevels(loadingScene);

    }

    private void UnloadLevels(Scene scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }

    private void LoadLevels(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }
}

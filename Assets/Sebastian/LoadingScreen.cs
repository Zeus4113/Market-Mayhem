using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingScreen : MonoBehaviour
{
    [SerializeField] string levelName;

    private IEnumerator LoadNextLevel(string levelName)
    {
        AsyncOperation loadlevel = SceneManager.LoadSceneAsync(levelName);

		yield return null;
    }

    private void Start()
    {
       // StartCoroutine(LoadNextLevel());
    }
}

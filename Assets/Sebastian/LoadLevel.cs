using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public void LoadNewLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadLevelAsync(string levelName)
    {
        SceneManager.LoadSceneAsync(levelName);
    }
}

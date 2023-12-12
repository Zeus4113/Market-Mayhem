using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] private GameObject m_pauseMenu;

    // Start is called before the first frame update
    void OnEnable()
    {
		Time.timeScale = 0f;
    }

	void ResumeGame()
	{
		Time.timeScale = 1f;
	}

}

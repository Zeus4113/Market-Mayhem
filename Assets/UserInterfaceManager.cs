 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
	public delegate void GameEndEvent();
	public event GameEndEvent gameLoss;
	public event GameEndEvent gameWon;

	private GameManager m_gameManager;

	private Canvas m_canvas;
	private GameObject m_progressBar;
	private GameObject m_gameOver;
	private GameObject m_winScreen;
	private GameObject m_digitalClock;
	private GameObject m_calendar;
	private GameObject m_peopleCounter;

	private Slider m_progressBarSlider;
	private TMPro.TextMeshProUGUI m_peopleCounterText;

	public void Init(Camera mainCamera, GameManager gm)
	{
		m_gameManager = gm;

		m_canvas = GetComponent<Canvas>();
		if (mainCamera != null)
		{
			m_canvas.worldCamera = mainCamera;
		}

		m_gameManager.GetScoreManager().updateProgressBar += SetProgressBar;
		m_gameManager.GetEnemyManager().enemyCountChange += SetPeopleCount;

		m_progressBar = this.transform.GetChild(0).gameObject;
		m_gameOver = this.transform.GetChild(1).gameObject;
		m_winScreen = this.transform.GetChild(2).gameObject;

		m_calendar = this.transform.GetChild(3).gameObject;
		m_digitalClock = this.transform.GetChild(4).gameObject;
		m_peopleCounter = this.transform.GetChild(5).gameObject;

		m_progressBarSlider = m_progressBar.GetComponentInChildren<Slider>();
		m_peopleCounterText = m_peopleCounter.GetComponentInChildren<TMPro.TextMeshProUGUI>();


		m_digitalClock.GetComponent<TimerController>().OnDayEnd += GameWon;
		m_calendar.SetActive(true);
	}

	public void StartGame()
	{
		m_progressBar.SetActive(true);
		m_calendar.SetActive(true);
		m_digitalClock.SetActive(true);
		m_peopleCounter.SetActive(true);
	}

	private void SetProgressBar(float barPercentage)
	{
		if(m_progressBar != null && m_progressBar.activeSelf && m_progressBarSlider != null) 
		{ 
			m_progressBarSlider.value = barPercentage;
			if(barPercentage <= 0 && m_winScreen.activeSelf == false) { GameLoss(); }
		}
	}

	private void SetPeopleCount(int peopleCount)
	{
		if (m_peopleCounter != null && m_peopleCounter.activeSelf && m_peopleCounterText != null) { m_peopleCounterText.text = peopleCount.ToString(); }
		//Debug.Log(m_progressBarSlider.value);
		if (peopleCount <= 0 && m_gameOver.activeSelf == false) { GameWon(); }
	}

	private void GameLoss()
	{
		EnableWidget("game over overlay", true);

		gameLoss?.Invoke();
	}

	private void GameWon()
	{
		EnableWidget("win screen overlay", true);

		gameWon?.Invoke();
	}


	public void EnableWidget(string name, bool isTrue)
	{
		string widgetName = name.ToLower();

		switch (widgetName)
		{
			case "progress bar":
				if (m_progressBar == null) return;
				m_progressBar.gameObject.SetActive(isTrue);
				break;

			case "game over overlay":
				if (m_gameOver == null) return;
				m_gameOver.gameObject.SetActive(isTrue);
				break;

			case "win screen overlay":
				if (m_winScreen == null) return;
				m_winScreen.gameObject.SetActive(isTrue);
				break;

			case "people counter":
				if (m_peopleCounter == null) return;
				m_peopleCounter.gameObject.SetActive(isTrue);
				break;

			case "calendar":
				if (m_calendar == null) return;
				m_calendar.gameObject.SetActive(isTrue);
				break;

			case "digital clock":
				if (m_digitalClock == null) return;
				m_digitalClock.gameObject.SetActive(isTrue);
				break;
		}
	}

	public GameObject GetWidget(string name)
	{
		string widgetName = name.ToLower();
		GameObject myObject;

		switch (widgetName)
		{
			case "progress bar":
				myObject = m_progressBar;
				break;

			case "game over overlay":
				myObject = m_gameOver;
				break;

			case "people counter":
				myObject = m_peopleCounter;
				break;

			case "calendar":
				myObject = m_calendar;
				break;

			case "digital clock":
				myObject = m_digitalClock;
				break;

			default:
				Debug.Log("Incorrect Widget Name:" +  name);
				myObject = null;
				break;
		}

		return myObject;
	}

}

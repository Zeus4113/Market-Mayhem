 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
	private GameManager m_gameManager;

	private Canvas m_canvas;
	private GameObject m_progressBar;
	private GameObject m_gameOverImage;
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
		m_gameOverImage = this.transform.GetChild(1).gameObject;
		m_calendar = this.transform.GetChild(2).gameObject;
		m_digitalClock = this.transform.GetChild(3).gameObject;
		m_peopleCounter = this.transform.GetChild(4).gameObject;

		m_progressBarSlider = m_progressBar.GetComponentInChildren<Slider>();
		m_peopleCounterText = m_peopleCounter.GetComponentInChildren<TMPro.TextMeshProUGUI>();

	}

	private void SetProgressBar(float barPercentage)
	{
		m_progressBarSlider.value = barPercentage;
	}

	private void SetPeopleCount(int peopleCount)
	{
		m_peopleCounterText.text = peopleCount.ToString();
	}

	public void EnableGameOverWidget(bool isTrue)
	{
		if (m_gameOverImage == null) return;
		m_gameOverImage.gameObject.SetActive(isTrue);
	}

	public void EnableProgressBar(bool isTrue) 
	{
		if (m_progressBar == null) return;
		m_progressBar.gameObject.SetActive(isTrue);
	}
	public void EnableClockWidget(bool isTrue)
	{
		m_canvas.gameObject.SetActive(isTrue);
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
				myObject = m_gameOverImage;
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

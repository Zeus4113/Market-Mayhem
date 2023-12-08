using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{

	private Canvas m_canvas;
	private Transform m_progressBar;
	private Transform m_gameOverImage;
	private Transform m_digitalClock;
	private Transform m_calendar;
	private Transform m_peopleCounter;

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		m_canvas = GetComponent<Canvas>();

		m_progressBar = transform.Find("Progress Bar");
		m_gameOverImage = transform.Find("Game Over Overlay");
		m_peopleCounter = transform.Find("People Counter");
		m_calendar = transform.Find("Calendar");
		m_digitalClock = transform.Find("Digital Clock");

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

	public Transform GetWidget(string name)
	{
		string widgetName = name.ToLower();
		Transform myTransform;

		switch (widgetName)
		{
			case "progress bar":
				myTransform = m_progressBar;
				return myTransform;

			case "game over overlay":
				myTransform = m_gameOverImage;
				return myTransform;

			case "people counter":
				myTransform = m_peopleCounter;
				return myTransform;

			case "calendar":
				myTransform = m_calendar;
				return myTransform;

			case "digital clock":
				myTransform = m_digitalClock;
				return myTransform;

			default:
				Debug.Log("Incorrect Widget Name:" +  name);
				break;
		}

		return null;
	}

}

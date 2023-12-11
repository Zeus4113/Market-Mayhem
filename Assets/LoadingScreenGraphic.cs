using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenGraphic : MonoBehaviour
{
	private Image m_loadingScreenImage;
	private Vector2 m_startPosition;
	private Vector2 m_endPosition;
	private float m_duration = 2f;

	public delegate void LoadingScreenEvent();
	public event LoadingScreenEvent OnLoadingScreenDone;


	void Start()
    {
        m_loadingScreenImage = GetComponentInChildren<Image>();
		m_startPosition = transform.Find("StartPos").position;
		m_endPosition = transform.Find("EndPos").position;

		m_loadingScreenImage.transform.position = m_startPosition;

		StartCoroutine(LerpImage());
    }

	IEnumerator LerpImage()
	{
		float time = 0;

		while (time < m_duration)
		{
			time += Time.deltaTime;

			m_loadingScreenImage.transform.position = Vector2.Lerp(m_startPosition, m_endPosition, time / m_duration);
			yield return new WaitForFixedUpdate();
		}

		OnLoadingScreenDone?.Invoke();

		yield return new WaitForSeconds(1f);

		time = 0;

		while (time < m_duration)
		{
			time += Time.deltaTime;

			m_loadingScreenImage.transform.position = Vector2.Lerp(m_endPosition, m_startPosition, time / m_duration);
			yield return new WaitForFixedUpdate();
		}

		//SceneManager.UnloadSceneAsync("LoadingScreen");

		yield return null;
	}
}

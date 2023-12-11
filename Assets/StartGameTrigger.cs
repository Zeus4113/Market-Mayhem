using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameTrigger : MonoBehaviour
{
	public delegate void OnStartEvent();
	public event OnStartEvent onStart;

	private SpriteRenderer m_spriteRenderer;
	private Collider2D m_collider;

	private void Start()
	{
		m_spriteRenderer = GetComponent<SpriteRenderer>();
		m_collider = GetComponent<Collider2D>();

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision == null) return;

		if(collision.GetComponent<PlayerActions>() == null) return;

		StartGame();
	}

	private void StartGame()
	{
		m_spriteRenderer.color = Color.green;
		m_collider.enabled = false;
		onStart.Invoke();
	}
}

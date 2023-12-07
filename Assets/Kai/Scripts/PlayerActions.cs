using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
	[SerializeField] private float m_punchForce;
	[SerializeField] private float m_attackCooldown;

	private PlayerController m_playerController;
	private Rigidbody2D m_handRB;
	private Transform m_handHomeTransform;

	private Coroutine C_Lerping;
	private bool C_IsLerping = false;

	private Coroutine C_Checking;
	private bool C_IsChecking = false;

	private bool canAttack = true;

	private void Awake()
	{
		m_handRB = this.GetComponent<Rigidbody2D>();
	}

	public void Init(PlayerInput inputComponent, Transform homeLocation, PlayerController characterController, string handSide)
	{
		if(handSide.ToLower() == "left")
		{
			inputComponent.actions.FindAction("Attack Left").performed += OnAttack;
		}
		else if(handSide.ToLower() == "right")
		{
			inputComponent.actions.FindAction("Attack Right").performed += OnAttack;
		}
		else
		{
			Debug.Log("Error: Hand Side Not Selected!");
		}

		m_handHomeTransform = homeLocation;
		m_playerController = characterController;

		StartCheckCoroutine();

	}

	private void StartCheckCoroutine()
	{
		if (C_IsChecking) return;

		C_IsChecking = true;

		if (C_Checking == null)
		{
			C_Checking = StartCoroutine(CheckHandPosition());
		}
	}

	private void StopCheckCoroutine()
	{
		if (C_IsChecking)
		{
			C_IsChecking = false;

			if (C_Checking != null)
			{
				StopCoroutine(C_Checking);
				C_Checking = null;
			}
		}
	}

	private IEnumerator CheckHandPosition()
	{
		while (true)
		{
			if (Vector2.Distance(this.transform.position, m_handHomeTransform.position) > 0.01f)
			{
				if (m_handRB.velocity.magnitude < 1f)
				{
					StartLerpCoroutine(m_handHomeTransform.position, 0.05f);
				}
			}

			if (Vector2.Distance(this.transform.position, m_handHomeTransform.position) < 0.01f)
			{
				StopLerpCoroutine();
			}

			if (Vector2.Distance(this.transform.position, m_handHomeTransform.position) < 0.5f)
			{
				this.transform.rotation = m_playerController.transform.rotation;
			}

			yield return new WaitForFixedUpdate();
		}
	}

	private void StartLerpCoroutine(Vector2 targetLocation, float duration)
	{
		if (C_IsLerping) return;

		C_IsLerping = true;

		if (C_Lerping == null)
		{
			C_Lerping = StartCoroutine(LerpPosition(targetLocation, duration));
		}
	}

	private void StopLerpCoroutine()
	{
		if (C_IsLerping)
		{
			C_IsLerping = false;

			if (C_Lerping != null)
			{
				StopCoroutine(C_Lerping);
				C_Lerping = null;
			}
		}
	}

	private IEnumerator LerpPosition(Vector2 targetLocation, float duration)
	{
		float time = 0;
		Vector2 startPosition = this.transform.position;

		while (time < duration)
		{

			this.transform.position = Vector2.Lerp(startPosition, targetLocation, time / duration);
			time += Time.deltaTime;

			yield return new WaitForFixedUpdate();
		}

		StopLerpCoroutine();
	}

	void OnAttack(InputAction.CallbackContext ctx)
	{
		if(canAttack) StartCoroutine(Attack());
	}

	IEnumerator Attack()
	{
		m_handRB.velocity = new Vector2(0, 0);
		m_handRB.AddForce(transform.up * m_punchForce, ForceMode2D.Impulse);

		canAttack = false;

		yield return new WaitForSeconds(m_attackCooldown);

		canAttack = true;

	}
}

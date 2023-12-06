using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
	[SerializeField] private GameObject m_playerHandPrefab;
	[SerializeField] private float m_punchForce;

	private GameObject m_playerHand;
	private Rigidbody2D m_handRB;
	private Transform m_handHomeTransform;

	private Coroutine C_Lerping;
	private bool C_IsLerping = false;

	private void Awake()
	{
		m_handHomeTransform = this.transform.GetChild(0).transform;
		Debug.Log(m_handHomeTransform.position);

		m_playerHand = Instantiate(m_playerHandPrefab, transform.position + m_handHomeTransform.position, m_handHomeTransform.rotation);

		m_handRB = m_playerHand.GetComponent<Rigidbody2D>();
	}

	public void Init(PlayerInput inputComponent)
	{
		inputComponent.actions.FindAction("Throw").performed += OnThrow;
		inputComponent.actions.FindAction("Attack").performed += OnAttack;
	}

	private void Update()
	{
		

		if (Vector2.Distance(m_playerHand.transform.position, m_handHomeTransform.position) > 0.01f)
		{
			if(m_handRB.velocity.magnitude < 1f)
			{
				StartLerpCoroutine(m_handHomeTransform.position, 0.2f);
			}
		}

		if (Vector2.Distance(m_playerHand.transform.position, m_handHomeTransform.position) < 0.01f)
		{
			StopLerpCoroutine();
		}

		if(Vector2.Distance(m_playerHand.transform.position, m_handHomeTransform.position) < 0.5f)
		{
			m_playerHand.transform.rotation = transform.rotation;
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
		Vector2 startPosition = m_playerHand.transform.position;

		while (time < duration)
		{

			m_playerHand.transform.position = Vector2.Lerp(startPosition, targetLocation, time / duration);
			time += Time.deltaTime;

			yield return new WaitForFixedUpdate();
		}

		StopLerpCoroutine();
	}


	void OnThrow(InputAction.CallbackContext ctx)
	{
		Debug.Log("Thowing...");
	}

	void OnAttack(InputAction.CallbackContext ctx)
	{
		Debug.Log("Attacking...");

		m_handRB.velocity = new Vector2(0,0);
		m_handRB.AddForce(transform.up * m_punchForce, ForceMode2D.Impulse);
	}

	IEnumerator AttackCooldown()
	{
		yield return null;
	}
}

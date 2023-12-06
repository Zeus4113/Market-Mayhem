using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
	private GameObject m_playerHand;
	private Transform m_handHomeTransform;

	public void Init(PlayerInput inputComponent)
	{
		inputComponent.actions.FindAction("Throw").performed += OnThrow;
		inputComponent.actions.FindAction("Attack").performed += OnAttack;

		m_playerHand = this.transform.GetChild(0).gameObject;
		m_handHomeTransform = m_playerHand.transform;
	}

	private void Update()
	{
		if (m_playerHand.transform != m_handHomeTransform)
		{
			Vector2.MoveTowards(transform.position, m_handHomeTransform.position, Time.deltaTime);
		}
	}


	void OnThrow(InputAction.CallbackContext ctx)
	{
		Debug.Log("Thowing...");
	}

	void OnAttack(InputAction.CallbackContext ctx)
	{
		Debug.Log("Attacking...");

		Rigidbody2D rb = m_playerHand.GetComponent<Rigidbody2D>();

		rb.AddForce(transform.up * 100f, ForceMode2D.Impulse);
	}

	IEnumerator AttackCooldown()
	{
		yield return null;
	}
}

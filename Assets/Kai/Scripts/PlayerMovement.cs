using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	// Designer Variables
	[SerializeField] private float m_movementSpeed;
	[SerializeField] private float m_rotationSpeed;

	// Vectors
	private Vector2 m_movementInput;
	private Vector2 m_mousePosition;

	// Components
	private Rigidbody2D m_rb;
	private PlayerInput m_playerInputComponent;

	// Movement Coroutine
	private Coroutine c_MoveCoroutine;
	private bool c_IsMoving = false;

	// Rotation Coroutine
	private Coroutine c_RotateCoroutine;
	private bool c_isRotating = false;


	// Init & Event Bindings

	public void Init(PlayerInput playerInputComponent)
    {
		m_playerInputComponent = playerInputComponent;
		m_rb = GetComponent<Rigidbody2D>();

		BindEvents(true);
	}

	private void OnDestroy()
	{
		if (m_playerInputComponent == null) return;

		BindEvents(false);
	}

	public void BindEvents(bool isTrue)
	{
		if(isTrue) 
		{
			m_playerInputComponent.actions.FindAction("Movement").performed += OnBeginMove;
			m_playerInputComponent.actions.FindAction("Movement").canceled += OnEndMove;

			m_playerInputComponent.actions.FindAction("Mouse").performed += OnBeginRotate;
			m_playerInputComponent.actions.FindAction("Mouse").canceled += OnEndRotate;
		}
		else if (!isTrue)
		{
			m_playerInputComponent.actions.FindAction("Movement").performed -= OnBeginMove;
			m_playerInputComponent.actions.FindAction("Movement").canceled -= OnEndMove;

			m_playerInputComponent.actions.FindAction("Mouse").performed -= OnBeginRotate;
			m_playerInputComponent.actions.FindAction("Mouse").canceled -= OnEndRotate;
		}
	}

	// Player Movement

	void OnBeginMove(InputAction.CallbackContext ctx)
	{
		m_movementInput = ctx.ReadValue<Vector2>();
		c_IsMoving = true;

		if (c_MoveCoroutine == null)
		{
			c_MoveCoroutine = StartCoroutine(CMove());
		}
	}
	void OnEndMove(InputAction.CallbackContext ctx)
	{
		m_movementInput = ctx.ReadValue<Vector2>();
		c_IsMoving = false;

		if (c_MoveCoroutine != null)
		{
			StopCoroutine(c_MoveCoroutine);
			c_MoveCoroutine = null;
		}
	}

	IEnumerator CMove()
	{
		while (c_IsMoving) 
		{

			if (m_rb.velocity.magnitude < m_movementSpeed)
			{
				m_rb.AddForce(m_movementInput * m_movementSpeed);
			}

			yield return new WaitForFixedUpdate();
		}

		yield return null;
	}

	// Player Rotation

	void OnBeginRotate(InputAction.CallbackContext ctx)
	{
		m_mousePosition = ctx.ReadValue<Vector2>();
		c_isRotating = true;

		if (c_RotateCoroutine == null)
		{
			c_RotateCoroutine = StartCoroutine(CRotate());
		}
	}
	void OnEndRotate(InputAction.CallbackContext ctx)
	{
		m_mousePosition = ctx.ReadValue<Vector2>();
		c_isRotating = false;

		if (c_RotateCoroutine != null)
		{
			StopCoroutine(c_RotateCoroutine);
			c_RotateCoroutine = null;
		}
	}

	IEnumerator CRotate()
	{
		while (c_isRotating)
		{
			Vector2 mousePositionWorld = new Vector2(Camera.main.ScreenToWorldPoint(m_mousePosition).x, Camera.main.ScreenToWorldPoint(m_mousePosition).y);

			float angleRad = Mathf.Atan2(mousePositionWorld.x - transform.position.x, mousePositionWorld.y - transform.position.y);

			float angleDeg = (180 / Mathf.PI) * angleRad;

			transform.rotation = Quaternion.Euler(0, 0, -angleDeg);

			yield return new WaitForFixedUpdate();
		}

		yield return null;
	}
}

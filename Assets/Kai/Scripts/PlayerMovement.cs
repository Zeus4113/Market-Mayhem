using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float m_movementSpeed;
	[SerializeField] private float m_rotationSpeed;

	private Rigidbody2D m_rb;
	private Vector2 m_movementInput;


	public void Init(PlayerInput playerInputComponent)
    {
        m_rb = GetComponent<Rigidbody2D>();
		playerInputComponent.actions.FindAction("Movement").performed += OnMovement;
	}


    void FixedUpdate()
    {
		if(m_rb.velocity.magnitude < m_movementSpeed)
		{
			m_rb.AddForce(m_movementInput * m_movementSpeed);
		}

		transform.rotation = Quaternion.Euler(0, 0, CalculateDirection());
	}

	public void OnMovement(InputAction.CallbackContext ctx)
	{
		m_movementInput = ctx.ReadValue<Vector2>();
	}

	float CalculateDirection()
	{
		Vector2 mousePosition = Mouse.current.position.ReadValue();

		Vector2 mousePositionWorld = new Vector2(Camera.main.ScreenToWorldPoint(mousePosition).x, Camera.main.ScreenToWorldPoint(mousePosition).y);

		float angleRad = Mathf.Atan2(mousePositionWorld.x - transform.position.x, mousePositionWorld.y - transform.position.y);

		float angleDeg = (180 / Mathf.PI) * angleRad;

		return -angleDeg;
	}


}

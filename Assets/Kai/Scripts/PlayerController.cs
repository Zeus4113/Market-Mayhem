using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private GameObject m_leftHandPrefab;
	[SerializeField] private GameObject m_rightHandPrefab;

	private PlayerInput m_playerInputComponent;
	private PlayerActions m_leftHand;
	private PlayerActions m_rightHand;
	private PlayerMovement m_movement;

	private Transform m_leftHandPosition;
	private Transform m_rightHandPosition;

    private Transform m_leftSwingPosition;
    private Transform m_rightSwingPosition;

    public void Init(PlayerInput playerInputComponent)
	{
		m_playerInputComponent = playerInputComponent;
		m_movement = GetComponent<PlayerMovement>();

		Transform handHolders = this.transform.Find("Hand Positions");
		m_leftHandPosition = handHolders.GetChild(1);
		m_rightHandPosition = handHolders.GetChild(0);

        Transform swingPositions = this.transform.Find("Swing Positions");
        m_leftSwingPosition = swingPositions.GetChild(1);
        m_rightSwingPosition = swingPositions.GetChild(0);

        SetupHands();
		InitialisePlayerComponents();
	}

	private void InitialisePlayerComponents()
	{
		m_movement.Init(m_playerInputComponent);
		m_leftHand.Init(m_playerInputComponent, m_leftHandPosition, m_leftSwingPosition, this, "left");
		m_rightHand.Init(m_playerInputComponent, m_rightHandPosition, m_rightSwingPosition, this, "right");
	}

	private void SetupHands()
	{
		m_leftHand = Instantiate(m_leftHandPrefab, m_leftHandPosition.position, m_leftHandPosition.rotation).GetComponent<PlayerActions>();
		m_rightHand = Instantiate(m_rightHandPrefab, m_rightHandPosition.position, m_rightHandPosition.rotation).GetComponent<PlayerActions>();
	}
}

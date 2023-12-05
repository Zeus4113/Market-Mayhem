using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	[SerializeField] GameObject prefabPlayer;
	[SerializeField] GameObject prefabInputManager;
	[SerializeField] GameObject prefabCamera;

	[SerializeField] Vector2 m_playerSpawnPosition = new Vector2(0,0);
	[SerializeField] float m_cameraZOffset = -10f;

	private GameObject m_player;
	private GameObject m_inputManager;
	private GameObject m_camera;

	private PlayerMovement m_movementComponent;
	private PlayerActions m_actionComponent;

	private InputManager m_inputManagerComponent;
	private PlayerInput m_playerInputComponent;
	private CameraSetter m_cameraSetterComponent;

    void Start()
    {
		SetupInput();
		SetupPlayer();
	}

	void SetupInput()
	{
		// Spawn Input Manager
		m_inputManager = Instantiate(prefabInputManager, new Vector3(0, 0, 0), Quaternion.identity);

		// Grab Components
		m_playerInputComponent = m_inputManager.GetComponent<PlayerInput>();
	}

	void SetupPlayer()
	{
		// Spawn Player and Camera
		m_player = Instantiate(prefabPlayer, new Vector3(m_playerSpawnPosition.x, m_playerSpawnPosition.y, 0f), Quaternion.identity);
		m_camera = Instantiate(prefabCamera, new Vector3(m_playerSpawnPosition.x, m_playerSpawnPosition.y, -10f), Quaternion.identity);

		// Grab Components
		m_movementComponent = m_player.GetComponent<PlayerMovement>();
		m_actionComponent = m_player.GetComponent<PlayerActions>();
		m_cameraSetterComponent = m_camera.GetComponent<CameraSetter>();

		// Initialize Components
		m_cameraSetterComponent.Init(m_player, m_cameraZOffset);
		m_movementComponent.Init(m_playerInputComponent);
		m_actionComponent.Init(m_playerInputComponent);
	}

}

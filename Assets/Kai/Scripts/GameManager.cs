using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct EnemyManagerSetupData
{
	public GameObject enemyPrefab;
	public int enemyCount;
	public float enemySpawnDelay;
}

public struct ScoreManagerSetupData
{
	public GameObject itemPrefab;
	public GameObject breakablePrefab;
}

public struct PlayerData
{
	public GameObject player;
	public GameObject camera;
	public GameObject playerInput;
	public float playerCameraOffset;
}

public class GameManager : MonoBehaviour
{

	// References
	[Header("Manager References")]
	[SerializeField] private ScoreManager m_scoreManager;
	[SerializeField] private UserInterfaceManager m_UIManager;
	[SerializeField] private EnemyManager m_enemyManager;
	[Space(2)]

	[Header("Level Data")]
	[SerializeField] LevelDataScriptableObject m_LevelDataScriptableObject;

	// Transforms
	Transform[] m_breakableTransforms;
	Transform[] m_enemySpawnTransforms;
	Transform m_playerSpawnTransform;

	private Transform m_transformHolder;
 
	// Player Components
	private PlayerController m_playerController;
	private PlayerInput m_playerInputComponent;
	private CameraSetter m_cameraSetterComponent;

	// Data Structures
	private LevelDataScriptableObject levelData;
	private ScoreManagerSetupData scoreManagerSetupData;
	private EnemyManagerSetupData enemyManagerSetupData;
	private PlayerData playerSetupData;

	void Start()
	{
		Init(m_LevelDataScriptableObject);
	}

	public void Init(LevelDataScriptableObject inputData)
	{
		levelData = inputData;
		m_transformHolder = transform.GetChild(0);
		GetTransformData();

		// Score Data
		scoreManagerSetupData.breakablePrefab = inputData.m_breakablePrefab;
		scoreManagerSetupData.itemPrefab = inputData.m_itemPrefab;

		// Enemy Data
		enemyManagerSetupData.enemyPrefab = inputData.m_enemyPrefab;
		enemyManagerSetupData.enemyCount = inputData.m_enemyCount;
		enemyManagerSetupData.enemySpawnDelay = inputData.m_enemySpawnDelay;

		// Player Data
		playerSetupData.player = inputData.m_playerPrefab;
		playerSetupData.camera = inputData.m_cameraPrefab;
		playerSetupData.playerInput = inputData.m_playerInputPrefab;
		playerSetupData.playerCameraOffset = inputData.m_playerCameraOffset;

		if (m_breakableTransforms == null || m_enemySpawnTransforms == null || m_playerSpawnTransform == null) return;

		// Setup Game
		SetupPlayer();
		SetupUIManager();
		SetupScoreManager();
		SetupEnemyManager();

		StartGame();
	}

	void GetTransformData()
	{
		// Player Transform Data
		m_playerSpawnTransform = m_transformHolder.GetChild(0);

		// Breakable Transform Data
		Transform breakableHolder = m_transformHolder.GetChild(1);
		m_breakableTransforms = new Transform[breakableHolder.childCount];
		for (int i = 0; i < breakableHolder.childCount; i++)
		{
			m_breakableTransforms[i] = breakableHolder.GetChild(i);
		}

		// Enemy Spawn Transform Data
		Transform enemyHolder = m_transformHolder.GetChild(2);
		m_enemySpawnTransforms = new Transform[enemyHolder.childCount];
		for (int i = 0; i < enemyHolder.childCount; i++)
		{
			m_enemySpawnTransforms[i] = enemyHolder.GetChild(i);
		}
	}

	void SetupPlayer()
	{
		// Spawn Player and Camera
		playerSetupData.player = Instantiate(levelData.m_playerPrefab, new Vector3(m_playerSpawnTransform.position.x, m_playerSpawnTransform.position.y, 0f), Quaternion.identity);
		playerSetupData.camera = Instantiate(levelData.m_cameraPrefab, new Vector3(m_playerSpawnTransform.position.x, m_playerSpawnTransform.position.y, -10f), Quaternion.identity);
		playerSetupData.playerInput = Instantiate(levelData.m_playerInputPrefab, new Vector3(0, 0, 0), Quaternion.identity);

		// Grab Components
		m_playerController = playerSetupData.player.GetComponent<PlayerController>();
		m_cameraSetterComponent = playerSetupData.camera.GetComponent<CameraSetter>();
		m_playerInputComponent = playerSetupData.playerInput.GetComponent<PlayerInput>();

		// Initialize Components
		m_playerController.Init(m_playerInputComponent);
		m_cameraSetterComponent.Init(playerSetupData.player, playerSetupData.playerCameraOffset);
	}

	void SetupScoreManager()
	{
		if (m_scoreManager == null) return;
		m_scoreManager.Init(scoreManagerSetupData, this, m_breakableTransforms);
	}

	void SetupEnemyManager()
	{
		if (m_enemyManager == null) return;
		m_enemyManager.Init(enemyManagerSetupData, this, m_enemySpawnTransforms);
	}

	void SetupUIManager()
	{
		if (m_UIManager == null) return;
	
		m_UIManager.Init(playerSetupData.camera.GetComponent<Camera>(), this);
	
	}

	void StartGame()
	{
		m_scoreManager.SetupBreakables();
		m_enemyManager.StartSpawnEnemies();
	}

	public EnemyManager GetEnemyManager() { return m_enemyManager; }

	public ScoreManager GetScoreManager() { return m_scoreManager; }

	public UserInterfaceManager GetUIManager() { return m_UIManager; }

}

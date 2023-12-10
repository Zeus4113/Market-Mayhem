using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IManager
{
	// Prefabs
	[SerializeField] private LevelSetupData m_levelSetupData;

	private GameObject m_player;
	private Camera m_camera;

	private UserInterfaceManager m_userInterfaceManager;
	private ScoreManager m_scoreManager;
	private EnemyManager m_enemyManager;

	private SceneData m_coreScene;
	private SceneData m_mainScene;
	private SceneData m_userInterfaceScene;

	private PlayerController m_playerController;
	private PlayerInput m_playerInputComponent;
	

	public struct SceneData
	{
		public Scene m_scene;
		public List<GameObject> m_rootObjects;
	}

    void Start()
    {
		Init();
	}

	void Init()
	{
		if (m_levelSetupData == null) return;

		SetupGameScenes();
		SetupPlayer();
		GetManagerReferences();
		
		if(m_userInterfaceManager != null)
		{
			InitializeUIManager();
		}
		
		if(m_scoreManager != null)
		{
			InitializeScoreManager();
		}

        if (m_enemyManager != null)
        {
			InitializeEnemyManager();
		}

	}

	// Setup Functions

	void SetupGameScenes()
	{
		m_mainScene.m_scene = AddScene("MainScene");
		m_userInterfaceScene.m_scene = AddScene("UIScene");
	}

	void SetupPlayer()
	{
		m_playerInputComponent = GetComponent<PlayerInput>();

		// Spawn Player and Camera
		m_player = Instantiate(m_levelSetupData.GetPlayerPrefab(), new Vector3(m_levelSetupData.GetPlayerPosition().position.x, m_levelSetupData.GetPlayerPosition().position.y, 0f), Quaternion.identity);

		// Grab Components
		m_playerController = m_player.GetComponent<PlayerController>();
		m_camera = m_player.GetComponentInChildren<Camera>();

		// Initialize Components
		m_playerController.Init(m_playerInputComponent);
	}

	void GetManagerReferences()
	{
		m_userInterfaceScene.m_rootObjects = new List<GameObject>();

		m_userInterfaceScene.m_rootObjects.AddRange(m_userInterfaceScene.m_scene.GetRootGameObjects());

		for(int i = 0; i < m_userInterfaceScene.m_rootObjects.Count; i++)
		{
			if (m_userInterfaceScene.m_rootObjects[i].GetComponent<UserInterfaceManager>())
			{
				m_userInterfaceManager = m_userInterfaceScene.m_rootObjects[i].GetComponent<UserInterfaceManager>();
			}
		}

		m_mainScene.m_rootObjects = new List<GameObject>();

		m_mainScene.m_rootObjects.AddRange(m_mainScene.m_scene.GetRootGameObjects());

		for (int i = 0; i < m_mainScene.m_rootObjects.Count; i++)
		{
			if (m_mainScene.m_rootObjects[i].GetComponent<EnemyManager>())
			{
				m_enemyManager = m_mainScene.m_rootObjects[i].GetComponent<EnemyManager>();
			}

			if (m_mainScene.m_rootObjects[i].GetComponent<ScoreManager>())
			{
				m_scoreManager = m_mainScene.m_rootObjects[i].GetComponent<ScoreManager>();
			}
		}
	}

	// Initialization Functions

	void InitializeUIManager()
	{
		m_userInterfaceManager.Init(this, m_camera);
	}

	void InitializeEnemyManager()
	{
		m_enemyManager.Init(this, m_levelSetupData.GetEnemyPrefab(), m_levelSetupData.GetEnemyPositions());
	}

	void InitializeScoreManager()
	{
		m_scoreManager.Init(this, m_levelSetupData.GetBreakablePrefab(), m_levelSetupData.GetBreakablePositions());
	}

	// Scene Management
	Scene AddScene(string sceneName)
	{

		SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		Scene newScene = SceneManager.GetSceneByName(sceneName);

		return newScene;
	}

	void RemoveScene(string sceneName)
	{
		SceneManager.UnloadSceneAsync(sceneName);
	}

	// Getters
	public UserInterfaceManager GetUIManager()
	{
		return m_userInterfaceManager;
	}

	public EnemyManager GetEnemyManager()
	{
		return m_enemyManager;
	}

	public ScoreManager GetScoreManager()
	{
		return m_scoreManager;
	}

}

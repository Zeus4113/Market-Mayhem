using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	// Events
	public delegate void EnemyManagerEvent(int count);
	public event EnemyManagerEvent enemyCountChange;

	// Game Manager
	private GameManager m_gameManager;
	private Transform[] m_spawnPositons;

	List<EnemyController> m_enemyList = new List<EnemyController>();

	Coroutine C_SpawnEnemies;
	bool C_IsSpawning = false;

	private EnemyManagerSetupData enemySetupData;

	public void Init(EnemyManagerSetupData myData, GameManager gm, Transform[] positions)
	{
		enemySetupData = myData;
		m_gameManager = gm;
		m_spawnPositons = positions;
	}

	public void StartSpawnEnemies()
	{
		if (C_IsSpawning) return;

		C_IsSpawning = true;

		if (C_SpawnEnemies != null) return;

		C_SpawnEnemies = StartCoroutine(SpawnEnemies(enemySetupData.enemyCount));
	}

	public void StopSpawnEnemies()
	{
		if (!C_IsSpawning) return;

		C_IsSpawning = false;

		if (C_SpawnEnemies == null) return;

		StopCoroutine(C_SpawnEnemies);
		C_SpawnEnemies = null;
	}

	private IEnumerator SpawnEnemies(int amount)
	{
		while(C_IsSpawning)
		{
			for(int i = 0; i < amount; i++)
			{
				Debug.Log(i);
				Debug.Log(amount);
				SpawnNewEnemy();
				yield return new WaitForSeconds(enemySetupData.enemySpawnDelay);
			}
			StopSpawnEnemies();
		}
	}

	private void SpawnNewEnemy()
	{
		int index = Random.Range(0, m_spawnPositons.Length);

		GameObject newEnemy = Instantiate(enemySetupData.enemyPrefab, m_spawnPositons[index].position, m_spawnPositons[index].rotation);
		EnemyController controller = newEnemy.GetComponent<EnemyController>();

		AddEnemy(controller);
		controller.Init(m_spawnPositons[index], this);
	}

	public void AddEnemy(EnemyController controller)
	{
		m_enemyList.Add(controller);
		enemyCountChange?.Invoke(m_enemyList.Count);
	}

	public void RemoveEnemy(EnemyController controller)
	{
		m_enemyList.Remove(controller);
		enemyCountChange?.Invoke(m_enemyList.Count);

		GameObject enemy = controller.gameObject;
		Destroy(enemy);

	}

	public GameManager GetGameManager()
	{
		return m_gameManager;
	}
}

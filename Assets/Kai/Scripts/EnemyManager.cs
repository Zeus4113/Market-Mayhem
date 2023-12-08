using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	[SerializeField] private Transform[] m_spawnLocations;

	[SerializeField] private int m_spawnCount;
	[SerializeField] private GameObject m_enemyPrefab;

	private Transform[] m_breakableLocations;

	List<EnemyController> m_enemyList = new List<EnemyController>();

	Coroutine C_SpawnEnemies;
	bool C_IsSpawning = false;

	public void Init(Transform[] breakableLocations)
	{
		m_breakableLocations = breakableLocations;

		if(m_breakableLocations.Length > 0)
		{
			StartSpawnEnemies();
		}
	}

	void StartSpawnEnemies()
	{
		if (C_IsSpawning) return;

		C_IsSpawning = true;

		if (C_SpawnEnemies != null) return;

		C_SpawnEnemies = StartCoroutine(SpawnEnemies(m_spawnCount));
	}

	void StopSpawnEnemies()
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
				int index = Random.Range(0, m_spawnLocations.Length);

				GameObject newEnemy = Instantiate(m_enemyPrefab, m_spawnLocations[index].position, m_spawnLocations[index].rotation);
				EnemyController controller = newEnemy.GetComponent<EnemyController>();

				AddEnemy(controller);
				controller.Init(m_breakableLocations[Random.Range(0, m_breakableLocations.Length)], m_spawnLocations[index], this);

				yield return new WaitForSeconds(0.1f);
			}

			StopSpawnEnemies();
		}
	}

	public Transform GetNewBreakable()
	{
		bool isRunning = true;
		while (isRunning)
		{
			Transform newLocation = m_breakableLocations[Random.Range(0, m_breakableLocations.Length)];

			if (newLocation.GetComponent<ItemStore>().CheckRemainingItems())
			{
				return m_breakableLocations[Random.Range(0, m_breakableLocations.Length)];
			}

			int count = 0;

			for (int i = 0; i < m_breakableLocations.Length; i++)
			{
				Debug.Log(count);

				if (!m_breakableLocations[i].GetComponent<ItemStore>().CheckRemainingItems())
				{
					count++;
				}

				if(count == m_breakableLocations.Length - 1)
				{
					isRunning = false;
				}
			}
		}

		return null;
	}

	public void AddEnemy(EnemyController controller)
	{
		m_enemyList.Add(controller);
	}

	public void RemoveEnemy(EnemyController controller)
	{
		m_enemyList.Remove(controller);

		GameObject enemy = controller.gameObject;
		Destroy(enemy);

	}
}

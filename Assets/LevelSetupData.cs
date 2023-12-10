using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetupData : MonoBehaviour
{
	[Header("Enemy Data")]
	[SerializeField] private Transform[] m_enemySpawnPositions;
	[SerializeField] private GameObject m_enemyPrefab;
	[SerializeField] private int m_enemySpawnCount;

	public Transform[] GetEnemyPositions() { return m_enemySpawnPositions; }
	public GameObject GetEnemyPrefab() {  return m_enemyPrefab; }
	public int GetEnemySpawnCount() {  return m_enemySpawnCount; }

	[Space(2)]



	[Header("Breakable Data")]
	[SerializeField] private Transform[] m_breakableSpawnPositions;
	[SerializeField] private GameObject m_breakablePrefab;

	public Transform[] GetBreakablePositions() { return m_breakableSpawnPositions; }
	public GameObject GetBreakablePrefab() { return m_breakablePrefab; }

	[Space(2)]



	[Header("Player Data")]
	[SerializeField] private Transform m_playerSpawnPosition;
	[SerializeField] private GameObject m_playerPrefab;

	public Transform GetPlayerPosition() { return m_playerSpawnPosition; }
	public GameObject GetPlayerPrefab() { return m_playerPrefab; }
}

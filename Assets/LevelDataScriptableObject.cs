using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelDataScriptableObject", order = 2)]
public class LevelDataScriptableObject : ScriptableObject
{
	[Header("Player Data")]

	public GameObject m_playerPrefab;
	public GameObject m_cameraPrefab;
	public GameObject m_playerInputPrefab;
	public float m_playerCameraOffset;

	[Header("Score Data")]
	public GameObject m_breakablePrefab;
	public GameObject m_itemPrefab;
	[Space(2)]

	[Header("Enemy Data")]
	public GameObject m_enemyPrefab;
	public int m_enemyCount;
	public float m_enemySpawnDelay;
}

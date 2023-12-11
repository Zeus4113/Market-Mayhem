using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
	[SerializeField] private GameObject m_deadEnemy;
	[SerializeField] private float m_maxHealth;
	private float m_currentHealth;

	void Start()
	{
		m_currentHealth = m_maxHealth;
	}

	public void SetHealth(float health)
	{
		m_currentHealth = health;
	}

	public void Damage(float damage)
	{
		m_currentHealth -= damage;

		if (m_currentHealth < 0)
		{
			m_currentHealth = 0;
		}

		if (!IsAlive())
		{
			if(m_deadEnemy != null)
			{
				Instantiate(m_deadEnemy, transform.position, transform.rotation);
			}

			if (GetComponent<EnemyController>())
			{
				EnemyController enemyController = GetComponent<EnemyController>();
				enemyController.DropItem();
				enemyController.GetEnemyManager().RemoveEnemy(enemyController);
			}
		}
	}

	public bool IsAlive()
	{
		if (m_currentHealth <= 0) return false;
		else return true;
	}
}

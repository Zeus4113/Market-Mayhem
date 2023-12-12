using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
	[SerializeField] private GameObject m_deadEnemy;
    [SerializeField] private GameObject m_deadEnemyTwo;
    [SerializeField] private GameObject m_deadEnemyNoHead;
    [SerializeField] private GameObject m_deadEnemyNoHeadTwo;
    [SerializeField] private float m_maxHealth;
	private float m_currentHealth;
	private int i;

	private GameObject m_spawnedDeadEnemy;

	void Start()
	{
		m_currentHealth = m_maxHealth;
	}

	public void SetHealth(float health)
	{
		m_currentHealth = health;
	}

	public void Damage(float damage, string damageType)
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
				switch (damageType)
				{

					default:
						i = Random.Range(0, 2);
						switch (i)
						{
							case 0:
								m_spawnedDeadEnemy = m_deadEnemy;
								break;
							case 1:
								m_spawnedDeadEnemy = m_deadEnemyTwo;
								break;
						}             
						break;

					case "blunt":
                        i = Random.Range(0, 4);
						Debug.Log("blunt "+ i);
                        switch (i)
                        {
                            case 0:
                                m_spawnedDeadEnemy = m_deadEnemy;
                                break;
                            case 1:
                                m_spawnedDeadEnemy = m_deadEnemyTwo;
                                break;
                            case 2:
                                m_spawnedDeadEnemy = m_deadEnemyNoHead;
                                break;
                            case 3:
                                m_spawnedDeadEnemy = m_deadEnemyNoHeadTwo;
                                break;
                        }
                        break;
                }
                Instantiate(m_spawnedDeadEnemy, transform.position, transform.rotation);
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

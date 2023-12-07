using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
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

		if(m_currentHealth < 0)
		{
			m_currentHealth = 0;
		}

		Debug.Log(gameObject + m_currentHealth.ToString());

		if (!IsAlive())
		{
			Debug.Log("Dead");
			Destroy(this.gameObject);
		}
	}

	public bool IsAlive()
	{
		if (m_currentHealth <= 0) return false;
		else return true;
	}
}

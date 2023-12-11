using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
	[SerializeField] private float m_damageMultiplier = 10f;
	[SerializeField] private float m_forceMultiplier = 10f;
	[SerializeField] private GameObject m_hitParticlesObject;

	private ParticleSystem m_hitParticles;
	private Rigidbody2D m_rigidbody;

	private void Start()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
		if (m_hitParticlesObject) SetHitParicles(m_hitParticlesObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision == null) return;

		if(collision.GetComponent<IDamageable>() == null) return;

		Rigidbody2D rigidbody2D = collision.GetComponent<Rigidbody2D>();
		IDamageable damageable = collision.GetComponent<IDamageable>();
		EnemyController controller = collision.GetComponent<EnemyController>();

		//Debug.Log("Damage: " + m_rigidbody.velocity.magnitude * m_damageMultiplier);
		damageable.Damage(m_rigidbody.velocity.magnitude * m_damageMultiplier);

		Vector2 force = new Vector2(m_rigidbody.velocity.x * m_forceMultiplier, m_rigidbody.velocity.y * m_forceMultiplier);

		//Debug.Log(force);

		if (m_hitParticlesObject)
		{
			m_hitParticles.transform.position = transform.position;
			m_hitParticles.Play();
		}

		if (controller)
		{
			controller.SetEnemyState(EnemyStates.Stunned);
		}

		if(rigidbody2D)
		{
			rigidbody2D.AddForce(force, ForceMode2D.Impulse);
		}

	}

	public void SetDamageStats(float damage, float knockback)
	{
		m_damageMultiplier = damage;
		m_forceMultiplier = knockback;
    }

	public void SetRB(Rigidbody2D RB)
	{
		m_rigidbody = RB;
	}

	public void SetHitParicles(GameObject hitParticles)
	{
		m_hitParticles = Instantiate(hitParticles, transform.position, transform.rotation).GetComponent<ParticleSystem>();
	}
}

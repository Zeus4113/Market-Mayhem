using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
	[SerializeField] private float m_damageMultiplier = 10f;
	[SerializeField] private float m_forceMultiplier = 10f;

	private Rigidbody2D m_rigidbody;

	private void Start()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision == null) return;

		if(collision.GetComponent<IDamageable>() == null) return;

		Rigidbody2D rigidbody2D = collision.GetComponent<Rigidbody2D>();
		IDamageable damageable = collision.GetComponent<IDamageable>();
		EnemyController controller = collision.GetComponent<EnemyController>();

		Debug.Log("Damage: " + m_rigidbody.velocity.magnitude * m_damageMultiplier);
		damageable.Damage(m_rigidbody.velocity.magnitude * m_damageMultiplier);

		Vector2 force = new Vector2(m_rigidbody.velocity.x * m_forceMultiplier, m_rigidbody.velocity.y * m_forceMultiplier);

		Debug.Log(force);

		controller.SetEnemyState(EnemyStates.Stunned);
		rigidbody2D.AddForce(force, ForceMode2D.Impulse);

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
	[SerializeField] private float m_damage = -10;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision == null) return;

		if(collision.GetComponent<IDamageable>() == null) return;

		IDamageable damageable = collision.GetComponent<IDamageable>();

		damageable.Damage(m_damage);
	}
}

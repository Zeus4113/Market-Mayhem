using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
	[SerializeField] private float m_damage = 10f;
	[SerializeField] private float m_forceMultiplier = 10f;
    [SerializeField] private GameObject m_hitParticlesObject;
    [SerializeField] private AudioClip m_hitAudioClip;

	private ParticleSystem m_hitParticles;
	private Rigidbody2D m_rigidbody;
	private bool m_attacking = false;
	private AudioSource m_audioSource;
	private string m_damageType;

	private void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
        m_audioSource = GetComponent<AudioSource>();
		if (m_hitParticlesObject) SetHitParicles(m_hitParticlesObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision == null) return;

        if (collision.GetComponent<IDamageable>() == null) return;

        if (!m_attacking) return;

		Rigidbody2D rigidbody2D = collision.GetComponent<Rigidbody2D>();
		IDamageable damageable = collision.GetComponent<IDamageable>();
		EnemyController controller = collision.GetComponent<EnemyController>();

		Debug.Log(this.gameObject.name + " Damage: " + m_damage
			);
		damageable.Damage(m_damage, m_damageType);

		Vector2 force = new Vector2(m_rigidbody.velocity.x * m_forceMultiplier, m_rigidbody.velocity.y * m_forceMultiplier);

		//Debug.Log(force);

		if (m_hitParticles)
		{
			m_hitParticles.transform.position = transform.position;
			m_hitParticles.Play();
		}

		if (m_hitAudioClip)
		{
			m_audioSource.clip = m_hitAudioClip;
			m_audioSource.Play();
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

	public void SetDamageStats(float damage, float knockback, string damageType)
	{
		m_damage = damage;
		m_forceMultiplier = knockback;
		m_damageType = damageType;
    }

	public void SetRB(Rigidbody2D RB)
	{
		m_rigidbody = RB;
	}

	public void SetHitParicles(GameObject hitParticles)
	{
		m_hitParticles = Instantiate(hitParticles, transform.position, transform.rotation).GetComponent<ParticleSystem>();
	}

	public void setHitAudio(AudioClip hitAudio)
	{
		m_hitAudioClip = hitAudio;
	}

	public void SetAttacking(bool attacking)
	{
		m_attacking = attacking;
	}
}

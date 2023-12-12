using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_damage = 40;
    [SerializeField] private float m_knockback = 20;
    [SerializeField] private string m_damageType;

    private Rigidbody2D m_RB;
    private Coroutine C_LifeSpan;
    private bool m_alive = false;
    private CollisionDamage m_damageComponent;

    private void Awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_damageComponent = GetComponent<CollisionDamage>();
        m_damageComponent.SetAttacking(true);
        m_damageComponent.SetDamageStats(m_damage, m_knockback, m_damageType);
    }
    public void FireProjectile(Transform fireTransform, float fireSpeed)
    {
        transform.position = fireTransform.position;
        transform.rotation = fireTransform.rotation;

        m_RB.AddForce(transform.up * fireSpeed, ForceMode2D.Impulse);
        C_LifeSpan = StartCoroutine(LifeSpan());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopLifeSpan();
    }

    private void StopLifeSpan()
    {
        if (m_alive)
        {
            m_alive = false;
            if (C_LifeSpan != null)
            {
                StopCoroutine(C_LifeSpan);
                C_LifeSpan = null;
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator LifeSpan()
    {
        m_alive = true;
        yield return new WaitForSeconds(3);
        StopLifeSpan();
    }
}

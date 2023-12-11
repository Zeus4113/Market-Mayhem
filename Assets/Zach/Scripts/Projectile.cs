using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D m_RB;
    private Coroutine C_LifeSpan;
    private bool m_alive = false;

    private void Awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
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
        Debug.Log(collision.otherCollider.name);
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
                this.gameObject.SetActive(false);
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

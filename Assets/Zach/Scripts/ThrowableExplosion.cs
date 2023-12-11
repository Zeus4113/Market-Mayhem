using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableExplosion : MonoBehaviour
{
    private ParticleSystem m_explodeParticles;
    private bool m_canExplode = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_canExplode)
        {
            m_explodeParticles.transform.position = transform.position;
            m_explodeParticles.Play();
            Destroy(this.gameObject);
        }     
    }

    public void SetParticles(GameObject particles)
    {
        m_explodeParticles = Instantiate(particles, transform.position, transform.rotation).GetComponent<ParticleSystem>(); 
    }

    public void SetCanExplode(bool canExplode)
    {
        m_canExplode = canExplode;
    }
}
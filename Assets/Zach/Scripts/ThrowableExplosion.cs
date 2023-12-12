using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableExplosion : MonoBehaviour
{
    private ParticleSystem m_explodeParticles;
    private bool m_canExplode = false;
    private Throwable m_throwable;

    private void Awake()
    {
        m_throwable = GetComponent<Throwable>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_canExplode)
        {
            m_explodeParticles.transform.position = transform.position;
            m_explodeParticles.Play();

            StartCoroutine(DestroyAfterSound());
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

    private IEnumerator DestroyAfterSound()
    {
        m_throwable.PlayHitAudio();
        m_throwable.gameObject.layer = LayerMask.NameToLayer("IgnoreAll");
        m_throwable.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}

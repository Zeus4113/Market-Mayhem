using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private Rigidbody2D m_playerHand;
    bool m_pickedUp = false;
    bool m_canPickUp = true;
    Quaternion m_weaponRot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() == null || collision.GetComponent<PlayerActions>().GetHolding() || !m_canPickUp || m_pickedUp) return;
        m_playerHand = collision.GetComponent<Rigidbody2D>();

        if (m_canPickUp) m_pickedUp = true;
        collision.GetComponent<PlayerActions>().SetHolding(true);
    }
 
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && m_pickedUp) Throw();

        if (m_pickedUp)
        {
            this.transform.position = m_playerHand.transform.position;

            m_weaponRot = m_playerHand.transform.rotation;
            m_weaponRot *= Quaternion.Euler(Vector3.forward * 90);
            this.transform.rotation = m_weaponRot;
        }


    }

    private void Throw()
    {
        m_pickedUp = false;
        this.GetComponent<Rigidbody2D>().AddForce(m_playerHand.transform.up * 7, ForceMode2D.Impulse);
        this.GetComponent<Rigidbody2D>().AddTorque(-20f);

        m_playerHand.GetComponent<PlayerActions>().SetHolding(false);
        DelayPickup();
        m_canPickUp = true;
    }

    private IEnumerator DelayPickup()
    {
        yield return new WaitForSeconds(1);
    }
}

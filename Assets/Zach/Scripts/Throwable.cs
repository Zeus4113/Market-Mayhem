using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private Rigidbody2D m_playerRB;
    private Rigidbody2D m_RB;
    bool m_pickedUp = false;
    bool m_canPickUp = true;
    Quaternion m_weaponRot;
    Vector3 m_throwDirection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_RB = GetComponent<Rigidbody2D>();
        m_playerRB = collision.GetComponent<Rigidbody2D>();

        if(m_canPickUp) m_pickedUp = true;
    }

    private void LateUpdate()
    {
        if (m_pickedUp)
        {
            m_RB.transform.position = m_playerRB.transform.position;

            m_weaponRot = m_playerRB.transform.rotation;
            m_weaponRot *= Quaternion.Euler(Vector3.forward * 90);
            m_RB.transform.rotation = m_weaponRot;
        }
    }

    private void Throw()
    {
        m_throwDirection = m_playerRB.transform.up;
        m_pickedUp = false;
        m_canPickUp = false;
        m_RB.AddForce(new Vector2(m_throwDirection.x, m_throwDirection.y) * 10);
    }
}

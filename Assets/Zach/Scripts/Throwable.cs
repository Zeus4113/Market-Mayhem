using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Throwable : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject weaponData;


    private Rigidbody2D m_playerHand;
    bool m_pickedUp = false;
    bool m_canPickUp = true;
    Quaternion m_weaponRot;
    PlayerInput m_playerInput;
    string m_handSide;
    int m_durability;
    float m_handleOffset;
    float m_rotationOffset;

    private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Init(WeaponScriptableObject myWeaponData)
    {
        weaponData = myWeaponData;

        m_spriteRenderer.sprite = weaponData.m_spriteUnequipped;
        m_handleOffset = weaponData.m_handleOffset;
        m_rotationOffset = weaponData.m_rotationOffset;
        m_durability = weaponData.m_durability;
    }

    public void SetAttached(PlayerInput playerInputComponent, string HandSide)
    {
        m_playerInput = playerInputComponent;
        m_handSide = HandSide.ToLower();
        m_spriteRenderer.sprite = weaponData.m_spriteEquipped;
        if (m_durability <= 0) WeaponDestroyed();
    }

    private void WeaponDestroyed()
    {
        m_canPickUp = false;
        switch (m_handSide)
        {
            case "left":
                m_playerInput.actions.FindAction("Attack Left").performed += Throw;
                m_playerHand.GetComponent<PlayerActions>().BindEvents(false);
                break;

            case "right":
                m_playerInput.actions.FindAction("Attack Right").performed += Throw;
                m_playerHand.GetComponent<PlayerActions>().BindEvents(false);
                break;
        }
    }

    private void OnDropped()
    {
        m_pickedUp = false;
        switch (m_handSide)
        {
            case "left":
                m_playerInput.actions.FindAction("Attack Left").performed -= Throw;
                m_playerHand.GetComponent<PlayerActions>().BindEvents(true);
                break;

            case "right":
                m_playerInput.actions.FindAction("Attack Right").performed -= Throw;
                m_playerHand.GetComponent<PlayerActions>().BindEvents(true);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == null) return;

        if (!collision.GetComponent<Rigidbody2D>()) return;

        if (!collision.GetComponent<PlayerActions>()) return;

        if (collision.GetComponent<PlayerActions>().GetHolding() || !m_canPickUp || m_pickedUp) return;

        m_playerHand = collision.GetComponent<Rigidbody2D>();

        if (m_canPickUp)
        {
            SetAttached(m_playerHand.GetComponent<PlayerActions>().GetPlayerInputComponent(), m_playerHand.GetComponent<PlayerActions>().GetHandSide());
        }

        if (m_canPickUp) m_pickedUp = true;
        collision.GetComponent<PlayerActions>().SetHolding(true, this.gameObject);
        m_pickedUp = true;
    }

    private void LateUpdate()
    {

        if (m_pickedUp)
        {
            this.transform.position = m_playerHand.transform.position;
            m_weaponRot = m_playerHand.transform.rotation;

            switch (m_handSide.ToLower())
            {
                case "left":
                    this.transform.position += m_playerHand.transform.right * m_handleOffset;
                    m_weaponRot *= Quaternion.Euler(Vector3.forward * m_rotationOffset);

                    break;
                case "right":
                    this.transform.position += m_playerHand.transform.right * -m_handleOffset;
                    m_weaponRot *= Quaternion.Euler(Vector3.forward * (m_rotationOffset + 180));

                    break;
            }

            this.transform.rotation = m_weaponRot;
        }

    }

    private void Throw(InputAction.CallbackContext ctx)
    {
        m_pickedUp = false;
        OnDropped();
        //this.transform.parent = null;
        this.GetComponent<Rigidbody2D>().AddForce(m_playerHand.GetComponent<PlayerActions>().GetPlayerController().transform.up * 10, ForceMode2D.Impulse);
        switch (m_handSide.ToLower())
        {
            case "left":
                this.GetComponent<Rigidbody2D>().AddTorque(-30f);
                break;
            case "right":
                this.GetComponent<Rigidbody2D>().AddTorque(30f);
                break;
        }


        m_playerHand.GetComponent<PlayerActions>().SetHolding(false, null);
        StartCoroutine(DelayPickup());
    }

    private IEnumerator DelayPickup()
    {
        yield return new WaitForSeconds(1);
        //m_canPickUp = true;
    }

    public void EditDurability(int DurabilityIncrement)
    {
        m_durability += DurabilityIncrement;
        if (m_durability <= 0) WeaponDestroyed();
    }
}

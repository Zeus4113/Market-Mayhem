using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Throwable : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject weaponData;


    private Rigidbody2D m_playerHand;
    private bool m_pickedUp = false;
    private bool m_canPickUp = true;
    private Quaternion m_weaponRot;
    private PlayerInput m_playerInput;
    private string m_handSide;
    private int m_durability;
    private float m_handleOffset;
    private float m_rotationOffset;
    private Sprite m_spriteDamaged;
    private Sprite m_spriteDestroyed;
    private int m_maxDurability;
    private ParticleSystem m_breakParticles;
    private bool m_explodeOnThrow;
    private Rigidbody2D m_RB;


    private SpriteRenderer m_spriteRenderer;
    private CollisionDamage m_collisionDamage;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_collisionDamage = GetComponent<CollisionDamage>();
        m_RB = GetComponent<Rigidbody2D>();
    }
    public void Init(WeaponScriptableObject myWeaponData)
    {
        weaponData = myWeaponData;

        m_spriteRenderer.sprite = weaponData.m_spriteUnequipped;
        m_handleOffset = weaponData.m_handleOffset;
        m_rotationOffset = weaponData.m_rotationOffset;
        m_durability = weaponData.m_durability;
        m_maxDurability = m_durability;

        m_spriteDamaged = weaponData.m_spriteDamaged;
        m_spriteDestroyed = weaponData.m_spriteDestroyed;
        m_explodeOnThrow = weaponData.m_explodeOnThrow;

        m_breakParticles = Instantiate(weaponData.m_breakParticles, transform.position, transform.rotation).GetComponent<ParticleSystem>();

        m_collisionDamage.SetDamageStats(weaponData.m_damage, weaponData.m_knockback);

        switch (m_explodeOnThrow)
        {
            case true:
                GetComponent<ThrowableExplosion>().SetParticles(weaponData.m_explodeParticles);
                break;
            case false:
                GetComponent<ThrowableExplosion>().enabled = false;
                m_collisionDamage.SetHitParicles(weaponData.m_hitParticles);
                break;
        }
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
        m_spriteRenderer.sprite = m_spriteDestroyed;
        m_canPickUp = false;
        m_collisionDamage.SetRB(m_RB);
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

    private void PlayBreakParticles()
    {
        m_breakParticles.transform.position = transform.position;
        m_breakParticles.Play();
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
        if (m_explodeOnThrow)
        {
            GetComponent<ThrowableExplosion>().SetCanExplode(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == null) return;

        if (!collision.GetComponent<Rigidbody2D>()) return;

        if (!collision.GetComponent<PlayerActions>()) return;

        if (collision.GetComponent<PlayerActions>().GetHolding() || !m_canPickUp || m_pickedUp) return;

        m_playerHand = collision.GetComponent<Rigidbody2D>();
        m_collisionDamage.SetRB(m_playerHand.GetComponent<Rigidbody2D>());

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
        if (m_durability <= 0)
        {
            PlayBreakParticles();
            WeaponDestroyed();
        }
        else if (m_durability == Mathf.Floor(m_maxDurability / 2))
        {
            m_spriteRenderer.sprite = m_spriteDamaged;
            PlayBreakParticles();
        }
    }
}

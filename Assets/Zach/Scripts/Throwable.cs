using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Throwable : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject weaponData;
    [SerializeField] private AudioClip m_punchWoosh;


    private Rigidbody2D m_playerHand;
    private bool m_pickedUp = false;
    private bool m_canPickUp = true;
    private Quaternion m_weaponRot;
    private PlayerInput m_playerInput;
    private string m_handSide;
    private int m_durability;
    private float m_handleOffset;
    private float m_handleVertOffset;
    private float m_rotationOffset;
    private Sprite m_spriteDamaged;
    private Sprite m_spriteDestroyed;
    private int m_maxDurability;
    private ParticleSystem m_breakParticles;
    private bool m_explodeOnThrow;
    private Rigidbody2D m_RB;
    private bool m_ranged;
    private string m_damageType;


    private SpriteRenderer m_spriteRenderer;
    private CollisionDamage m_collisionDamage;
    private ShootingComponent m_shootingComponent;
    private ThrowableExplosion m_throwableExplosion;
    private AudioSource m_audioSource;
    private AudioClip m_hitAudio;
    private AudioClip m_fireAudio;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_collisionDamage = GetComponent<CollisionDamage>();
        m_shootingComponent = GetComponent<ShootingComponent>();
        m_throwableExplosion = GetComponent<ThrowableExplosion>();
        m_RB = GetComponent<Rigidbody2D>();
        m_audioSource = GetComponent<AudioSource>();
    }
    public void Init(WeaponScriptableObject myWeaponData)
    {
        weaponData = myWeaponData;

        m_spriteRenderer.sprite = weaponData.m_spriteUnequipped;
        m_handleOffset = weaponData.m_handleOffset;
        m_handleVertOffset = weaponData.m_handleVertOffset;
        m_rotationOffset = weaponData.m_rotationOffset;
        m_durability = weaponData.m_durability;
        m_maxDurability = m_durability;

        m_spriteDamaged = weaponData.m_spriteDamaged;
        m_spriteDestroyed = weaponData.m_spriteDestroyed;
        m_explodeOnThrow = weaponData.m_explodeOnThrow;
        m_hitAudio = weaponData.m_hitAudio;
        m_ranged = weaponData.m_ranged;
        m_damageType = weaponData.m_damageType;

        m_breakParticles = Instantiate(weaponData.m_breakParticles, transform.position, transform.rotation).GetComponent<ParticleSystem>();

        m_collisionDamage.SetDamageStats(weaponData.m_damage, weaponData.m_knockback, m_damageType);
        m_collisionDamage.setHitAudio(m_hitAudio);

        switch (m_explodeOnThrow)
        {
            case true:
                m_throwableExplosion.SetParticles(weaponData.m_explodeParticles);
                break;
            case false:
                m_throwableExplosion.enabled = false;
                m_collisionDamage.SetHitParicles(weaponData.m_hitParticles);
                break;
        }

        switch (m_ranged)
        {
            case true:
                m_shootingComponent.SetProjectile(weaponData.m_projectilePrefab);
                m_fireAudio = weaponData.m_fireSound;
                m_shootingComponent.SetFireSound(m_fireAudio);
                break;
            case false:
                m_shootingComponent.enabled = false;
                break;
        }
    }

    public void SetAttached(PlayerInput playerInputComponent, string HandSide)
    {
        this.gameObject.layer = LayerMask.NameToLayer("Weapon");
        Debug.Log("Holding");

        m_playerInput = playerInputComponent;
        m_handSide = HandSide.ToLower();
        m_spriteRenderer.sprite = weaponData.m_spriteEquipped;
        if (m_durability <= 0) WeaponDestroyed();

        if (m_ranged)
        {
            m_shootingComponent.BindEvents(true, m_playerInput, m_handSide); 
            m_playerHand.GetComponent<PlayerActions>().BindEvents(false);
            m_shootingComponent.SetPlayerHand(m_playerHand);
        }
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
        if(m_ranged) m_shootingComponent.BindEvents(false, m_playerInput, m_handSide);
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
            m_throwableExplosion.SetCanExplode(true);
        }
        if (m_ranged) m_playerHand.GetComponent<PlayerActions>().BindEvents(true);
        StartCoroutine(LifeSpan());
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
        collision.GetComponent<PlayerActions>().SetHolding(true, this.gameObject, m_ranged);
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
                    this.transform.position += (m_playerHand.transform.right * m_handleOffset);
                    m_weaponRot *= Quaternion.Euler(Vector3.forward * m_rotationOffset);
                    break;
                case "right":
                    this.transform.position += (m_playerHand.transform.right * -m_handleOffset);
                    m_weaponRot *= Quaternion.Euler(Vector3.forward * (m_rotationOffset + 180));
                    break;
            }
            this.transform.position += (m_playerHand.transform.up * m_handleVertOffset);

            this.transform.rotation = m_weaponRot;
        }

    }

    private void Throw(InputAction.CallbackContext ctx)
    {
        m_pickedUp = false;
        OnDropped();
        //this.transform.parent = null;
        this.GetComponent<Rigidbody2D>().AddForce(m_playerHand.GetComponent<PlayerActions>().GetPlayerController().transform.up * 10, ForceMode2D.Impulse);
        m_collisionDamage.SetAttacking(true);

		if(m_audioSource != null)
		{
            m_audioSource.clip = m_punchWoosh;
            m_audioSource.Play();
		}

        switch (m_handSide.ToLower())
        {
            case "left":
                this.GetComponent<Rigidbody2D>().AddTorque(-30f);
                break;
            case "right":
                this.GetComponent<Rigidbody2D>().AddTorque(30f);
                break;
        }


        m_playerHand.GetComponent<PlayerActions>().SetHolding(false, null, m_ranged);
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

    private IEnumerator DisableCollision()
    {
        yield return new WaitForSeconds(3);
        this.gameObject.layer = LayerMask.NameToLayer("IgnoreAll");
    }

    private IEnumerator LifeSpan()
    {
        StartCoroutine(DisableCollision());
        yield return new WaitForSeconds(30);
        Destroy(this.gameObject);
    }

    public void PlayHitAudio()
    {
        m_audioSource.clip = m_hitAudio;
        m_audioSource.Play();
    }
}

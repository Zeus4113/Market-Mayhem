using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
	[SerializeField] private float m_punchForce;
	[SerializeField] private float m_attackCooldown;
	[SerializeField] private Sprite m_emptyHandSprite;
	[SerializeField] private Sprite m_holdingHandSprite;
	[SerializeField] private GameObject m_punchAnimator;
	[SerializeField] private AudioClip m_punchSwooshAudioClip;

    private PlayerController m_playerController;
	private Rigidbody2D m_handRB;
	private Transform m_handHomeTransform;
	private Transform m_savedHandHomeTransform;
	private Transform m_savedSwingTransform;

	private string m_handSide;
	private PlayerInput m_playerInput;

    private Coroutine C_Lerping;
	private bool C_IsLerping = false;

	private Coroutine C_Checking;
	private bool C_IsChecking = false;

	private bool canAttack = true;

	private bool m_isHolding = false;
	private GameObject m_heldItem;

	private Sprite m_handSprite;
	private Rigidbody2D m_punchAnimRB;

	private int m_handSideMultiplier;

	private CollisionDamage m_damageComponent;
	private AudioSource m_audioSource;

	private bool m_ranged = false;

	private void Awake()
	{
		m_handRB = this.GetComponent<Rigidbody2D>();
		m_punchAnimator = Instantiate(m_punchAnimator, transform.position, transform.rotation);
		m_punchAnimator.SetActive(false);
		m_punchAnimRB = m_punchAnimator.GetComponent<Rigidbody2D>();
		m_damageComponent = GetComponent<CollisionDamage>();
        m_audioSource = GetComponent<AudioSource>();
    }

	public void Init(PlayerInput inputComponent, Transform homeLocation, Transform swingPosition, PlayerController characterController, string handSide)
	{
        m_handSide = handSide.ToLower();
		m_playerInput = inputComponent;
		m_handHomeTransform = homeLocation;
		m_savedHandHomeTransform = homeLocation;
		m_savedSwingTransform = swingPosition;
		m_playerController = characterController;

		switch (m_handSide)
		{
			case "left":
				m_handSideMultiplier = 1;
				break;
			case "right":
                m_handSideMultiplier = -1;
                break;
		}

		BindEvents(true);
		StartCheckCoroutine();
	}

	public PlayerInput GetPlayerInputComponent()
	{
		return m_playerInput;
	}

	public string GetHandSide()
	{
		return m_handSide;
	}

	public void BindEvents(bool isTrue)
	{
		switch (isTrue)
		{
			case true:
                if (m_handSide.ToLower() == "left")
                {
                    m_playerInput.actions.FindAction("Attack Left").performed += OnAttack;
                }
                else if (m_handSide.ToLower() == "right")
                {
                    m_playerInput.actions.FindAction("Attack Right").performed += OnAttack;
                }
                else
                {
                    Debug.Log("Error: Hand Side Not Selected!");
                }
                break;

			case false:
                if (m_handSide.ToLower() == "left")
                {
                    m_playerInput.actions.FindAction("Attack Left").performed -= OnAttack;
                }
                else if (m_handSide.ToLower() == "right")
                {
                    m_playerInput.actions.FindAction("Attack Right").performed -= OnAttack;
                }
                else
                {
                    Debug.Log("Error: Hand Side Not Selected!");
                }
                break;
		}
	}

	private void StartCheckCoroutine()
	{
		if (C_IsChecking) return;

		C_IsChecking = true;

		if (C_Checking == null)
		{
			C_Checking = StartCoroutine(CheckHandPosition());
		}
	}

	private void StopCheckCoroutine()
	{
		if (C_IsChecking)
		{
			C_IsChecking = false;

			if (C_Checking != null)
			{
				StopCoroutine(C_Checking);
				C_Checking = null;
			}
		}
	}

	private IEnumerator CheckHandPosition()
	{
		while (true)
		{
			if (Vector2.Distance(this.transform.position, m_handHomeTransform.position) > 0.01f)
			{
				if (m_handRB.velocity.magnitude < 1f)
				{
					StartLerpCoroutine(m_handHomeTransform.position, 0.05f);
				}
			}

			if (Vector2.Distance(this.transform.position, m_handHomeTransform.position) < 0.01f)
			{
				StopLerpCoroutine();
			}

			if (Vector2.Distance(this.transform.position, m_handHomeTransform.position) < 0.5f)
			{
                this.transform.rotation = m_playerController.transform.rotation;
                switch (m_isHolding && !m_ranged)
				{
					case true:
                        this.transform.rotation *= Quaternion.Euler(m_playerController.transform.forward * 90 * m_handSideMultiplier);
                        break;
					case false:
                        break;
				}
			}

			yield return new WaitForFixedUpdate();
		}
	}

	private void StartLerpCoroutine(Vector2 targetLocation, float duration)
	{
		if (C_IsLerping) return;

		C_IsLerping = true;

		if (C_Lerping == null)
		{
			C_Lerping = StartCoroutine(LerpPosition(targetLocation, duration));
            m_handRB.angularDrag = 0.05f;
        }
	}

	private void StopLerpCoroutine()
	{
		if (C_IsLerping)
		{
			C_IsLerping = false;

			if (C_Lerping != null)
			{
				StopCoroutine(C_Lerping);
				C_Lerping = null;
			}
		}
	}

	private IEnumerator LerpPosition(Vector2 targetLocation, float duration)
	{
		float time = 0;
		Vector2 startPosition = this.transform.position;

		while (time < duration)
		{

			this.transform.position = Vector2.Lerp(startPosition, targetLocation, time / duration);
			time += Time.deltaTime;

			yield return new WaitForFixedUpdate();
		}

		StopLerpCoroutine();
	}

	void OnAttack(InputAction.CallbackContext ctx)
	{
		if(canAttack) StartCoroutine(Attack());
	}

	IEnumerator Attack()
	{
        m_handRB.velocity = new Vector2(0, 0);
        m_handRB.angularVelocity = 0;
        switch (m_isHolding)
		{
			case true:       
                this.transform.position = m_savedSwingTransform.position;
                m_handRB.angularDrag = 10;
				switch (m_handSide.ToLower())
				{
					case "left":
                        this.transform.rotation *= Quaternion.Euler(m_playerController.transform.forward * 90 * m_handSideMultiplier);
                        m_handRB.AddForce((m_playerController.transform.up + m_playerController.transform.right/3) * m_punchForce, ForceMode2D.Impulse);
                        m_handRB.AddTorque(-1000f);
                        break;
					case "right":
                        this.transform.rotation *= Quaternion.Euler(m_playerController.transform.forward * 90 * m_handSideMultiplier);
                        m_handRB.AddForce((m_playerController.transform.up - m_playerController.transform.right/3) * m_punchForce, ForceMode2D.Impulse);
                        m_handRB.AddTorque(1000f);
                        break;
				}
                m_heldItem.GetComponent<Throwable>().EditDurability(-1);
                break;

			case false:
				StartCoroutine(PlayAnimation());
                m_handRB.AddForce(transform.up * m_punchForce, ForceMode2D.Impulse);
				break;
        }
        canAttack = false;
		if (!m_isHolding) m_damageComponent.SetAttacking(true);
		else if (m_isHolding && m_heldItem != null) m_heldItem.GetComponent<CollisionDamage>().SetAttacking(true);

		m_audioSource.clip = m_punchSwooshAudioClip;
		m_audioSource.Play();

        yield return new WaitForSeconds(m_attackCooldown);

        canAttack = true;
        m_damageComponent.SetAttacking(false);

        if (m_isHolding && m_heldItem != null) m_heldItem.GetComponent<CollisionDamage>().SetAttacking(false);
    }

    private IEnumerator PlayAnimation()
	{
		m_punchAnimator.transform.position = this.transform.position;
        m_punchAnimator.transform.rotation = this.transform.rotation;
		m_punchAnimRB.AddForce(transform.up * m_punchForce, ForceMode2D.Impulse);

        m_punchAnimator.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        m_punchAnimator.SetActive(false);
    }

	public void SetHolding(bool HoldingBool, GameObject Item, bool ranged)
	{
        m_isHolding = HoldingBool;
        m_heldItem = Item;
		m_ranged = ranged;

		switch (m_isHolding && !m_ranged)
		{
			case true:
				GetComponent<SpriteRenderer>().sprite = m_holdingHandSprite;
				m_handHomeTransform = m_savedSwingTransform;
				m_damageComponent.enabled = false;
				break;
			case false:
				GetComponent<SpriteRenderer>().sprite = m_emptyHandSprite;
                m_handHomeTransform = m_savedHandHomeTransform;
                m_damageComponent.enabled = true;
				if (m_ranged) GetComponent<SpriteRenderer>().sprite = m_holdingHandSprite;
                break;
		}
	}

	public bool GetHolding()
	{
		return m_isHolding;
	}

	public PlayerController GetPlayerController()
	{
		return m_playerController;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour, IDamageable
{
	[SerializeField] private const float maxHealth = 100f;
	[SerializeField] private Sprite[] breakableSprites = new Sprite[3];

	private float currentHealth;
	private SpriteRenderer spriteRenderer;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void UpdateSprite()
	{
		switch(currentHealth)
		{
			case >= maxHealth:
				spriteRenderer.sprite = breakableSprites[0];
				break;

			case < maxHealth and > maxHealth / 2:
				spriteRenderer.sprite = breakableSprites[1];
				break;

			case < maxHealth / 2 and > 0:
				spriteRenderer.sprite = breakableSprites[2];
				break;

			case <= 0:
				spriteRenderer.sprite = breakableSprites[3];
				break;
		}
	}

	public void SetHealth(float health)
	{
		currentHealth = health;
	}

	public void Damage(float damage)
	{
		currentHealth += damage;
	}

	public void IsAlive()
	{
		Debug.Log("Health: " + currentHealth);

		if(currentHealth <= 0)
		{
			Debug.Log("Breakable Destroyed");
		}
	}
}

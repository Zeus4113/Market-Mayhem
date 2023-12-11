using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	// Events

	public delegate void OnItemDestroyed();
	public event OnItemDestroyed itemDestroyed;

	[SerializeField] private Sprite[] m_sprites;
	private bool m_isPickedUp = false;

	private void Awake()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = SelectRandomSprite();
	}

	private Sprite SelectRandomSprite()
	{
		Sprite mySprite = m_sprites[Random.Range(0, m_sprites.Length -1)];
		return mySprite;
	}

	public void OnDestroy()
	{
		itemDestroyed?.Invoke();
		Destroy(gameObject);
	}

	public bool IsPickedUp()
	{
		return m_isPickedUp;
	}

	public void SetPickedUp(bool isTrue)
	{
		m_isPickedUp = isTrue;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision == null) return;

		if(collision.GetComponent<EnemyController>() == null) return;

		EnemyController enemyController = collision.GetComponent<EnemyController>();

		if(enemyController.GetPickedItem() != null) return;

		enemyController.AddItem(this);
	}

}	



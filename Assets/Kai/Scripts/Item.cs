using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

	[SerializeField] private Sprite[] m_sprites;

	private bool m_isPickedup = false;

	public delegate void OnItemDestroyed();

	public event OnItemDestroyed itemDestroyed;

	private void Start()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = SelectRandomSprite();
	}

	private Sprite SelectRandomSprite()
	{
		Sprite mySprite;

		mySprite = m_sprites[Random.Range(0, m_sprites.Length -1)];

		return mySprite;
	}

	public void SetPickedUp(bool isPickedup)
	{
		m_isPickedup = isPickedup;
	}

	public bool IsPickedUp()
	{
		return m_isPickedup;
	}

	public void OnDestroy()
	{
		itemDestroyed?.Invoke();
	}
}

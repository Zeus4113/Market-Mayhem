using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRandomiser : MonoBehaviour
{

	[SerializeField] private Sprite[] m_sprites;

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

	public void OnDestroy()
	{
		itemDestroyed?.Invoke();
	}
}
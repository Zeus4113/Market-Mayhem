using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStore : MonoBehaviour
{
	[SerializeField] private GameObject m_itemPrefab;

	private Transform[] m_itemSpawnTransforms;

	List<GameObject> m_items = new List<GameObject>();

	private void Start()
	{
		Transform myTransform = transform.Find("Item Positons");
		m_itemSpawnTransforms = new Transform[myTransform.childCount];

		for (int i = 0; i < myTransform.childCount; i++) 
		{
			m_itemSpawnTransforms[i] = myTransform.GetChild(i).transform;
		}

		for(int i = 0;  i < m_itemSpawnTransforms.Length; i++)
		{
			m_items.Add(Instantiate(m_itemPrefab, m_itemSpawnTransforms[i].position, m_itemSpawnTransforms[i].rotation));
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision == null) return;
		
		if(collision.GetComponent<EnemyController>() == null) return;

		EnemyController enemyController = collision.GetComponent<EnemyController>();

		if(enemyController.GetPickedItem() != null) return;

		if(m_items.Count == 0) return;

		enemyController.AddItem(m_items[m_items.Count-1]);
		m_items.Remove(m_items[m_items.Count - 1]);
		
	}

	public bool CheckRemainingItems()
	{
		if (m_items.Count == 0) return false;
		else return true;
	}
}

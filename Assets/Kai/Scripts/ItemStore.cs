using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStore : MonoBehaviour
{
	[SerializeField] private GameObject m_itemPrefab;

	private Transform[] m_itemSpawnTransforms;

	List<Item> m_items = new List<Item>();

	public void Init()
	{
		Transform myTransform = transform.Find("Item Positions");
		m_itemSpawnTransforms = new Transform[myTransform.childCount];

		for (int i = 0; i < myTransform.childCount; i++) 
		{
			m_itemSpawnTransforms[i] = myTransform.GetChild(i).transform;
		}

		for(int i = 0;  i < m_itemSpawnTransforms.Length; i++)
		{
			m_items.Add(Instantiate(m_itemPrefab.GetComponent<Item>(), m_itemSpawnTransforms[i].position, Quaternion.Euler(0, 0, Random.Range(0, 360))));
		}
	}

	//private void OnTriggerEnter2D(Collider2D collision)
	//{
	//	if (collision == null) return;
		
	//	if(collision.GetComponent<EnemyController>() == null) return;

	//	EnemyController enemyController = collision.GetComponent<EnemyController>();

	//	if(enemyController.GetPickedItem() != null) return;

	//	if(m_items.Count == 0) return;

	//	enemyController.AddItem(m_items[m_items.Count-1]);
	//	m_items.Remove(m_items[m_items.Count - 1]);
		
	//}

	public int GetItemCount()
	{
		return m_items.Count;
	}
	
	public List<Item> GetItems()
	{
		return m_items;
	}
}

using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	// Temp References
	[SerializeField] private Transform m_sliderTransform;
	private Slider m_slider;

	[SerializeField] private NavMeshSurface m_surface;
	

	// Designer Fields
	[SerializeField] private GameObject m_breakablePrefab;
	[SerializeField] private Transform[] m_breakablePositions;
	[SerializeField] private GameObject m_enemyManager;

	// Member Variables
	private int m_numberOfItems;
	private int m_totalItems;
	private float m_cashTotal;
	private float m_pricePerItem;

	// Lists
	private List<Transform> m_breakableList = new List<Transform>();
	private List<GameObject> m_itemList = new List<GameObject>();

	public void Init()
	{
		m_slider = m_sliderTransform.GetComponent<Slider>();
		m_enemyManager.GetComponent<EnemyManager>().Init(SetupBreakables());
	}

	public void DecrementCurrentItems()
	{
		m_numberOfItems--;
		m_slider.value = CalculateBarPercentage();
	}

	private Transform[] SetupBreakables()
	{
		m_numberOfItems = 0;

		for(int i = 0; i < m_breakablePositions.Length; i++) 
		{
			Transform newBreakable = Instantiate(m_breakablePrefab, m_breakablePositions[i].position, m_breakablePositions[i].rotation).transform;

			m_breakableList.Add(newBreakable);

			ItemStore itemStore = newBreakable.GetComponent<ItemStore>();

			itemStore.Init();
			m_numberOfItems += itemStore.GetItemCount();
			m_itemList.AddRange(itemStore.GetItems());
		}

		BindItemEvents();

		m_surface.BuildNavMeshAsync();

		m_totalItems = m_numberOfItems;
		return m_breakableList.ToArray();
	}

	private void BindItemEvents()
	{
		for(int i = 0 ; i < m_itemList.Count ; i++)
		{
			ItemRandomiser itemRandomiser = m_itemList[i].GetComponent<ItemRandomiser>();
			itemRandomiser.itemDestroyed += DecrementCurrentItems;
		}
	}

	private void CalculateValuePerItem()
	{
		m_pricePerItem = m_cashTotal / m_numberOfItems;
	}

	private float CalculateBarPercentage()
	{
		Debug.Log("Number of Items: " + m_numberOfItems);
		Debug.Log("Total Items: " + m_totalItems);

		float barPercentage = 1.0f * m_numberOfItems / m_totalItems;

		Debug.Log(barPercentage);
		return barPercentage;
	}
}

using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ScoreManager : MonoBehaviour, IManager
{
	private GameManager m_gameManager;

	// UI Elements
	private Slider m_progressBar;
	private Transform m_gameOverOverlay;

	// Designer Fields
	[SerializeField] private NavMeshSurface m_surface;
	[SerializeField] private GameObject m_enemyManager;

	// Init Variables
	private GameObject m_breakablePrefab;
	private UserInterfaceManager m_userInterfaceManager;
	private Transform[] m_breakablePositions;

	// Member Variables
	private int m_numberOfItems;
	private int m_totalItems;
	private float m_cashTotal;
	private float m_pricePerItem;

	// Lists
	private List<Transform> m_breakableList = new List<Transform>();
	private List<Item> m_itemList = new List<Item>();

	public void Init(GameManager gm, GameObject prefab, Transform[] positions)
	{
		m_gameManager = gm;
		m_userInterfaceManager = m_gameManager.GetUIManager();
		m_breakablePrefab = prefab;
		m_breakablePositions = positions;

		if (m_userInterfaceManager == null) return;
		if (m_breakablePositions.Length == 0) return;
		if(m_breakablePrefab == null) return;

		Transform myWidget = m_userInterfaceManager.GetWidget("Progress Bar").transform;

		if (myWidget == null) return;
		if (myWidget.GetComponent<Slider>() == null) return;

		m_progressBar = myWidget.GetComponent<Slider>();

		SetupBreakables();
	}

	public void DecrementCurrentItems()
	{
		m_numberOfItems--;
		m_progressBar.value = CalculateBarPercentage();
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

		m_userInterfaceManager.EnableGameOverWidget(false);

		m_totalItems = m_numberOfItems;
		return m_breakableList.ToArray();
	}

	public Item GetItem()
	{
		return m_itemList[Random.Range(0, m_itemList.Count - 1)];
	}

	private void BindItemEvents()
	{
		for(int i = 0 ; i < m_itemList.Count ; i++)
		{
			Item itemRandomiser = m_itemList[i].GetComponent<Item>();
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

		if (barPercentage <= 0f)
		{
			m_userInterfaceManager.EnableGameOverWidget(true);
			m_userInterfaceManager.EnableProgressBar(false);
		}

		return barPercentage;
	}
}

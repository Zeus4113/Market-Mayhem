using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ScoreManager : MonoBehaviour
{
	// Events
	public delegate void ProgressBarEvent(float barPercentage);
	public event ProgressBarEvent updateProgressBar;

	public delegate void GameOverEvent();
	public event GameOverEvent gameOver;

	// Initialization References
	private GameManager m_gameManager;
	private Transform[] m_breakableTransforms;
	private ScoreManagerSetupData m_scoreManagerSetupData;

	// UI Elements
	private Slider m_progressBar;
	private Transform m_gameOverOverlay;
	private UserInterfaceManager m_userInterfaceManager;

	// Member Variables
	private int m_numberOfItems;
	private int m_totalItems;
	private float m_cashTotal;
	private float m_pricePerItem;

	// Lists
	private List<Transform> m_breakableList = new List<Transform>();
	private List<Item> m_itemList = new List<Item>();


	public void Init(ScoreManagerSetupData myData, GameManager gm, Transform[] positions)
	{
		m_gameManager = gm;
		m_scoreManagerSetupData = myData;
		m_breakableTransforms = positions;
		m_userInterfaceManager = m_gameManager.GetUIManager();

		if (m_userInterfaceManager == null) return;

		Transform myWidget = m_userInterfaceManager.GetWidget("Progress Bar").transform;

		if (myWidget == null) return;
		if (myWidget.GetComponent<Slider>() == null) return;

		m_progressBar = myWidget.GetComponent<Slider>();

	}

	private void BindHUDElements()
	{
		if (m_userInterfaceManager == null) return;

		Transform myWidget = m_userInterfaceManager.GetWidget("Progress Bar").transform;

		if (myWidget == null) return;
		if (myWidget.GetComponent<Slider>() == null) return;

		m_progressBar = myWidget.GetComponent<Slider>();
	}


	public void DecrementCurrentItems()
	{
		m_numberOfItems--;
		if (m_numberOfItems == 0) gameOver?.Invoke();
		m_progressBar.value = CalculateBarPercentage();
	}

	public Item GetRandomItem()
	{
		return m_itemList[Random.Range(0, m_itemList.Count-1)];
	}

	public List<Item> GetItemList() { return m_itemList; }

	public void SetupBreakables()
	{
		m_numberOfItems = 0;

		for(int i = 0; i < m_breakableTransforms.Length; i++) 
		{
			Transform newBreakable = Instantiate(m_scoreManagerSetupData.breakablePrefab, m_breakableTransforms[i].position, m_breakableTransforms[i].rotation).transform;

			m_breakableList.Add(newBreakable);

			ItemStore itemStore = newBreakable.GetComponent<ItemStore>();

			itemStore.Init();
			m_numberOfItems += itemStore.GetItemCount();
			m_itemList.AddRange(itemStore.GetItems());
		}

		BindItemEvents();
		m_userInterfaceManager.EnableGameOverWidget(false);

		m_totalItems = m_numberOfItems;
		return;
	}

	private void BindItemEvents()
	{
		for(int i = 0 ; i < m_itemList.Count ; i++)
		{
			Item item = m_itemList[i].GetComponent<Item>();
			item.itemDestroyed += DecrementCurrentItems;
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

		updateProgressBar?.Invoke(barPercentage);
		return barPercentage;
	}
}

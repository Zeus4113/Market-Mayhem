using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public enum EnemyStates
{
	Searching,
	Fleeing,
	Stunned,
}

public class EnemyController : MonoBehaviour
{
	[SerializeField] float m_movementSpeed = 4f;
	[SerializeField] float m_stunTime = 1f;

	private EnemyManager m_manager;

	private Item m_targetItem;
	private Transform m_spawnDoor;

	private Item m_pickedItem;

	private EnemyStates m_currentState;

	private Rigidbody2D m_rigidbody;
	private Collider2D m_collider;

	public void Init(Transform spawnDoor, EnemyManager enemyManager)
	{
		m_spawnDoor = spawnDoor;
		m_manager = enemyManager;
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_pickedItem = null;
		m_currentState = EnemyStates.Searching;

		m_targetItem = GetTargetItem();
		StartStateMachine();
	}

	public Item GetTargetItem()
	{
		List<Item> currentItems = m_manager.GetGameManager().GetScoreManager().GetItemList();

		int validItems = currentItems.Count;
		Debug.Log(validItems);

		while (true)
		{
			Item newItem = m_manager.GetGameManager().GetScoreManager().GetRandomItem();

			if (newItem == null) return null;

			if (!newItem.IsPickedUp())
			{
				return newItem;
			}
			else if(newItem.IsPickedUp())
			{
				validItems--;
				Debug.Log(m_manager.GetGameManager().GetScoreManager().GetItemList().Count);
				if (validItems == 0) break;
			}

		}

		return null;
	}

	public EnemyManager GetEnemyManager()
	{
		return m_manager;
	}

	Coroutine C_StateMachine;
	bool C_IsStateMachining = false;

	private void StartStateMachine()
	{
		if (C_IsStateMachining) return;

		C_IsStateMachining = true;

		if(C_StateMachine == null)
		{
			C_StateMachine = StartCoroutine(EnemyStateMachine());
		}
	}

	private void StopStateMachine()
	{
		if(!C_IsStateMachining) return;

		C_IsStateMachining = false;

		if(C_StateMachine != null)
		{
			StopCoroutine(C_StateMachine);
			C_StateMachine = null;
		}
	}

	private IEnumerator EnemyStateMachine()
	{
		while (C_IsStateMachining)
		{
			switch (m_currentState)
			{
				case EnemyStates.Searching:

					if (m_targetItem != null && !m_targetItem.IsPickedUp())
					{
						Move(m_targetItem.transform);
					}
					else
					{
						m_targetItem = GetTargetItem(); ;

						if(m_targetItem == null && GetTargetItem() == null)
						{
							SetEnemyState(EnemyStates.Fleeing);
							//m_manager.RemoveEnemy(this);
						}
					}

					break;

				case EnemyStates.Fleeing:

					Move(m_spawnDoor);

					break;

				case EnemyStates.Stunned:

					yield return new WaitForSeconds(m_stunTime);
					
					if(m_pickedItem != null)
					{
						SetEnemyState(EnemyStates.Fleeing);
					}
					else if(m_pickedItem == null)
					{
						SetEnemyState(EnemyStates.Searching);
					}

					break;
			}

			yield return new WaitForFixedUpdate();
		}
	}

	public void SetEnemyState(EnemyStates state)
	{
		m_currentState = state;
	}

	private void Move(Transform target)
	{
		Vector2 pos = Vector2.MoveTowards(transform.position, target.position, m_movementSpeed * Time.fixedDeltaTime);
		Vector2 direction = (target.position - transform.position).normalized;
		m_rigidbody.MovePosition(pos);
		transform.up = direction;
	}

	public void AddItem(Item item)
	{
		item.SetPickedUp(true);
		m_pickedItem = item;
		m_pickedItem.transform.parent = transform;
		m_pickedItem.transform.position = transform.position;
		SetEnemyState(EnemyStates.Fleeing);
	}

	public void DropItem()
	{
		if(m_pickedItem != null)
		{
			m_pickedItem.transform.parent = null;
			m_pickedItem.SetPickedUp(false);
		}
	}

	public void RemoveItem()
	{
		m_manager.GetGameManager().GetScoreManager().RemoveItemFromList(m_pickedItem);
		Destroy(m_pickedItem);
		SetEnemyState(EnemyStates.Searching);
	}

	public Item GetPickedItem()
	{
		return m_pickedItem;
	}
}

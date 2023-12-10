using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
	private Transform m_targetDoor;

	private GameObject m_pickedItem;

	private EnemyStates m_currentState;

	private Rigidbody2D m_rigidbody;
	private Collider2D m_collider;

	public void Init(Transform targetDoor, EnemyManager enemyManager)
	{
		m_targetDoor = targetDoor;
		m_manager = enemyManager;
		m_targetItem = GetTargetItem();
		m_rigidbody = GetComponent<Rigidbody2D>();
		m_pickedItem = null;
		m_currentState = EnemyStates.Searching;

		StartStateMachine();
	}

	public Item GetTargetItem()
	{
		while (true)
		{
			Item newItem = m_manager.GetNewItem();

			if (newItem.IsPickedUp())
			{
				newItem = m_manager.GetNewItem();
			}
			else if(!newItem.IsPickedUp())
			{
				return newItem;
			}
		}
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

					if (m_targetItem)
					{
						Move(m_targetItem.transform);
					}
					else
					{
						m_targetItem = GetTargetItem();

						if(m_targetItem == null)  
						{
							m_manager.RemoveEnemy(this);
						}
					}

					break;

				case EnemyStates.Fleeing:

					Move(m_targetDoor);

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
		m_pickedItem = item.gameObject;
		m_pickedItem.transform.parent = transform;
		m_pickedItem.transform.position = transform.position;
		SetEnemyState(EnemyStates.Fleeing);
	}

	public void RemoveItem()
	{
		Destroy(m_pickedItem);

		SetEnemyState(EnemyStates.Searching);
	}

	public GameObject GetPickedItem()
	{
		return m_pickedItem;
	}
}

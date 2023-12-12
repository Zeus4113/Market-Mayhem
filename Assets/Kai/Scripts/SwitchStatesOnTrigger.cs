using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchStatesOnTrigger : MonoBehaviour
{
	[SerializeField] private EnemyStates m_triggeredState;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision == null) return;

		if (collision.GetComponent<EnemyController>() == null) return;

		EnemyController enemyController = collision.GetComponent<EnemyController>();


		enemyController.RemoveItem();

		if(enemyController.GetTargetItem() == null)
		{
			enemyController.GetEnemyManager().RemoveEnemy(enemyController);
		}

		//enemyController.SetEnemyState(m_triggeredState);
	}
}

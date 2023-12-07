using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetter : MonoBehaviour
{
	private Camera m_camera;
	private GameObject m_playerRef;
	private Transform m_parentPosition;
	private float m_offset;

	public void Init(GameObject player, float offset)
	{
		m_offset = offset;
		m_parentPosition = player.transform;
		m_camera = GetComponent<Camera>();
	}

	private void FixedUpdate()
	{
		if (m_parentPosition == null || m_offset == 0) return;

		Debug.Log("Firing");
		transform.position = new Vector3(m_parentPosition.position.x, m_parentPosition.position.y, m_offset);

	}
}

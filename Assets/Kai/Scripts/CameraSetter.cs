using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetter : MonoBehaviour
{
	private Camera m_camera;
	private GameObject m_playerRef;
	private Vector3 m_parentPosition;
	private float m_offset;

	public void Init(GameObject player, float offset)
	{
		m_offset = offset;
		m_parentPosition = player.transform.position;
		m_camera = GetComponent<Camera>();
	}

	private void FixedUpdate()
	{
		if (m_parentPosition == null || m_offset != 0) return;

		transform.position = new Vector3(m_parentPosition.x, m_parentPosition.y, m_offset);

	}
}

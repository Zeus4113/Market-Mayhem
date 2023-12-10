using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetter : MonoBehaviour
{
	public void StartCameraRotate()
	{
		if (C_IsRotating) return;

		C_IsRotating = true;

		if (C_Rotating != null) return;

		C_Rotating = StartCoroutine(RotateCamera());
	}

	public void StopCameraRotate()
	{
		if (!C_IsRotating) return;

		C_IsRotating = false;

		if (C_Rotating == null) return;

		StopCoroutine(C_Rotating);
	}

	bool C_IsRotating = false;
	Coroutine C_Rotating;

	private IEnumerator RotateCamera()
	{
		while(C_IsRotating)
		{
			transform.rotation = Quaternion.identity;
			yield return new WaitForFixedUpdate();
		}
	}
}

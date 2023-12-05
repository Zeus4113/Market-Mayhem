using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
	public void Init(PlayerInput inputComponent)
	{
		inputComponent.actions.FindAction("Throw").performed += OnThrow;
		inputComponent.actions.FindAction("Attack").performed += OnAttack;
	}

	void OnThrow(InputAction.CallbackContext ctx)
	{
		Debug.Log("Thowing...");
	}

	void OnAttack(InputAction.CallbackContext ctx)
	{
		Debug.Log("Attacking...");
	}
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[System.Serializable] public class Vector2Event : UnityEvent<Vector2> { }
[System.Serializable] public class BoolEvent : UnityEvent<bool> { }
[System.Serializable] public class FloatEvent : UnityEvent<float> { }


public class InputManager : MonoBehaviour
{

	[SerializeField] public Vector2Event movementInput;
	[SerializeField] public UnityEvent hitInput;
	[SerializeField] public UnityEvent throwInput;

	void OnAttack()
	{
		hitInput?.Invoke();
	}

	void OnThrow()
	{
		throwInput?.Invoke();
	}

	public void OnMovement(InputValue inputValue)
	{
		movementInput?.Invoke(inputValue.Get<Vector2>());
		Debug.Log(inputValue.ToString());
	}
}

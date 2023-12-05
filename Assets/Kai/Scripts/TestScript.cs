using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
	PlayerInput input;

	

    void Start()
    {
        input = GetComponent<PlayerInput>();


		input.actions.FindAction("Movement").performed += Movement;

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void Movement(InputAction.CallbackContext ctx)
	{
		Vector2 dir = ctx.ReadValue<Vector2>();



		//iMoveable iMove = GetComponent<iMoveable>();

		//iMove.OnMovement(dir);
	}
}

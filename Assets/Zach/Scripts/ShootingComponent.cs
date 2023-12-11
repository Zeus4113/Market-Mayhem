using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingComponent : MonoBehaviour
{
    private GameObject m_projectile;
    private Rigidbody2D m_playerHand;
    private GameObject m_spawnedProjectile;

    public void BindEvents(bool bindBool, PlayerInput playerInputComponent, string handSide)
    {
        switch (bindBool)
        {
            case true:
                switch (handSide)
                {
                    case "left":
                        playerInputComponent.actions.FindAction("Attack Left").performed += Fire;
                        break;
                    case "right":
                        playerInputComponent.actions.FindAction("Attack Right").performed += Fire;
                        break;
                }
                break;
            case false:
                switch (handSide)
                {
                    case "left":
                        playerInputComponent.actions.FindAction("Attack Left").performed -= Fire;
                        break;
                    case "right":
                        playerInputComponent.actions.FindAction("Attack Right").performed -= Fire;
                        break;
                }
                break;
        }
    }

    private void Fire(InputAction.CallbackContext ctx)
    {
        Debug.Log("Fire ranged weapon");
        m_spawnedProjectile = Instantiate(m_projectile, transform.position, transform.rotation);
        m_spawnedProjectile.GetComponent<Projectile>().FireProjectile(m_playerHand.transform, 50f);
    }

    public void SetProjectile(GameObject projectile)
    {
        m_projectile = projectile;
    }

    public void SetPlayerHand(Rigidbody2D playerHand)
    {
        m_playerHand = playerHand;
    }
}

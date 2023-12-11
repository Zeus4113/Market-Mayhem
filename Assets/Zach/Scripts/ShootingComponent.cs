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
    private Throwable m_throwable;

    private void Awake()
    {
        m_throwable = this.gameObject.GetComponent<Throwable>();
    }

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
        m_spawnedProjectile = Instantiate(m_projectile, transform.position, transform.rotation);
        m_spawnedProjectile.GetComponent<Projectile>().FireProjectile(m_playerHand.transform, 50f);
        m_throwable.EditDurability(-1);
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

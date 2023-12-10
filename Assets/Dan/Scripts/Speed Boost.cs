using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float speedBoostPower;
    public bool isFacingUp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        Debug.Log(rb);
        if(isFacingUp == true)
        {
            
            Debug.Log("Whee");
            rb.AddForce(Vector2.down * speedBoostPower, ForceMode2D.Impulse);
        }
        if(isFacingUp == !true)
        {
            rb.AddForce(Vector2.left * speedBoostPower, ForceMode2D.Impulse);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

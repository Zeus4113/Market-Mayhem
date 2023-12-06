using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private Rigidbody2D PlayerRB;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerRB = collision.transform.parent.GetComponent<Rigidbody2D>();

    }
}

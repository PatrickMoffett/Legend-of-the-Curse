using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
        Destroy(col.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        throw new NotImplementedException();
    }
}

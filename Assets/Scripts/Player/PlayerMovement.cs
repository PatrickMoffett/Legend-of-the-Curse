using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public void Rotate(Vector3 direction)
    {
        transform.up = direction;
    }

    public void Move(Vector2 direction)
    {
        direction.Normalize();
        Vector3 movement = Vector2.zero;
        movement.x += direction.x * speed * Time.deltaTime;
        movement.y += direction.y * speed * Time.deltaTime;
        transform.position += movement;
    }
}

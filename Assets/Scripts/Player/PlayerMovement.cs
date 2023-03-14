using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private AttributeSet _attributeSet;
    private void Start()
    {
        _attributeSet = GetComponent<AttributeSet>();
    }

    public void Rotate(Vector3 direction)
    {
        transform.up = direction;
    }

    public void Move(Vector2 direction)
    {
        direction.Normalize();
        Vector3 movement = Vector2.zero;
        movement.x += direction.x * Time.deltaTime * _attributeSet.moveSpeed.CurrentValue;
        movement.y += direction.y * Time.deltaTime * _attributeSet.moveSpeed.CurrentValue;
        transform.position += movement;
    }
}

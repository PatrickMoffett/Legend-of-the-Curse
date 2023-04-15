using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
        [SerializeField] private CharacterStats _stats;
        private AttributeSet _attributeSet;
        private Vector2 _direction;
        private Animator _animator;
        private static readonly int XDirection = Animator.StringToHash("xDirection");
        private static readonly int YDirection = Animator.StringToHash("yDirection");

        private void Start()
    {
        _animator = GetComponent<Animator>();
        _attributeSet = GetComponent<AttributeSet>();
    }

    public void Rotate(Vector3 direction)
    {
        _direction = direction;
        _direction.Normalize();
        _animator.SetFloat(XDirection,_direction.x);
        _animator.SetFloat(YDirection,_direction.y);
        //transform.up = direction;
    }

    public Vector2 GetDirection()
    {
        return _direction;
    }

    public void Move(Vector2 direction)
    {
        direction.Normalize();
        Vector3 movement = Vector2.zero;
        movement.x += direction.x * Time.deltaTime * _attributeSet.moveSpeed.CurrentValue;
        movement.y += direction.y * Time.deltaTime * _attributeSet.moveSpeed.CurrentValue;
        transform.position += movement;

        _stats.totalDistance += movement.magnitude;
        _stats.steps = Mathf.RoundToInt(_stats.totalDistance);
    }
}

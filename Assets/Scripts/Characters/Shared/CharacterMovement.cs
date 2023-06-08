using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
        public ParticleSystem dust;
        private AttributeSet _attributeSet;
        private Vector2 _direction;
        private Animator _animator;
        private static readonly int XDirectionHash = Animator.StringToHash("xDirection");
        private static readonly int YDirectionHash = Animator.StringToHash("yDirection");
        private static readonly int SpeedHash = Animator.StringToHash("Speed");

        private void Start()
    {
        _animator = GetComponent<Animator>();
        _attributeSet = GetComponent<AttributeSet>();
    }

    public void Rotate(Vector3 direction)
    {
        _direction = direction;
        _direction.Normalize();
        if (_animator)
        {
            _animator.SetFloat(XDirectionHash, _direction.x);
            _animator.SetFloat(YDirectionHash, _direction.y);
        }
    }

    public Vector2 GetDirection()
    {
        return _direction;
    }

    public virtual void Move(Vector2 direction)
    {
        direction.Normalize();
        Vector3 movement = Vector3.zero;
        movement.x += direction.x * Time.deltaTime * _attributeSet.moveSpeed.CurrentValue;
        movement.y += direction.y * Time.deltaTime * _attributeSet.moveSpeed.CurrentValue;
        _animator.SetFloat(SpeedHash, movement.magnitude);
        transform.position += movement;
        CreateDust();
    }

    void CreateDust(){
        if (dust) { // make sure it isn't undefined
            dust.Play();
        }
    }
}

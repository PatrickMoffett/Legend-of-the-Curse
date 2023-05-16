using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{

    [SerializeField] private float aggroRange = 8f;
    [SerializeField] private float distanceToPerformAttack;
    [SerializeField] private Ability basicAttack;

    public event Action<GameObject> OnEnemyDied;
    
    private GameObject _player;
    private Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed");

    // Start is called before the first frame update
    private void Start()
    {
        base.Start();
        
        CombatSystem=GetComponent<CombatSystem>();
        CharacterMovement = GetComponent<CharacterMovement>();
        AttributeSet = GetComponent<AttributeSet>();
        AttributeSet.currentHealth.OnValueChanged += HealthChanged;
        _player = GameObject.Find("Player");
        _animator = GetComponent<Animator>();
        basicAttack = Instantiate(basicAttack);
        basicAttack.Initialize(gameObject);
    }

    private void HealthChanged(ModifiableAttributeValue health)
    {
        if (health.CurrentValue <= 0)
        {
            OnEnemyDied?.Invoke(gameObject);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        var dir = _player.transform.position - transform.position;
        float sqrDistance = dir.sqrMagnitude;
        
        //if outside of aggro range do nothing
        if (sqrDistance > aggroRange * aggroRange)
        {
            _animator.SetFloat(Speed, 0);
            return;
        }
        dir.Normalize();
        CharacterMovement.Rotate(dir);
        if (sqrDistance > distanceToPerformAttack * distanceToPerformAttack)
        {
            CharacterMovement.Move(dir);
        }
        else
        {
            _animator.SetFloat(Speed, 0);
            AbilityTargetData targetData = new AbilityTargetData();
            targetData.sourceCharacterDirection = dir;
            targetData.sourceCharacterLocation = transform.position;
            targetData.targetLocation = _player.transform.position;
            targetData.targetGameObject = _player;
            basicAttack.TryActivate(targetData);
        }
    }
}

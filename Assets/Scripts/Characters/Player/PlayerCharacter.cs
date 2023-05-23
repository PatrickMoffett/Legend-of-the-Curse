using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCharacter : Character
{
    [SerializeField] private PlayerStatistics _stats;
    public Ability basicAttack;
    public Ability specialAttack;

    public event Action OnPlayerDied;

    protected override void Start()
    {
        base.Start();
        
        AttributeSet.currentHealth.OnValueChanged += HealthChanged;
        basicAttack = Instantiate(basicAttack);
        basicAttack.Initialize(gameObject);
        specialAttack = Instantiate(specialAttack);
        specialAttack.Initialize(gameObject);
    }

    private void HealthChanged(ModifiableAttributeValue modifiableAttributeValue)
    {
        if (modifiableAttributeValue.BaseValue <= 0f)
        {
            ServiceLocator.Instance.Get<ApplicationStateManager>().PushState<GameOverState>(true);
            OnPlayerDied?.Invoke();
        }
    }

    public void PerformBasicAttack()
    {
        AbilityTargetData targetData = new AbilityTargetData();
        targetData.sourceCharacterDirection = CharacterMovement.GetDirection();
        targetData.sourceCharacterLocation = transform.position;
        //targetData.targetLocation
        //targetData.targetGameObject
        if (basicAttack.TryActivate(targetData))
        {
            _stats.shots++;
        }
    }

    public void PerformSpecialAttack()
    {
        AbilityTargetData targetData = new AbilityTargetData();
        targetData.sourceCharacterDirection = CharacterMovement.GetDirection();
        targetData.sourceCharacterLocation = transform.position;
        //targetData.targetLocation
        //targetData.targetGameObject
        specialAttack.TryActivate(targetData);
    }
}

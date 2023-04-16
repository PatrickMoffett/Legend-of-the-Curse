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

    protected override void Start()
    {
        base.Start();
        
        AttributeSet.currentHealth.OnValueChanged += HealthChanged;
        basicAttack = Instantiate(basicAttack);
        basicAttack.Initialize(gameObject);
    }

    private void HealthChanged(ModifiableAttributeValue modifiableAttributeValue)
    {
        if (modifiableAttributeValue.BaseValue <= 0f)
        {
            ServiceLocator.Instance.Get<ApplicationStateManager>().PushState<GameOverState>();
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
}

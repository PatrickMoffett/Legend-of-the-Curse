using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCharacter : MonoBehaviour
{
    public Ability basicAttack;
    private CombatSystem _combatSystem;
    private AttributeSet _attributeSet;

    public void Start()
    {
        _combatSystem = GetComponent<CombatSystem>();
        _attributeSet = GetComponent<AttributeSet>();
        _attributeSet.currentHealth.ValueChanged += HealthChanged;
        basicAttack.Initialize(gameObject);
    }

    private void HealthChanged(float newValue)
    {
        if (newValue <= 0f)
        {
            ServiceLocator.Instance.Get<ApplicationStateManager>().PushState<GameOverState>();
        }
    }

    public void PerformBasicAttack()
    {
        basicAttack.Activate();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private CharacterStats _stats;
    public Ability basicAttack;
    private CombatSystem _combatSystem;
    private AttributeSet _attributeSet;

    public void Start()
    {
        _combatSystem = GetComponent<CombatSystem>();
        _attributeSet = GetComponent<AttributeSet>();
        _attributeSet.currentHealth.OnValueChanged += HealthChanged;
        basicAttack = Instantiate(basicAttack);
        basicAttack.Initialize(gameObject);
    }

    private void HealthChanged(Attribute attribute)
    {
        if (attribute.BaseValue <= 0f)
        {
            ServiceLocator.Instance.Get<ApplicationStateManager>().PushState<GameOverState>();
        }
    }

    public void PerformBasicAttack()
    {
        basicAttack.Activate(transform.up);
        
        _stats.shots++;
        
    }
}

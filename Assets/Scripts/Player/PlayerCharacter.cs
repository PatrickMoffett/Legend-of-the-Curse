using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCharacter : MonoBehaviour
{
    public Ability basicAttack;
    public StatusEffect testEffect;
    private CombatSystem _combatSystem;

    public void Start()
    {
        _combatSystem = GetComponent<CombatSystem>();
        basicAttack.Initialize(gameObject);
    }

    public void PerformBasicAttack()
    {
        basicAttack.Activate();
    }
}

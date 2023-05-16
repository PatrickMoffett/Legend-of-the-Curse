
using System;
using UnityEngine;


public class Character : MonoBehaviour
{
    protected CombatSystem CombatSystem;
    protected AttributeSet AttributeSet;
    protected CharacterMovement CharacterMovement;
    protected SpriteFlash SpriteFlash; 
    
    protected virtual void Start()
    {
        CharacterMovement = GetComponent<CharacterMovement>();
        CombatSystem = GetComponent<CombatSystem>();
        AttributeSet = GetComponent<AttributeSet>();
        SpriteFlash = GetComponent<SpriteFlash>();

        AttributeSet.currentHealth.OnValueChanged += OnHealthChanged;
    }

    protected void OnHealthChanged(ModifiableAttributeValue value)
    {
        SpriteFlash.Flash();
    }
}

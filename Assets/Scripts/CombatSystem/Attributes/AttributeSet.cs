using System;
using UnityEngine;

public enum AttributeType
{
    Damage,
    CurrentHealth,
    MaxHealth,
    HealthRegen,
    CurrentMana,
    MaxMana,
    ManaRegen,
    AttackSpeed,
    AttackPower,
    MagicPower,
    PhysicalDefense,
    MagicalDefense,
    MoveSpeed
}
public class AttributeSet : MonoBehaviour
{
    public Attribute currentHealth;
    public Attribute maxHealth;
    public Attribute healthRegen;
    public Attribute currentMana;
    public Attribute maxMana;
    public Attribute manaRegen;
    public Attribute attackSpeed;
    public Attribute attackPower;
    public Attribute magicPower;
    public Attribute physicalDefense;
    public Attribute magicalDefense;
    public Attribute moveSpeed;

    private void Start()
    {
        currentHealth.UpdateCurrentValue();
        maxHealth.UpdateCurrentValue();
        healthRegen.UpdateCurrentValue();
        currentMana.UpdateCurrentValue();
        maxMana.UpdateCurrentValue();
        manaRegen.UpdateCurrentValue();
        attackSpeed.UpdateCurrentValue();
        attackPower.UpdateCurrentValue();
        magicPower.UpdateCurrentValue();
        physicalDefense.UpdateCurrentValue();
        magicalDefense.UpdateCurrentValue();
        moveSpeed.UpdateCurrentValue();
    }

    private void HandleDamage(AttributeModifier attributeModifier)
    {
        throw new System.NotImplementedException();
    }
    public void ApplyModifier(AttributeModifier attributeModifier)
    {
        switch (attributeModifier.attribute)
        { 
            case AttributeType.Damage:
                HandleDamage(attributeModifier);
                break;
            case AttributeType.CurrentHealth:
                currentHealth.AddModifier(attributeModifier);
                break;
            case AttributeType.MaxHealth:
                maxHealth.AddModifier(attributeModifier);
                break;
            case AttributeType.HealthRegen:
                healthRegen.AddModifier(attributeModifier);
                break;
            case AttributeType.CurrentMana:
                currentMana.AddModifier(attributeModifier);
                break;
            case AttributeType.MaxMana:
                maxMana.AddModifier(attributeModifier);
                break;
            case AttributeType.ManaRegen:
                manaRegen.AddModifier(attributeModifier);
                break;
            case AttributeType.AttackSpeed:
                attackSpeed.AddModifier(attributeModifier);
                break;
            case AttributeType.AttackPower:
                attackPower.AddModifier(attributeModifier);
                break;
            case AttributeType.MagicPower:
                magicPower.AddModifier(attributeModifier);
                break;
            case AttributeType.PhysicalDefense:
                physicalDefense.AddModifier(attributeModifier);
                break;
            case AttributeType.MagicalDefense:
                magicalDefense.AddModifier(attributeModifier);
                break;
            case AttributeType.MoveSpeed:
                moveSpeed.AddModifier(attributeModifier);
                break;
            default:
                Debug.LogError("Unexpected Attribute Enum on Apply Modifier");
                break;
        }
    }

    public void RemoveModifier(AttributeModifier attributeModifier)
    {
        switch (attributeModifier.attribute)
        {
            case AttributeType.CurrentHealth:
                currentHealth.RemoveModifier(attributeModifier);
                break;
            case AttributeType.MaxHealth:
                maxHealth.RemoveModifier(attributeModifier);
                break;
            case AttributeType.HealthRegen:
                healthRegen.RemoveModifier(attributeModifier);
                break;
            case AttributeType.CurrentMana:
                currentMana.RemoveModifier(attributeModifier);
                break;
            case AttributeType.MaxMana:
                maxMana.RemoveModifier(attributeModifier);
                break;
            case AttributeType.ManaRegen:
                manaRegen.RemoveModifier(attributeModifier);
                break;
            case AttributeType.AttackSpeed:
                attackSpeed.RemoveModifier(attributeModifier);
                break;
            case AttributeType.AttackPower:
                attackPower.RemoveModifier(attributeModifier);
                break;
            case AttributeType.MagicPower:
                magicPower.RemoveModifier(attributeModifier);
                break;
            case AttributeType.PhysicalDefense:
                physicalDefense.RemoveModifier(attributeModifier);
                break;
            case AttributeType.MagicalDefense:
                magicalDefense.RemoveModifier(attributeModifier);
                break;
            case AttributeType.MoveSpeed:
                moveSpeed.RemoveModifier(attributeModifier);
                break;
            default:
                Debug.LogError("Unexpected Attribute Enum on Remove Modifier");
                break;
        }
    }
}

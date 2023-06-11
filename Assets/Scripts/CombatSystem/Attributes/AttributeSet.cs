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
    public ModifiableAttributeValue currentHealth = new ModifiableAttributeValue(10f);
    public ModifiableAttributeValue maxHealth = new ModifiableAttributeValue(10f);
    public ModifiableAttributeValue healthRegen = new ModifiableAttributeValue(1f);
    public ModifiableAttributeValue currentMana = new ModifiableAttributeValue(10f);
    public ModifiableAttributeValue maxMana = new ModifiableAttributeValue(10f);
    public ModifiableAttributeValue manaRegen = new ModifiableAttributeValue(1f);
    public ModifiableAttributeValue attackSpeed = new ModifiableAttributeValue(2f);
    public ModifiableAttributeValue attackPower = new ModifiableAttributeValue(10f);
    public ModifiableAttributeValue magicPower = new ModifiableAttributeValue(10f);
    public ModifiableAttributeValue physicalDefense = new ModifiableAttributeValue(10f);
    public ModifiableAttributeValue magicalDefense = new ModifiableAttributeValue(10f);
    public ModifiableAttributeValue moveSpeed = new ModifiableAttributeValue(5f);

    private void Start()
    {
        currentHealth.SetMaxAttribute(maxHealth);
        currentMana.SetMaxAttribute(maxMana);
    }

    public void UpdateCurrentValues()
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
    // ReSharper disable Unity.PerformanceAnalysis
    public void ApplyModifier(AttributeModifier attributeModifier)
    {
        switch (attributeModifier.attribute)
        {
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
            case AttributeType.Damage:
            default:
                Debug.LogError("Unexpected Attribute Enum on Apply Modifier");
                break;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
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
            case AttributeType.Damage:
            default:
                Debug.LogError("Unexpected Attribute Enum on Remove Modifier");
                break;
        }
    }

    public void ApplyInstantModifier(AttributeModifier attributeModifier)
    {
        switch (attributeModifier.attribute)
        {
            case AttributeType.Damage:
                //TODO: Insert Defense Calculation Logic here
                currentHealth.InstantlyApply(attributeModifier);
                break;
            case AttributeType.CurrentHealth:
                currentHealth.InstantlyApply(attributeModifier);
                break;
            case AttributeType.MaxHealth:
                maxHealth.InstantlyApply(attributeModifier);
                break;
            case AttributeType.HealthRegen:
                healthRegen.InstantlyApply(attributeModifier);
                break;
            case AttributeType.CurrentMana:
                currentMana.InstantlyApply(attributeModifier);
                break;
            case AttributeType.MaxMana:
                maxMana.InstantlyApply(attributeModifier);
                break;
            case AttributeType.ManaRegen:
                manaRegen.InstantlyApply(attributeModifier);
                break;
            case AttributeType.AttackSpeed:
                attackSpeed.InstantlyApply(attributeModifier);
                break;
            case AttributeType.AttackPower:
                attackPower.InstantlyApply(attributeModifier);
                break;
            case AttributeType.MagicPower:
                magicPower.InstantlyApply(attributeModifier);
                break;
            case AttributeType.PhysicalDefense:
                physicalDefense.InstantlyApply(attributeModifier);
                break;
            case AttributeType.MagicalDefense:
                magicalDefense.InstantlyApply(attributeModifier);
                break;
            case AttributeType.MoveSpeed:
                moveSpeed.InstantlyApply(attributeModifier);
                break;
            default:
                Debug.LogError("Unexpected Attribute Enum on Remove Modifier");
                break;
        }
    }

    public float GetCurrentAttributeValue(AttributeType type)
    {
        switch (type)
        {
            case AttributeType.CurrentHealth:
                return currentHealth.CurrentValue;
            case AttributeType.MaxHealth:
                return maxHealth.CurrentValue;
            case AttributeType.HealthRegen:
                return healthRegen.CurrentValue;
            case AttributeType.CurrentMana:
                return currentMana.CurrentValue;
            case AttributeType.MaxMana:
                return maxMana.CurrentValue;
            case AttributeType.ManaRegen:
                return manaRegen.CurrentValue;
            case AttributeType.AttackSpeed:
                return attackSpeed.CurrentValue;
            case AttributeType.AttackPower:
                return attackPower.CurrentValue;
            case AttributeType.MagicPower:
                return magicPower.CurrentValue;
            case AttributeType.PhysicalDefense:
                return physicalDefense.CurrentValue;
            case AttributeType.MagicalDefense:
                return magicalDefense.CurrentValue;
            case AttributeType.MoveSpeed:
                return moveSpeed.CurrentValue;
            case AttributeType.Damage:
            default:
                Debug.LogError("Unexpected Attribute Enum on Remove Modifier");
                return 0f;
        }
    }
    
    public float GetBaseAttributeValue(AttributeType type)
    {
        switch (type)
        {
            case AttributeType.CurrentHealth:
                return currentHealth.BaseValue;
            case AttributeType.MaxHealth:
                return maxHealth.BaseValue;
            case AttributeType.HealthRegen:
                return healthRegen.BaseValue;
            case AttributeType.CurrentMana:
                return currentMana.BaseValue;
            case AttributeType.MaxMana:
                return maxMana.BaseValue;
            case AttributeType.ManaRegen:
                return manaRegen.BaseValue;
            case AttributeType.AttackSpeed:
                return attackSpeed.BaseValue;
            case AttributeType.AttackPower:
                return attackPower.BaseValue;
            case AttributeType.MagicPower:
                return magicPower.BaseValue;
            case AttributeType.PhysicalDefense:
                return physicalDefense.BaseValue;
            case AttributeType.MagicalDefense:
                return magicalDefense.BaseValue;
            case AttributeType.MoveSpeed:
                return moveSpeed.BaseValue;
            case AttributeType.Damage:
            default:
                Debug.LogError("Unexpected Attribute Enum on Remove Modifier");
                return 0f;
        }
    }
}

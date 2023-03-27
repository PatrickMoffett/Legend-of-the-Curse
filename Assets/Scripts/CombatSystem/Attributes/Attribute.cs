using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Attribute
{
    public Attribute(){ }

    public Attribute(float value)
    {
        baseValue = value;
    }
    /// <summary>
    /// Action Invoked whenever the CurrentValue of this Attribute changes
    /// </summary>
    public event Action<Attribute> OnValueChanged;

    /// <summary>
    /// Dictionary of Attribute modifiers
    /// </summary>
    private List<AttributeModifier> _modifiers = new List<AttributeModifier>();
    
    /// <summary>
    /// base value of this attribute before modifiers
    /// </summary>
    [SerializeField]private float baseValue;
    
    /// <summary>
    /// Base Value of the attribute before modifiers
    /// </summary>
    public float BaseValue
    {
        get
        {
            return baseValue;
        }
        set
        {
            baseValue = value;
            UpdateCurrentValue();
            OnValueChanged?.Invoke(this);
        }
    }
    
    /// <summary>
    /// Current Value of the attribute after all modifiers
    /// </summary>
    public float CurrentValue { get; private set; }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// Updates the current value of this attribute including all the modifiers affecting it
    /// </summary>
    public void UpdateCurrentValue()
    {
        float sumToAdd = 0f;
        float multiplier = 1f;
        foreach (var modifier in _modifiers)
        {
            switch (modifier.operation)
            {
                case AttributeModifier.Operator.Add:
                    sumToAdd += modifier.modificationValue;
                    break;
                case AttributeModifier.Operator.Subtract:
                    sumToAdd -= modifier.modificationValue;
                    break;
                case AttributeModifier.Operator.Multiply:
                    multiplier *= modifier.modificationValue;
                    break;
                case AttributeModifier.Operator.Divide:
                    multiplier /= modifier.modificationValue;
                    break;
                case AttributeModifier.Operator.Set:
                    CurrentValue = modifier.modificationValue;
                    return;
                default:
                    Debug.LogError("Unexpected Operator in attributeMod");
                    break;
            }
        }
        CurrentValue = (baseValue + sumToAdd) * multiplier;
    }
    
    /// <summary>
    /// Add a modifier that will change the value of the attribute
    /// </summary>
    /// <param name="modifier"></param>
    public void AddModifier(AttributeModifier modifier)
    {
        _modifiers.Add(modifier);
        UpdateCurrentValue();
        OnValueChanged?.Invoke(this);
    }
    /// <summary>
    /// Remove a modifier that was changing the value of the attribute
    /// </summary>
    /// <param name="modifier"></param>
    public void RemoveModifier(AttributeModifier modifier)
    {
        _modifiers.Remove(modifier);
        UpdateCurrentValue();
        OnValueChanged?.Invoke(this);
    }

    public void InstantlyApply(AttributeModifier modifier)
    {
        switch (modifier.operation)
        {
            case AttributeModifier.Operator.Add:
                baseValue += modifier.modificationValue;
                break;
            case AttributeModifier.Operator.Subtract:
                baseValue -= modifier.modificationValue;
                break;
            case AttributeModifier.Operator.Multiply:
                baseValue *= modifier.modificationValue;
                break;
            case AttributeModifier.Operator.Divide:
                baseValue /= modifier.modificationValue;
                break;
            case AttributeModifier.Operator.Set:
                baseValue = modifier.modificationValue;
                return;
            default:
                Debug.LogError("Unexpected Operator in attributeMod");
                break;
        }
        UpdateCurrentValue();
        OnValueChanged?.Invoke(this);
    }
}

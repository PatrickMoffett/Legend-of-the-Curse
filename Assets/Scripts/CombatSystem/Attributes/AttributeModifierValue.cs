using System;
using UnityEngine;

[Serializable]
public class AttributeModifierValue
{
    public enum ValueType
    {
        StaticFloat,
        AttributeBased,
        CustomCalculation
    }

    public enum AttributeSetToUse
    {
        Source,
        Target
    }
    
    public bool ValueTypeEqualsStaticFloat()
    {
        return valueType == ValueType.StaticFloat;
    }

    public bool ValueTypeEqualsAttributeBased()
    {
        return valueType == ValueType.AttributeBased;
    }

    public bool ValueTypeEqualsCustomCalc()
    {
        return valueType == ValueType.CustomCalculation;
    }
    
    [SerializeField]private ValueType valueType;
    
    [ShowIf(nameof(ValueTypeEqualsStaticFloat))]
    [SerializeField]
    private float staticFloat;
    
    [ShowIf(nameof(ValueTypeEqualsAttributeBased))]
    [SerializeField]
    private AttributeSetToUse attributeSet;

    [ShowIf(nameof(ValueTypeEqualsAttributeBased))]
    [SerializeField]
    private AttributeType sourceAttributeType;

    [ShowIf( nameof(ValueTypeEqualsAttributeBased))]
    [SerializeField]
    private float preCoefficientAddition;

    [ShowIf( nameof(ValueTypeEqualsAttributeBased))]
    [SerializeField]
    private float coefficient;
    
    [ShowIf( nameof(ValueTypeEqualsAttributeBased))]
    [SerializeField]
    private float postCoefficientAddition;

    [ShowIf(nameof(ValueTypeEqualsCustomCalc))]
    [SerializeField]
    private CustomValueCalculation customValueCalculation;

    private CombatSystem _sourceCombatSystem;
    private CombatSystem _targetCombatSystem;

    public float Value
    {
        get
        {
            if (!( _sourceCombatSystem && _targetCombatSystem))
            {
                Debug.LogError("Attribute Modifier Value missing properties");
                return 0f;
            }
            switch (valueType)
            {
                case ValueType.StaticFloat:
                    return staticFloat;
                case ValueType.AttributeBased:
                    if (attributeSet == AttributeSetToUse.Source)
                    {
                        return ((_sourceCombatSystem.GetAttributeSet().GetCurrentAttributeValue(sourceAttributeType)
                                 + preCoefficientAddition) * coefficient) + postCoefficientAddition;
                    }
                    else
                    {
                        return ((_targetCombatSystem.GetAttributeSet().GetCurrentAttributeValue(sourceAttributeType)
                                 + preCoefficientAddition) * coefficient) + postCoefficientAddition;
                    }
                case ValueType.CustomCalculation:
                    if (!customValueCalculation)
                    {
                        Debug.LogError("Custom Calculation Not Set");
                    }
                    return customValueCalculation.Calculate(_sourceCombatSystem, _targetCombatSystem);
                default:
                    Debug.LogError("Unsupported Value Type used");
                    return 0f;
            }
        }
    }

    public void SetSourceCombatSystem(CombatSystem combatSystem)
    {
        _sourceCombatSystem = combatSystem;
    }
    public void SetTargetCombatSystem(CombatSystem combatSystem)
    {
        _targetCombatSystem = combatSystem;
    }
}

using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class AttributeModifier
{
    public enum Operator
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Set
    }
    /// <summary>
    /// Which attribute to affect
    /// </summary>
    public AttributeType attribute;
    /// <summary>
    /// Operator to apply to Attribute
    /// </summary>
    public Operator operation;

    public AttributeModifierValue attributeModifierValue;
    
}
[System.Serializable]
public class AttributeModifierValue
{
    public enum ValueType
    {
        StaticFloat,
        AttributeBased,
    }

    public ValueType valueType;

    public float staticFloat;

    public AttributeType sourceAttributeType;

    public float preCoefficientAddition;

    public float coefficient;
    
    public float postCoefficientAddition;

    private AttributeSet _sourceAttributeSet;

    public void SetAttributeSet(AttributeSet attributeSet)
    {
        _sourceAttributeSet = attributeSet;
    }

    public float GetValue()
    {
        switch (valueType)
        {
            case ValueType.StaticFloat:
                return staticFloat;
            case ValueType.AttributeBased:
                if (!_sourceAttributeSet)
                {
                    Debug.LogError("sourceAttributeSet Not Set");
                    return 0f;
                }
                else
                {
                    return ((_sourceAttributeSet.GetCurrentAttributeValue(sourceAttributeType) + preCoefficientAddition)
                            * coefficient) + postCoefficientAddition;
                }
            default:
                Debug.LogError("Unsupported Value Type used");
                return 0f;
        }
        //TODO: implement
        return 0;
    }
}

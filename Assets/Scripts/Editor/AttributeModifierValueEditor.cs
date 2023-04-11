using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(AttributeModifierValue))]
public class AttributeModifierValueEditor : PropertyDrawer
{
    private SerializedProperty _property;
    private VisualElement _root;
    private EnumField _valueTypeField;
    private EnumField _attributeSetToUseField;
    private EnumField _attributeType;
    private FloatField _staticFloat;
    private FloatField _preCoefficient;
    private FloatField _coefficient;
    private FloatField _postcoefficient;
    private ObjectField _objectField;
    
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        _property =property;
        _root = new VisualElement();

        _valueTypeField = new EnumField();
        _valueTypeField.label = "Value Type:";
        _valueTypeField.BindProperty(_property.FindPropertyRelative("valueType"));
        _valueTypeField.RegisterValueChangedCallback(OnValueTypeChanged);

        _attributeSetToUseField = new EnumField();
        _attributeSetToUseField.label = "AttributeSource:";
        _attributeSetToUseField.BindProperty(_property.FindPropertyRelative("attributeSet"));

        _attributeType = new EnumField();
        _attributeType.label = "Attribute:";
        _attributeType.BindProperty(_property.FindPropertyRelative("sourceAttributeType"));

        _staticFloat = new FloatField();
        _staticFloat.label = "Value:";
        _staticFloat.BindProperty(_property.FindPropertyRelative("staticFloat"));
        
        _preCoefficient = new FloatField();
        _preCoefficient.label = "Pre-Coefficient Addition:";
        _preCoefficient.BindProperty(_property.FindPropertyRelative("preCoefficientAddition"));
        
        _coefficient = new FloatField();
        _coefficient.label = "Coefficient:";
        _coefficient.BindProperty(_property.FindPropertyRelative("coefficient"));
        
        _postcoefficient = new FloatField();
        _postcoefficient.label = "Post-Coefficient Addition:";
        _postcoefficient.BindProperty(_property.FindPropertyRelative("postCoefficientAddition"));
        
        _objectField = new ObjectField();
        _objectField.label = "Custom Value Calculation";
        _objectField.BindProperty(_property.FindPropertyRelative("customValueCalculation"));

        SetupRoot((AttributeModifierValue.ValueType)_property.FindPropertyRelative("valueType").intValue);
        return _root;
    }

    private void OnValueTypeChanged(ChangeEvent<Enum> evt)
    {
        AttributeModifierValue.ValueType typeValue = (AttributeModifierValue.ValueType)evt.newValue;
        SetupRoot(typeValue);
    }

    private void SetupRoot(AttributeModifierValue.ValueType typeValue)
    {
        _root.Clear();
        _root.Add(_valueTypeField);
        switch (typeValue)
        {
            case AttributeModifierValue.ValueType.AttributeBased:
                _root.Add(_attributeType);
                _root.Add(_attributeSetToUseField);
                _root.Add(_preCoefficient);
                _root.Add(_coefficient);
                _root.Add(_postcoefficient);
                break;
            case AttributeModifierValue.ValueType.StaticFloat:
                _root.Add(_staticFloat);
                break;
            case AttributeModifierValue.ValueType.CustomCalculation:
                _root.Add(_objectField);
                break;
        }
    }
}

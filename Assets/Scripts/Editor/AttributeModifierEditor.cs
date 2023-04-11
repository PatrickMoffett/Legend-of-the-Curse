
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(AttributeModifier))]
public class AttributeModifierPropertyDrawer : PropertyDrawer
{
    private readonly List<VisualElement> _roots = new List<VisualElement>();
    private readonly List<SerializedProperty> _properties = new List<SerializedProperty>();
    private readonly List<Foldout> _foldouts = new List<Foldout>();

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        int index = _properties.Count;
        
        _properties.Add(property);
        _roots.Add(new VisualElement());
        
        _foldouts.Add(new Foldout());
        _foldouts[index].value = false;
        _foldouts[index].text = "Attribute Modifier";
        _roots[index].Add(_foldouts[index]);
        
        EnumField attributeType = new EnumField();
        attributeType.label = "Attribute:";
        attributeType.BindProperty(property.FindPropertyRelative("attribute"));
        _foldouts[index].Add(attributeType);
        
        EnumField operationField = new EnumField();
        operationField.label = "Operation:";
        operationField.BindProperty(property.FindPropertyRelative("operation"));
        _foldouts[index].Add(operationField);

        PropertyField valueField = new PropertyField();
        valueField.BindProperty(property.FindPropertyRelative("attributeModifierValue"));
        _foldouts[index].Add(valueField);

        return _roots[index];
    }
    
}


using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(AttributeModifier))]
public class AttributeModifierPropertyDrawer : PropertyDrawer
{
    private VisualElement _root;
    private SerializedProperty _property;
    private Foldout _foldout;

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        _property = property;
        _root = new VisualElement();
        
        _foldout = new Foldout();
        _foldout.value = false;
        _foldout.text = "Attribute Modifier";
        _root.Add(_foldout);
        
        EnumField attributeType = new EnumField();
        attributeType.label = "Attribute:";
        attributeType.BindProperty(property.FindPropertyRelative("attribute"));
        _foldout.Add(attributeType);
        
        EnumField operationField = new EnumField();
        operationField.label = "Operation:";
        operationField.BindProperty(property.FindPropertyRelative("operation"));
        _foldout.Add(operationField);

        PropertyField valueField = new PropertyField();
        valueField.BindProperty(property.FindPropertyRelative("attributeModifierValue"));
        _foldout.Add(valueField);
        
        //attributeType.RegisterValueChangedCallback(OnAttributeChange);
        //operationField.RegisterValueChangedCallback(OnOperationChange);

        //UpdateFoldoutTitle();
        
        return _root;
    }

    private void OnOperationChange(ChangeEvent<Enum> evt)
    {
        UpdateFoldoutTitle();
    }

    private void OnAttributeChange(ChangeEvent<Enum> evt)
    {
        UpdateFoldoutTitle();
    }

    private void UpdateFoldoutTitle()
    {
        string attributeString = Enum.GetNames(typeof(AttributeType))[_property.FindPropertyRelative("attribute").intValue];
        string operationString = Enum.GetNames(typeof(AttributeModifier.Operator))[_property.FindPropertyRelative("operation").intValue];
        _foldout.text = operationString + " " + attributeString;
    }
}

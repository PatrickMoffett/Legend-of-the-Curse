using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Object = UnityEngine.Object;
using PopupWindow = UnityEditor.PopupWindow;


[CustomEditor(typeof(StatusEffect))]
public class StatusEffectEditor : Editor
{
    private VisualElement _root;
    private FloatField _durationField;
    private Toggle _isPeriodicToggle;
    private FloatField _periodicRate;
    private EnumField _durationType;
    private PropertyField _attributeModifierField;
    private PropertyField _abilityField;
    public override VisualElement CreateInspectorGUI()
    {
        _periodicRate = new FloatField("PeriodicRate");
        _periodicRate.label = "Periodic Rate:";
        _periodicRate.BindProperty(serializedObject.FindProperty("periodicRate"));
        
        _isPeriodicToggle = new Toggle("IsPeriodic");
        _isPeriodicToggle.label = "Is Periodic?";
        _isPeriodicToggle.tooltip = "Does the effect happen periodically?";
        _isPeriodicToggle.BindProperty(serializedObject.FindProperty("isPeriodic"));
        _isPeriodicToggle.RegisterValueChangedCallback(OnIsPeriodicChanged);

        _durationField = new FloatField("Duration");
        _durationField.label = "Duration:";
        _durationField.BindProperty(serializedObject.FindProperty("duration"));
        
        _durationType = new EnumField("DurationType");
        _durationType.label = "Duration Type:";
        _durationType.BindProperty(serializedObject.FindProperty("durationType"));
        _durationType.RegisterValueChangedCallback(OnDurationTypeChanged);

        _attributeModifierField = new PropertyField();
        _attributeModifierField.BindProperty(serializedObject.FindProperty("attributeModifiers"));
        
        _abilityField = new PropertyField();
        _abilityField.BindProperty(serializedObject.FindProperty("grantedAbilities"));

        _root = new VisualElement();

        //add default inspector
        Foldout defaultInspectorFoldout = new Foldout();
        defaultInspectorFoldout.name = "DefaultInspector";
        defaultInspectorFoldout.text = "Default Inspector";
        InspectorElement.FillDefaultInspector(defaultInspectorFoldout,serializedObject,this);

        StatusEffect.DurationType durationTypeValue = (StatusEffect.DurationType)serializedObject.FindProperty("durationType").intValue;
        
        
        _root.Add(_durationType);
        _root.Add(_attributeModifierField);
        _root.Add(_abilityField);
        //_root.Add(defaultInspectorFoldout);

        HandleDurationTypeValue(durationTypeValue);
        
        return _root;
    }

    private void OnDurationTypeChanged(ChangeEvent<Enum> evt)
    {
        StatusEffect.DurationType durationTypeValue = (StatusEffect.DurationType)serializedObject.FindProperty("durationType").intValue;
        HandleDurationTypeValue(durationTypeValue);
    }
    private void OnIsPeriodicChanged(ChangeEvent<bool> evt)
    {
        HandleIsPeriodicValue(evt.newValue);
    }
    private void HandleDurationTypeValue(StatusEffect.DurationType durationType)
    {
        switch (durationType)
        {
            case StatusEffect.DurationType.Duration:
                AddDurationField(1);
                AddIsPeriodicField(2);
                break;
            case StatusEffect.DurationType.Infinite:
                AddIsPeriodicField(1);
                if (_root.Contains(_durationField))
                {
                    _root.Remove(_durationField);
                }
                break;
            case StatusEffect.DurationType.Instant:
                if (_root.Contains(_durationField))
                {
                    _root.Remove(_durationField);
                }

                if (_root.Contains(_isPeriodicToggle))
                {
                    _root.Remove(_isPeriodicToggle);
                }

                if (_root.Contains(_periodicRate))
                {
                    _root.Remove(_periodicRate);
                }
                break;
            default:
                Debug.LogError("Unknown Duration Type Selected");
                break;
        }
    }
    private void HandleIsPeriodicValue(bool value)
    {
        if (value)
        {
            AddPeriodicRateField(_root.IndexOf(_isPeriodicToggle)+1);
        }
        else
        {
            if (_root.Contains(_periodicRate))
            {
                _root.Remove(_periodicRate);
            }
        }
    }
    private void AddIsPeriodicField(int indexToAdd)
    {
        if (_isPeriodicToggle != null && !_root.Contains(_isPeriodicToggle))
        {
            _root.Insert(indexToAdd,_isPeriodicToggle);
            HandleIsPeriodicValue(serializedObject.FindProperty("isPeriodic").boolValue);
        }
    }
    private void AddPeriodicRateField(int indexToAdd)
    {
        if (_periodicRate != null && !_root.Contains(_periodicRate))
        {
            _root.Insert(indexToAdd,_periodicRate);
        }
    }
    private void AddDurationField(int indexToAdd)
    {
        if (_durationField != null && !_root.Contains(_durationField))
        {
            _root.Insert(indexToAdd,_durationField);
        }
    }
}
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

        //load stylesheet
        //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/StatusEffectEditor.uss");

        _root = new VisualElement();

        //add stylesheet to root
       // _root.styleSheets.Add(styleSheet);
        
        //add default inspector
        Foldout defaultInspectorFoldout = new Foldout();
        defaultInspectorFoldout.name = "DefaultInspector";
        defaultInspectorFoldout.text = "Default Inspector";
        InspectorElement.FillDefaultInspector(defaultInspectorFoldout,serializedObject,this);

        StatusEffect.DurationType durationTypeValue = (StatusEffect.DurationType)serializedObject.FindProperty("durationType").intValue;
        
        
        _root.Add(_durationType);
        _root.Add(_attributeModifierField);
        _root.Add(defaultInspectorFoldout);

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

[CustomPropertyDrawer(typeof(StatusEffect))]
public class StatusEffectPropertyDrawer : PropertyDrawer
{
    private SerializedProperty _property;
    private SerializedObject _assetObject;
    private VisualElement _root;
    private Foldout _foldout;
    private FloatField _durationField;
    private Toggle _isPeriodicToggle;
    private FloatField _periodicRate;
    private EnumField _durationType;
    private ObjectField _objectField;

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        _property = property;
        
        _objectField = new ObjectField("StatusEffect:");
        _objectField.objectType = typeof(StatusEffect);
        _objectField.BindProperty(property);
        _objectField.RegisterValueChangedCallback(OnObjectChanged);

        _periodicRate = new FloatField("PeriodicRate");
        _periodicRate.label = "Periodic Rate:";

        _isPeriodicToggle = new Toggle("IsPeriodic");
        _isPeriodicToggle.label = "Is Periodic?";
        _isPeriodicToggle.tooltip = "Does the effect happen periodically?";
        _isPeriodicToggle.RegisterValueChangedCallback(OnIsPeriodicChanged);

        _durationField = new FloatField("Duration");
        _durationField.label = "Duration:";

        _durationType = new EnumField("DurationType");
        _durationType.label = "Duration Type:";
        _durationType.RegisterValueChangedCallback(OnDurationTypeChanged);

        //load stylesheet
        //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/StatusEffectEditor.uss");

        _root = new VisualElement();

        //add stylesheet to root
        //_root.styleSheets.Add(styleSheet);

        _foldout = new Foldout();
        _foldout.value = false;
        
        _root.Add(_objectField);
        _root.Add(_foldout);
        
        SetAssetObject(property);

        return _root;
    }

    private void SetAssetObject(SerializedProperty property)
    {
        if (property.objectReferenceValue == null)
        {
            if (_foldout.Contains(_durationType))
            {
                _foldout.Remove(_durationType);
            }
            return;
        }
        _assetObject = new SerializedObject(property.objectReferenceValue);
        
        _periodicRate.BindProperty(_assetObject.FindProperty("periodicRate"));
        _isPeriodicToggle.BindProperty(_assetObject.FindProperty("isPeriodic"));
        _durationField.BindProperty(_assetObject.FindProperty("duration"));
        _durationType.BindProperty(_assetObject.FindProperty("durationType"));
        
        StatusEffect.DurationType durationTypeValue = (StatusEffect.DurationType)_assetObject.FindProperty("durationType").intValue;

        if (!_foldout.Contains(_durationType))
        {
            _foldout.Add(_durationType);
        }

        HandleDurationTypeValue(durationTypeValue);
    }

    private void OnObjectChanged(ChangeEvent<Object> evt)
    {
        _periodicRate.Unbind();
        _isPeriodicToggle.Unbind();
        _durationField.Unbind();
        _durationType.Unbind();

        SetAssetObject(_property);
    }

    private void OnDurationTypeChanged(ChangeEvent<Enum> evt)
    {
        StatusEffect.DurationType durationTypeValue = (StatusEffect.DurationType)_assetObject.FindProperty("durationType").intValue;
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
                if (_foldout.Contains(_durationField))
                {
                    _foldout.Remove(_durationField);
                }
                break;
            case StatusEffect.DurationType.Instant:
                if (_foldout.Contains(_durationField))
                {
                    _foldout.Remove(_durationField);
                }

                if (_foldout.Contains(_isPeriodicToggle))
                {
                    _foldout.Remove(_isPeriodicToggle);
                }

                if (_foldout.Contains(_periodicRate))
                {
                    _foldout.Remove(_periodicRate);
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
            AddPeriodicRateField(_foldout.IndexOf(_isPeriodicToggle)+1);
        }
        else
        {
            if (_foldout.Contains(_periodicRate))
            {
                _foldout.Remove(_periodicRate);
            }
        }
    }
    private void AddIsPeriodicField(int indexToAdd)
    {
        if (_isPeriodicToggle != null && !_foldout.Contains(_isPeriodicToggle))
        {
            _foldout.Insert(indexToAdd,_isPeriodicToggle);
            HandleIsPeriodicValue(_assetObject.FindProperty("isPeriodic").boolValue);
        }
    }
    private void AddPeriodicRateField(int indexToAdd)
    {
        if (_periodicRate != null && !_foldout.Contains(_periodicRate))
        {
            _foldout.Insert(indexToAdd,_periodicRate);
        }
    }
    private void AddDurationField(int indexToAdd)
    {
        if (_durationField != null && !_foldout.Contains(_durationField))
        {
            _foldout.Insert(indexToAdd,_durationField);
        }
    }
}
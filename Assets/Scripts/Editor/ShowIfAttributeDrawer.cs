using System.Reflection;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;
using PlasticPipe.PlasticProtocol.Messages;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

[CustomPropertyDrawer(typeof(ShowIfAttribute), true)]
public class ShowIfAttributeDrawer : PropertyDrawer
{

    #region Reflection helpers.
    private static MethodInfo GetMethod(object target, string methodName)
    {
        return GetAllMethods(target, m => m.Name.Equals(methodName, 
                  StringComparison.InvariantCulture)).FirstOrDefault();
    }

    private static FieldInfo GetField(object target, string fieldName)
    {
        return GetAllFields(target, f => f.Name.Equals(fieldName, 
              StringComparison.InvariantCulture)).FirstOrDefault();
    }
    private static IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo, 
            bool> predicate)
    {
        List<Type> types = new List<Type>()
            {
                target.GetType()
            };

        while (types.Last().BaseType != null)
        {
            types.Add(types.Last().BaseType);
        }

        for (int i = types.Count - 1; i >= 0; i--)
        {
            IEnumerable<FieldInfo> fieldInfos = types[i]
                .GetFields(BindingFlags.Instance | BindingFlags.Static | 
   BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(predicate);

            foreach (var fieldInfo in fieldInfos)
            {
                yield return fieldInfo;
            }
        }
    }
    private static IEnumerable<MethodInfo> GetAllMethods(object target, 
  Func<MethodInfo, bool> predicate)
    {
        IEnumerable<MethodInfo> methodInfos = target.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Static | 
  BindingFlags.NonPublic | BindingFlags.Public)
            .Where(predicate);

        return methodInfos;
    }
    #endregion

    public static object GetTargetObjectParentOfProperty(SerializedProperty prop)
    {
        if (prop == null) return null;

        var path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        var elements = path.Split('.');
        List<string> stringList = new List<string>(elements);
        stringList.RemoveAt(stringList.Count-1);
        foreach (var element in stringList)
        {
            if (element.Contains("["))
            {
                var elementName = element.Substring(0, element.IndexOf("["));
                var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue_Imp(obj, elementName, index);
            }
            else
            {
                obj = GetValue_Imp(obj, element);
            }
        }
        return obj;
    }
    
    private static object GetValue_Imp(object source, string name)
    {
        if (source == null)
            return null;
        var type = source.GetType();

        while (type != null)
        {
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (f != null)
                return f.GetValue(source);

            var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p != null)
                return p.GetValue(source, null);

            type = type.BaseType;
        }
        return null;
    }
    private static object GetValue_Imp(object source, string name, int index)
    {
        var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
        if (enumerable == null) return null;
        var enm = enumerable.GetEnumerator();
        //while (index-- >= 0)
        //    enm.MoveNext();
        //return enm.Current;

        for (int i = 0; i <= index; i++)
        {
            if (!enm.MoveNext()) return null;
        }
        return enm.Current;
    }
    private bool MeetsConditions(SerializedProperty property)
    {
        var showIfAttribute = this.attribute as ShowIfAttribute;
        object target = GetTargetObjectParentOfProperty(property);
        
        List<bool> conditionValues = new List<bool>();

        foreach (var condition in showIfAttribute.Conditions)
        {
            FieldInfo conditionField = GetField(target, condition);
            if (conditionField != null &&
                conditionField.FieldType == typeof(bool))
            {
                conditionValues.Add((bool)conditionField.GetValue(target));
            }

            MethodInfo conditionMethod = GetMethod(target, condition);
            if (conditionMethod != null &&
                conditionMethod.ReturnType == typeof(bool) &&
                conditionMethod.GetParameters().Length == 0)
            {
                conditionValues.Add((bool)conditionMethod.Invoke(target, null));
            }
        }

        if (conditionValues.Count > 0)
        {
            bool met;
            if (showIfAttribute.Operator == ConditionOperator.And)
            {
                met = true;
                foreach (var value in conditionValues)
                {
                    met = met && value;
                }
            }
            else
            {
                met = false;
                foreach (var value in conditionValues)
                {
                    met = met || value;
                }
            }
            return met;
        }
        else
        {
            Debug.LogError("Invalid boolean condition fields or methods used!");
            return true;
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent 
                 label)
    {
        // Calcluate the property height, if we don't meet the condition and the
        // draw mode is DontDraw, then height will be 0.
        bool meetsCondition = MeetsConditions(property);
        var showIfAttribute = this.attribute as ShowIfAttribute;

        if (!meetsCondition && showIfAttribute.Action == 
            ActionOnConditionFail.DontDraw)
            return 0;

        if (property.hasVisibleChildren && property.isExpanded)
        {
            float sum = 0f;
            var it = property.Copy();
            int depth = it.depth;
            while (it.NextVisible(true)
                   && it.depth> depth)
            {
                sum += EditorGUI.GetPropertyHeight(it);
            }

            return base.GetPropertyHeight(property, label) + sum + 30f;
        }

        return base.GetPropertyHeight(property, label);
        
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent 
           label)
    {
        bool meetsCondition = MeetsConditions(property);
        // Early out, if conditions met, draw and go.
        if (meetsCondition)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return; 
        }

        var showIfAttribute = this.attribute as ShowIfAttribute;
        if(showIfAttribute.Action == ActionOnConditionFail.DontDraw)
        {
            return;
        }
        else if (showIfAttribute.Action == ActionOnConditionFail.JustDisable)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndDisabledGroup();
        }

    }
}
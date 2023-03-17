using System;
using System.Collections.Generic;
using StateManager;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Services
{
    public class CombatSystemEditor : EditorWindow
    {

        private DropdownField _effectDropDownField;
        private Button _addEffectButton;
        private Button _removeEffectButton;
        private VisualElement _statusEffectList;
        
        private ApplicationStateManager _applicationStateManager;
        private List<StatusEffect> _effects;
        private List<string> _statusEffectNames;
        private TypeCache.TypeCollection _types;
    
        [MenuItem("CombatSystem/CombatSystem Debug Window")]
        public static void OpenWindow()
        {
            CombatSystemEditor wnd = GetWindow<CombatSystemEditor>();
            wnd.titleContent = new GUIContent("CombatSystemEditorEditor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/CombatSystem/Editor/CombatSystemEditor.uxml");
            VisualElement labelFromUxml = visualTree.Instantiate();
            root.Add(labelFromUxml);
        
            //Get visual elements
            _statusEffectList = root.Q<VisualElement>("StateStackList");
            _addEffectButton = root.Q<Button>("PushStateButton");
            _removeEffectButton = root.Q<Button>("PopStateButton");
            _effectDropDownField = root.Q<DropdownField>("StateToPushDropDown");
            
            //get all status effect Scriptable Objects
            string[] guids = AssetDatabase.FindAssets("t:"+ nameof(StatusEffect));
            _effects = new List<StatusEffect>();
            for(int i =0;i<guids.Length;i++)         //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                _effects.Add(AssetDatabase.LoadAssetAtPath<StatusEffect>(path));
            }
            
            //put all status effect names into dropdown list
            _statusEffectNames = new List<string>();
            foreach (var effect in _effects)
            {
                _statusEffectNames.Add(effect.name);
            }
            _effectDropDownField.choices = _statusEffectNames;
            
            //binds buttons to functions
            _removeEffectButton.clicked += RemoveEffectButtonClicked;
            _addEffectButton.clicked += AddEffectButtonClicked;
            
        }
        
        private void OnDestroy()
        {
            //Stop listening for buttons
            _removeEffectButton.clicked -= RemoveEffectButtonClicked;
            _addEffectButton.clicked -= AddEffectButtonClicked;
        }

        private void OnInspectorUpdate()
        {
            RefreshStatusEffectListVisual();
        }

        private void RefreshStatusEffectListVisual()
        {
            GameObject player = GameObject.Find("Player");
            CombatSystem system = player.GetComponent<CombatSystem>();
            if (!system) { return; }
            //Empty list
            for (int i = _statusEffectList.childCount - 1; i >= 0; i--)
            {
                _statusEffectList.RemoveAt(i);
            }
            
            var statusEffects = system.GetStatusEffects();
            for (int i = statusEffects.Count - 1; i >= 0; i--)
            {
                var field = new TextField
                {
                    value = statusEffects[i].name
                };
                _statusEffectList.Insert(0,field); 
            }
        }

        private void AddEffectButtonClicked()
        {
            if (!EditorApplication.isPlaying)
            {
                Debug.LogError("Cannot change state while game isn't running");
                return;
            }
            GameObject player = GameObject.Find("Player");
            if (!player)
            {
                Debug.LogError("Player Not Found");
                return;
            }
            CombatSystem system = player.GetComponent<CombatSystem>();
            if (_effectDropDownField.index < 0 || _effectDropDownField.index >= _effects.Count)
            {
                Debug.LogWarning("Tried to add Status Effect that was out of array range");
                return;
            }
            system.ApplyStatusEffect(Instantiate(_effects[_effectDropDownField.index]));
        }
        private void RemoveEffectButtonClicked()
        {
            if (!EditorApplication.isPlaying)
            {
                Debug.LogError("Cannot change state while game isn't running");
                return;
            }else if (_applicationStateManager == null)
            {
                Debug.LogError("Application State Manager not set");
                return;
            }
        
            _applicationStateManager.PopState();
        }
    }
}
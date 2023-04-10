using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
public class CombatSystemEditor : EditorWindow
{

    private DropdownField _effectDropDownField;
    private Button _addEffectButton;
    private Button _removeEffectButton;
    private ListView _statusEffectList;
    private Label _gameObjectNameLabel;

    private Label _healthCurrentValue;
    private Label _healthBaseValue;
    private Label _healthMaxCurrentValue;
    private Label _healthMaxBaseValue;
    private Label _healthRegenCurrentValue;
    private Label _healthRegenBaseValue;
    private Label _manaCurrentValue;
    private Label _manaBaseValue;
    private Label _manaMaxCurrentValue;
    private Label _manaMaxBaseValue;
    private Label _manaRegenCurrentValue;
    private Label _manaRegenBaseValue;
    private Label _attackSpeedCurrentValue;
    private Label _attackSpeedBaseValue;
    private Label _attackPowerCurrentValue;
    private Label _attackPowerBaseValue;
    private Label _magicPowerCurrentValue;
    private Label _magicPowerBaseValue;
    private Label _physicalDefenseCurrentValue;
    private Label _physicalDefenseBaseValue;
    private Label _magicalDefenseCurrentValue;
    private Label _magicalDefenseBaseValue;
    private Label _moveSpeedCurrentValue;
    private Label _moveSpeedBaseValue;
    
    private List<StatusEffect> _statusEffectsScriptableObjects;
    private List<string> _statusEffectScriptableObjectNames;

    private CombatSystem _lastSelectedCombatSystem;
    private AttributeSet _attributeSet;
    private List<StatusEffect> _activeStatusEffects;

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
        _statusEffectList = root.Q<ListView>("EffectList");
        _addEffectButton = root.Q<Button>("AddEffectButton");
        _removeEffectButton = root.Q<Button>("RemoveEffectButton");
        _effectDropDownField = root.Q<DropdownField>("EffectToAddDropDown");
        _gameObjectNameLabel = root.Q<Label>("GameobjectName");
        
        _healthCurrentValue = root.Q<Label>("HealthCurrentValue");
        _healthBaseValue = root.Q<Label>("HealthBaseValue");
        _healthMaxCurrentValue = root.Q<Label>("MaxHealthCurrentValue");
        _healthMaxBaseValue = root.Q<Label>("MaxHealthBaseValue");
        _healthRegenCurrentValue = root.Q<Label>("HealthRegenCurrentValue");
        _healthRegenBaseValue = root.Q<Label>("HealthRegenBaseValue");
        
        _manaCurrentValue = root.Q<Label>("ManaCurrentValue");
        _manaBaseValue = root.Q<Label>("ManaBaseValue");
        _manaMaxCurrentValue = root.Q<Label>("MaxManaCurrentValue");
        _manaMaxBaseValue = root.Q<Label>("MaxManaBaseValue");
        _manaRegenCurrentValue = root.Q<Label>("ManaRegenCurrentValue");
        _manaRegenBaseValue = root.Q<Label>("ManaRegenBaseValue");

        _attackSpeedCurrentValue = root.Q<Label>("AttackSpeedCurrentValue");
        _attackSpeedBaseValue = root.Q<Label>("AttackSpeedBaseValue");
        _attackPowerCurrentValue = root.Q<Label>("AttackPowerBaseValue");
        _attackPowerBaseValue = root.Q<Label>("AttackPowerBaseValue");
        
        _magicPowerCurrentValue = root.Q<Label>("MagicPowerCurrentValue");
        _magicPowerBaseValue = root.Q<Label>("MagicPowerBaseValue");

        _physicalDefenseCurrentValue = root.Q<Label>("PhysicalDefenseCurrentValue");
        _physicalDefenseBaseValue = root.Q<Label>("PhysicalDefenseBaseValue");
        _magicalDefenseCurrentValue = root.Q<Label>("MagicalDefenseCurrentValue");
        _magicalDefenseBaseValue = root.Q<Label>("MagicalDefenseBaseValue");
        
        _moveSpeedCurrentValue = root.Q<Label>("MoveSpeedCurrentValue");
        _moveSpeedBaseValue = root.Q<Label>("MoveSpeedBaseValue");
        
        _gameObjectNameLabel.name = "No Object Selected!";

        //get all status effect Scriptable Objects
        string[] guids = AssetDatabase.FindAssets("t:"+ nameof(StatusEffect));
        _statusEffectsScriptableObjects = new List<StatusEffect>();
        for(int i =0;i<guids.Length;i++)         //probably could get optimized 
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            _statusEffectsScriptableObjects.Add(AssetDatabase.LoadAssetAtPath<StatusEffect>(path));
        }
        
        //put all status effect names into dropdown list
        _statusEffectScriptableObjectNames = new List<string>();
        foreach (var effect in _statusEffectsScriptableObjects)
        {
            _statusEffectScriptableObjectNames.Add(effect.name);
        }
        _effectDropDownField.choices = _statusEffectScriptableObjectNames;

        
        //bind create list item function
        _statusEffectList.makeItem += MakeStatusEffectListItem;
        _statusEffectList.bindItem += BindItem;
        _statusEffectList.itemsSource = _activeStatusEffects;
        _statusEffectList.fixedItemHeight = 16f;


        //binds buttons to functions
        _removeEffectButton.clicked += RemoveEffectButtonClicked;
        _addEffectButton.clicked += AddEffectButtonClicked;
        
        EditorApplication.playModeStateChanged += change =>
        {
            if (change == PlayModeStateChange.ExitingPlayMode)
            {
                _lastSelectedCombatSystem = null;
                _gameObjectNameLabel.text = "No Object Selected";
            }
        };
    }
    private void OnDestroy()
    {
        //Stop listening for buttons
        _removeEffectButton.clicked -= RemoveEffectButtonClicked;
        _addEffectButton.clicked -= AddEffectButtonClicked;
    }
    private void OnInspectorUpdate()
    {
        
        if (!EditorApplication.isPlaying) { return; }

        if (_lastSelectedCombatSystem == null)
        {
            GameObject[] gameObjects = new GameObject[1]; 
            gameObjects[0]= GameObject.Find("Player");
            newCombatSystemSelected(gameObjects); 
        }
        else if(Selection.gameObjects.Length > 0 &&
                !Selection.gameObjects.Contains(_lastSelectedCombatSystem.gameObject))
        {
            newCombatSystemSelected(Selection.gameObjects);
        }
        else
        {
            Refresh();
        }
    }

    private void newCombatSystemSelected(GameObject[] gameObjects)
    {
        foreach (var gameObject in gameObjects)
        {
            CombatSystem combatSystem= gameObject.GetComponent<CombatSystem>();
            if (combatSystem != null &&
                combatSystem != _lastSelectedCombatSystem)
            {
                _gameObjectNameLabel.text = gameObject.name;
                _lastSelectedCombatSystem = combatSystem;
                _attributeSet = gameObject.GetComponent<AttributeSet>();
                _statusEffectList.itemsSource = _lastSelectedCombatSystem.GetStatusEffects();
                _lastSelectedCombatSystem.StatusEffectAdded += Refresh;
                _lastSelectedCombatSystem.StatusEffectRemoved += Refresh;
                Refresh();
                return;
            }
        }
    }

    private void Refresh()
    {
        _statusEffectList.RefreshItems();

        _healthCurrentValue.text = _attributeSet.currentHealth.CurrentValue.ToString();
        _healthBaseValue.text = _attributeSet.currentHealth.BaseValue.ToString();
        _healthMaxCurrentValue.text = _attributeSet.maxHealth.CurrentValue.ToString();
        _healthMaxBaseValue.text = _attributeSet.maxHealth.BaseValue.ToString();
        _healthRegenCurrentValue.text = _attributeSet.healthRegen.CurrentValue.ToString();
        _healthRegenBaseValue.text = _attributeSet.healthRegen.BaseValue.ToString();
        
        _manaCurrentValue.text = _attributeSet.currentMana.CurrentValue.ToString();
        _manaBaseValue.text = _attributeSet.currentMana.BaseValue.ToString();
        _manaMaxCurrentValue.text = _attributeSet.maxMana.CurrentValue.ToString();
        _manaMaxBaseValue.text = _attributeSet.maxMana.BaseValue.ToString();
        _manaRegenCurrentValue.text = _attributeSet.manaRegen.CurrentValue.ToString();
        _manaRegenBaseValue.text = _attributeSet.manaRegen.BaseValue.ToString();

        _attackSpeedCurrentValue.text = _attributeSet.attackSpeed.CurrentValue.ToString();
        _attackSpeedBaseValue.text = _attributeSet.attackSpeed.BaseValue.ToString();
        _attackPowerCurrentValue.text = _attributeSet.attackPower.CurrentValue.ToString();
        _attackPowerBaseValue.text = _attributeSet.attackPower.BaseValue.ToString();
        
        _magicPowerCurrentValue.text = _attributeSet.magicPower.CurrentValue.ToString();
        _magicPowerBaseValue.text = _attributeSet.magicPower.BaseValue.ToString();

        _physicalDefenseCurrentValue.text = _attributeSet.physicalDefense.CurrentValue.ToString();
        _physicalDefenseBaseValue.text = _attributeSet.physicalDefense.BaseValue.ToString();
        _magicalDefenseCurrentValue.text = _attributeSet.magicalDefense.CurrentValue.ToString();
        _magicalDefenseBaseValue.text = _attributeSet.magicalDefense.BaseValue.ToString();
        
        _moveSpeedCurrentValue.text = _attributeSet.moveSpeed.CurrentValue.ToString();
        _moveSpeedBaseValue.text = _attributeSet.moveSpeed.BaseValue.ToString();
    }

    VisualElement MakeStatusEffectListItem()
    {
        Label label = new Label
        {
            style =
            {
                flexGrow = 1f,
                alignContent = new StyleEnum<Align>(Align.Stretch)
            }
        };
        return label;
    }

    void BindItem(VisualElement e, int i)
    {
        if (_lastSelectedCombatSystem == null) { return; }
        ((Label)e).text = _lastSelectedCombatSystem.GetStatusEffects()[i].EffectName;
    }

    private void AddEffectButtonClicked()
    {
        if (!EditorApplication.isPlaying)
        {
            Debug.LogError("Cannot change state while game isn't running");
            return;
        }

        if (!_lastSelectedCombatSystem)
        {
            Debug.LogError("No Combat System found");
            return;
        }
        //_lastSelectedCombatSystem.ApplyStatusEffect(Instantiate(_statusEffectsScriptableObjects[_effectDropDownField.index]));
    }
    private void RemoveEffectButtonClicked()
    {
        if (!EditorApplication.isPlaying)
        {
            Debug.LogError("Cannot change state while game isn't running");
            return;
        }

        if (_lastSelectedCombatSystem == null)
        {
            Debug.LogError("Can't remove effects with no CombatSystem Selected");
            return;
        }
        _lastSelectedCombatSystem.RemoveStatusEffect(_lastSelectedCombatSystem.GetStatusEffects()[_statusEffectList.selectedIndex]);
    }
}

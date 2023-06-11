
using Services;
using TMPro;
using UnityEngine;

public class UIBarText : MonoBehaviour
{
    [SerializeField] private bool isHealth;
    
    private TMP_Text _text;
    private ModifiableAttributeValue _maxAttribute;
    private ModifiableAttributeValue _currentAttribute;
    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        
        GameObject player = ServiceLocator.Instance.Get<PlayerManager>().GetPlayer();
        if (player == null) return;
        
        AttributeSet attributeSet= player.GetComponent<AttributeSet>();
        
        if (isHealth)
        {
            _currentAttribute = attributeSet.currentHealth;
            _maxAttribute = attributeSet.maxHealth;
            
            attributeSet.currentHealth.OnValueChanged += OnValueChanged;
            attributeSet.maxHealth.OnValueChanged += OnValueChanged;
        }
        else
        {
            _currentAttribute = attributeSet.currentMana;
            _maxAttribute = attributeSet.maxMana;
            
            attributeSet.currentMana.OnValueChanged += OnValueChanged;
            attributeSet.maxMana.OnValueChanged += OnValueChanged;
        }
    }

    private void OnValueChanged(ModifiableAttributeValue arg1, float arg2)
    {
        _text.text = Mathf.FloorToInt(_currentAttribute.CurrentValue) + "/" + Mathf.FloorToInt(_maxAttribute.CurrentValue);
    }

    private void OnEnable()
    {
        ServiceLocator.Instance.Get<PlayerManager>().OnPlayerSpawned += OnPlayerSpawned;
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.Get<PlayerManager>().OnPlayerSpawned -= OnPlayerSpawned;
    }

    private void OnPlayerSpawned(GameObject player)
    {
        AttributeSet attributeSet= player.GetComponent<AttributeSet>();
        
        if (isHealth)
        {
            _currentAttribute = attributeSet.currentHealth;
            _maxAttribute = attributeSet.maxHealth;
            
            attributeSet.currentHealth.OnValueChanged += OnValueChanged;
            attributeSet.maxHealth.OnValueChanged += OnValueChanged;
        }
        else
        {
            _currentAttribute = attributeSet.currentMana;
            _maxAttribute = attributeSet.maxMana;
            
            attributeSet.currentMana.OnValueChanged += OnValueChanged;
            attributeSet.maxMana.OnValueChanged += OnValueChanged;
        }
    }
}

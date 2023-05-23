using System;
using Services;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UIFill : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private bool lerp;
    [SerializeField] private bool isHealth;
    [SerializeField] private float lerpRate = 1f;

    private ModifiableAttributeValue maxAttribute;
    private ModifiableAttributeValue currentAttribute;

    private float _targetFillAmount = 0f;
    private void Start()
    {
        GameObject player = ServiceLocator.Instance.Get<PlayerManager>().GetPlayer();
        if (player == null) return;
        
        AttributeSet attributeSet= player.GetComponent<AttributeSet>();
        
        if (isHealth)
        {
            currentAttribute = attributeSet.currentHealth;
            maxAttribute = attributeSet.maxHealth;
            
            attributeSet.currentHealth.OnValueChanged += OnValueChanged;
            attributeSet.maxHealth.OnValueChanged += OnValueChanged;
        }
        else
        {
            currentAttribute = attributeSet.currentMana;
            maxAttribute = attributeSet.maxMana;
            
            attributeSet.currentMana.OnValueChanged += OnValueChanged;
            attributeSet.maxMana.OnValueChanged += OnValueChanged;
        }
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
            currentAttribute = attributeSet.currentHealth;
            maxAttribute = attributeSet.maxHealth;
            
            attributeSet.currentHealth.OnValueChanged += OnValueChanged;
            attributeSet.maxHealth.OnValueChanged += OnValueChanged;
        }
        else
        {
            currentAttribute = attributeSet.currentMana;
            maxAttribute = attributeSet.maxMana;
            
            attributeSet.currentMana.OnValueChanged += OnValueChanged;
            attributeSet.maxMana.OnValueChanged += OnValueChanged;
        }
    }
    private void OnValueChanged(ModifiableAttributeValue attribute)
    {
        
        _targetFillAmount = currentAttribute.CurrentValue/maxAttribute.CurrentValue;
    }

    private void Update()
    {
        if (lerp)
        {
            image.fillAmount = Mathf.Lerp(image.fillAmount, _targetFillAmount, lerpRate * Time.deltaTime);
        }
        else
        {
            image.fillAmount = _targetFillAmount;
        }
    }
    
}

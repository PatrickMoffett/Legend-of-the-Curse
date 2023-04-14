using System;
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
        GameObject player = GameObject.Find("Player");
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

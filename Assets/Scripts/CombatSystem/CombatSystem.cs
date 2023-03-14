using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttributeSet))]
public class CombatSystem : MonoBehaviour
{
    private AttributeSet _attributeSet;

    private List<StatusEffect> _currentStatusEffects = new List<StatusEffect>();
    // Start is called before the first frame update
    void Start()
    {
        _attributeSet = GetComponent<AttributeSet>();
    }

    public void ApplyStatusEffect(StatusEffect effectToApply)
    {
        _currentStatusEffects.Add(effectToApply);
        foreach (var modifier in effectToApply.attributeModifiers)
        {
            _attributeSet.ApplyModifier(modifier);
        }
    }

    public void RemoveStatusEffect(StatusEffect effectToRemove)
    {
        _currentStatusEffects.Remove(effectToRemove);
        foreach (var modifier in effectToRemove.attributeModifiers)
        {
            _attributeSet.RemoveModifier(modifier);
        }
    }
}

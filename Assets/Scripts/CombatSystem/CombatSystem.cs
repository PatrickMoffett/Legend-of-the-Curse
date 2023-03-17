using System.Collections;
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

    // ReSharper disable Unity.PerformanceAnalysis
    public void ApplyStatusEffect(StatusEffect effectToApply)
    {
        switch (effectToApply.durationType)
        {
            case StatusEffect.DurationType.Duration:
                _currentStatusEffects.Add(effectToApply); ;
                foreach (var modifier in effectToApply.attributeModifiers)
                {
                    _attributeSet.ApplyModifier(modifier);
                }
                StartCoroutine(WaitToRemoveStatusEffect(effectToApply));
                break;
            
            case StatusEffect.DurationType.Infinite:
                _currentStatusEffects.Add(effectToApply);
                foreach (var modifier in effectToApply.attributeModifiers)
                {
                    _attributeSet.ApplyModifier(modifier);
                }
                break;
            
            case StatusEffect.DurationType.Instant:
                foreach (var modifier in effectToApply.attributeModifiers)
                {
                    _attributeSet.ApplyInstantModifier(modifier);
                }
                break;
            default:
                
                Debug.LogError("Unexpected effect type in ApplyStatusEffect");
                break;
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

    private IEnumerator WaitToRemoveStatusEffect(StatusEffect effectToRemove)
    {
        yield return new WaitForSeconds(effectToRemove.duration);
        RemoveStatusEffect(effectToRemove);
    }
}

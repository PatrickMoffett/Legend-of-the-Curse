using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AttributeSet))]
public class CombatSystem : MonoBehaviour
{
    private AttributeSet _attributeSet;

    private readonly List<StatusEffectInstance> _currentStatusEffects = new List<StatusEffectInstance>();

    public event Action StatusEffectAdded;
    public event Action StatusEffectRemoved;
    // Start is called before the first frame update
    void Start()
    {
        _attributeSet = GetComponent<AttributeSet>();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void ApplyStatusEffect(StatusEffectInstance effectToApply)
    {
        //set this as the targetCombatSystem
        effectToApply.SetTargetCombatSystem(this);

        if (effectToApply.DurationType == StatusEffect.DurationType.Instant)
        {
            //apply all modifiers instantly
            foreach (var modifier in effectToApply.AttributeModifiers)
            {
                _attributeSet.ApplyInstantModifier(modifier);
            }
        }
        else
        {
            //if the effect has a duration start a coroutine to remove the effect when it's done.
            if (effectToApply.DurationType == StatusEffect.DurationType.Duration)
            {
                StartCoroutine(WaitToRemoveStatusEffect(effectToApply));
            }
            
            //Add it to the list of current status effects
            _currentStatusEffects.Add(effectToApply);

            //if the effect happens periodically, start a coroutine to do that
            if (effectToApply.IsPeriodic)
            {
                StartCoroutine(ApplyPeriodicEffect(effectToApply));
            }
            else
            {
                //otherwise add a modifier to each affected attribute
                foreach (var modifier in effectToApply.AttributeModifiers)
                {
                    _attributeSet.ApplyModifier(modifier);
                }
            }
        }
        StatusEffectAdded?.Invoke();
    }

    private IEnumerator ApplyPeriodicEffect(StatusEffectInstance effectToApply)
    {
        //While we have this effect
        while (_currentStatusEffects.Contains(effectToApply)){
            //Apply Modifiers instantly
            foreach (var modifier in effectToApply.AttributeModifiers)
            {
                _attributeSet.ApplyInstantModifier(modifier);
            }
            //and wait periodic rate before doing it again
            yield return new WaitForSeconds(effectToApply.PeriodicRate);
        }
    }

    public void RemoveStatusEffect(StatusEffectInstance effectToRemove)
    {
        _currentStatusEffects.Remove(effectToRemove);
        foreach (var modifier in effectToRemove.AttributeModifiers)
        {
            _attributeSet.RemoveModifier(modifier);
        }
        StatusEffectRemoved?.Invoke();
    }

    public List<StatusEffectInstance> GetStatusEffects()
    {
        return _currentStatusEffects;
    }
    private IEnumerator WaitToRemoveStatusEffect(StatusEffectInstance effectToRemove)
    {
        yield return new WaitForSeconds(effectToRemove.Duration);
        RemoveStatusEffect(effectToRemove);
    }

    public AttributeSet GetAttributeSet()
    {
        return _attributeSet;
    }
}

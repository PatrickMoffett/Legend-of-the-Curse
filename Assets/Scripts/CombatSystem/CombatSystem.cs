using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttributeSet))]
public class CombatSystem : MonoBehaviour
{
    private AttributeSet _attributeSet;

    private List<StatusEffect> _currentStatusEffects = new List<StatusEffect>();

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
    public void ApplyStatusEffect(StatusEffect effectToApply)
    {

        if (effectToApply.durationType == StatusEffect.DurationType.Instant)
        {
            //apply all modifiers instantly
            foreach (var modifier in effectToApply.attributeModifiers)
            {
                _attributeSet.ApplyInstantModifier(modifier);
            }
        }
        else
        {
            //Add it to the list of current status effects
            _currentStatusEffects.Add(effectToApply);
            
            //if the effect has a duration start a coroutine to remove the effect when it's done.
            if (effectToApply.durationType == StatusEffect.DurationType.Duration)
            {
                StartCoroutine(WaitToRemoveStatusEffect(effectToApply));
            }
            
            //if the effect happens periodically, start a coroutine to do that
            if (effectToApply.isPeriodic)
            {
                StartCoroutine(ApplyPeriodicEffect(effectToApply));
            }
            else
            {
                //otherwise add a modifier to each affected attribute
                foreach (var modifier in effectToApply.attributeModifiers)
                {
                    _attributeSet.ApplyModifier(modifier);
                }
            }
        }
        StatusEffectAdded?.Invoke();
    }

    private IEnumerator ApplyPeriodicEffect(StatusEffect effectToApply)
    {
        //While we have this effect
        while (_currentStatusEffects.Contains(effectToApply)){
            //Apply Modifiers instantly
            foreach (var modifier in effectToApply.attributeModifiers)
            {
                _attributeSet.ApplyInstantModifier(modifier);
            }
            //and wait periodic rate before doing it again
            yield return new WaitForSeconds(effectToApply.periodicRate);
        }
    }

    public void RemoveStatusEffect(StatusEffect effectToRemove)
    {
        _currentStatusEffects.Remove(effectToRemove);
        foreach (var modifier in effectToRemove.attributeModifiers)
        {
            _attributeSet.RemoveModifier(modifier);
        }
        StatusEffectRemoved?.Invoke();
    }

    public List<StatusEffect> GetStatusEffects()
    {
        return _currentStatusEffects;
    }
    private IEnumerator WaitToRemoveStatusEffect(StatusEffect effectToRemove)
    {
        yield return new WaitForSeconds(effectToRemove.duration);
        RemoveStatusEffect(effectToRemove);
    }
}

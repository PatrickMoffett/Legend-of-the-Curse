using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    private List<StatusEffectInstance> _effectsToApply = new List<StatusEffectInstance>();
    private void OnCollisionEnter2D(Collision2D col)
    {
        CombatSystem combatSystem = col.gameObject.GetComponent<CombatSystem>();
        if (combatSystem)
        {
            foreach (var effect in _effectsToApply)
            {
                combatSystem.ApplyStatusEffect(effect);
            }
        }
        Destroy(gameObject);
    }

    public void AddStatusEffects(List<StatusEffectInstance> effects)
    {
        _effectsToApply.AddRange(effects);
    }
}

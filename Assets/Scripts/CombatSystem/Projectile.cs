using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [SerializeField]private List<StatusEffect> effectsToApply;
    private void OnCollisionEnter2D(Collision2D col)
    {
        CombatSystem combatSystem = col.gameObject.GetComponent<CombatSystem>();
        if (combatSystem)
        {
            foreach (var effect in effectsToApply)
            {
                combatSystem.ApplyStatusEffect(effect);
            }
        }
        Destroy(gameObject);
    }

    public void AddStatusEffects(List<StatusEffect> effects)
    {
        effectsToApply.AddRange(effects);
    }
}

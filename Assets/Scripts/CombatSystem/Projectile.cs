using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    private List<OutgoingStatusEffectInstance> _effectsToApply = new List<OutgoingStatusEffectInstance>();

    private static GameObject _bucket;
    private void Start()
    {
        if (!_bucket)
        {
            _bucket = new GameObject("Projectile_Bucket");
        }
        transform.parent = _bucket.transform;
    }

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

    public void AddStatusEffects(List<OutgoingStatusEffectInstance> effects)
    {
        _effectsToApply.AddRange(effects);
    }
}

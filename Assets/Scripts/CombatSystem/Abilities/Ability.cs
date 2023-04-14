using UnityEditor;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public bool passiveAbility = false;

    protected GameObject _owner;
    protected AttributeSet _attributes;
    protected CombatSystem _combatSystem;

    public StatusEffect activationCost;
    public StatusEffect cooldown;

    private StatusEffectInstance _appliedCooldown;
    
    public void Initialize(GameObject owner)
    {
        _owner = owner;
        _combatSystem = _owner.GetComponent<CombatSystem>();
        _attributes = _owner.GetComponent<AttributeSet>();
    }
    public bool TryActivate(Vector2 direction)
    {
        if ((_appliedCooldown == null || !_combatSystem.GetStatusEffects().Contains(_appliedCooldown)) 
            &&
            (activationCost == null || _combatSystem.TryActivationCost(activationCost)))
        {
            Activate(direction);
            
            if (cooldown != null)
            {
                OutgoingStatusEffectInstance effect = new OutgoingStatusEffectInstance(cooldown, _combatSystem);
                _appliedCooldown = _combatSystem.ApplyStatusEffect(effect);
            }

            return true;
        }
        else
        {
            return false;
        }
    }
    protected abstract void Activate(Vector2 direction);
}

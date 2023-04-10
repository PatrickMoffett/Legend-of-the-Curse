using UnityEditor;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public bool passiveAbility = false;

    protected GameObject _owner;
    protected AttributeSet _attributes;
    protected CombatSystem _combatSystem;
    
    public void Initialize(GameObject owner)
    {
        _owner = owner;
        _combatSystem = _owner.GetComponent<CombatSystem>();
        _attributes = _owner.GetComponent<AttributeSet>();
    }
    public abstract void Activate(Vector2 direction);
}

using UnityEditor;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public bool passiveAbility = false;

    protected GameObject _owner;
    protected AttributeSet _attributes;
    
    public void Initialize(GameObject owner)
    {
        this._owner = owner;
        _attributes = _owner.GetComponent<AttributeSet>();
    }
    public abstract void Activate(Vector2 direction);
}

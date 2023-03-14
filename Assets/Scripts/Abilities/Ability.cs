using UnityEditor;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public bool passiveAbility = false;

    protected GameObject _owner;
    
    public void Initialize(GameObject owner)
    {
        this._owner = owner;
    }
    public abstract void Activate();
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObject/StatusEffect/Default")]
public class StatusEffect : ScriptableObject
{
    public enum DurationType
    {
        Instant, // Applies instantly
        Duration, // Lasts for Duration amount or until removed manually
        Infinite // Lasts until removed manually
    }
    /// <summary>
    /// The duration type of the effect
    /// </summary>
    public DurationType durationType = DurationType.Instant;
    /// <summary>
    /// The Duration of the effect (Only applies to effects with DurationType = Duration)
    /// </summary>
    public float duration = 0f;
    /// <summary>
    /// Whether or not to continually reapply the effect (Only applies to effects with DurationType = Duration)
    /// </summary>
    public bool appliesPeriodically = false;
    /// <summary>
    /// How often to reapply the effect
    /// </summary>
    public float periodicRate = 0f;
    /// <summary>
    /// List of attribute modifiers this effect applies
    /// </summary>
    public List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();
    /// <summary>
    /// List of abilities granted while this effect is active
    /// </summary>
    public List<Ability> grantedAbilities = new List<Ability>();

    public virtual void OnApply()
    {
        
    }
    public virtual void OnRemove()
    {
        
    }
}

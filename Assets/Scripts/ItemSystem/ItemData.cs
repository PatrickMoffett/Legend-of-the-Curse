using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName=("ItemSystem/ItemData"))]
public class ItemData : ScriptableObject
{
    public string displayName;

    public string description;

    public Sprite sprite;

    public List<StatusEffect> itemEffects;
}

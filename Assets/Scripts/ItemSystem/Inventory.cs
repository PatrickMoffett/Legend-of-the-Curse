
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemData> items = new List<ItemData>();

    private CombatSystem _combatSystem;
    private void Start()
    {
        _combatSystem = GetComponent<CombatSystem>();
    }

    public void AddItem(ItemData itemToAdd)
    {
        if (!itemToAdd.itemConsumedOnPickup)
        {
            items.Add(itemToAdd);
        }

        foreach (var effect in itemToAdd.itemEffects)
        {
            _combatSystem.ApplyStatusEffect(new OutgoingStatusEffectInstance(effect, _combatSystem));
        }
    }

}


using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName="ItemSystem/DropTable")]
public class DropTable : ScriptableObject, IResetOnExitPlay
{
    public ItemFloatDictionary dropTable;

    private float sum = 0;
    private void OnEnable()
    {
        ResetTable();
    }

    private void OnDisable()
    {
        ResetTable();
    }

    private void OnValidate()
    {
        ResetTable();
    }

    private void ResetTable()
    {
        sum = 0f;
        foreach (var entry in dropTable)
        {
            sum += entry.Value;
        }
    }

    public ItemData GetDrop()
    {
        float random = Random.Range(0f, sum);
        float currentSum = 0f;
        foreach (var entry in dropTable)
        {
            currentSum += entry.Value;
            if (currentSum >= random)
            {
                return entry.Key;
            }
        }
        Debug.LogError("We somehow didn't get an item from the generator");
        return null;
    }

    public void ResetOnExitPlay()
    {
        ResetTable();
    }
}

using System;
using UnityEngine;

public class PlayerStatistics : ScriptableObject 
{
    [SerializeField] public int shots;
    [SerializeField] public int hits;
    [SerializeField] public int steps;
    [SerializeField] public float totalDistance;
    [SerializeField] public int kills;

    public void Reset()
    {
        shots = 0;
        hits = 0;
        steps = 0;
        totalDistance = 0;
        kills = 0;
    }
}


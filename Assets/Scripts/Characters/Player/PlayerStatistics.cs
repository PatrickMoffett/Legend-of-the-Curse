using UnityEngine;

public class PlayerStatistics : ScriptableObject 
{
    [SerializeField] public int shots;
    [SerializeField] public int hits;
    [SerializeField] public int steps;
    [SerializeField] public float totalDistance;
    [SerializeField] public int deaths;
    [SerializeField] public int kills;

}


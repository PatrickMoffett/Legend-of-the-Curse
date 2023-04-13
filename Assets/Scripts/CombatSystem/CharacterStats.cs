using UnityEngine;

public class CharacterStats : ScriptableObject 
{
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
    [SerializeField] public int shots;
    [SerializeField] public int hits;
    [SerializeField] public int steps;
    [SerializeField] public float totalDistance;
}


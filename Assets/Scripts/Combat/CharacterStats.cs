using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Character/CharacterStats", order = 0)]
public class CharacterStats : ScriptableObject 
{
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
}



using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName="Settings/LevelSettings")]
public class LevelsSettings : ScriptableObject
{
    public List<LevelGenerator> levels = new List<LevelGenerator>();
}

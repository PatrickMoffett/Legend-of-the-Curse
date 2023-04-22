using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelGeneration
{
    [CreateAssetMenu(menuName="LevelGenerator/Test")]
    public class LevelGeneratorSettings : ScriptableObject
    {
        public BoundsInt levelSizeBounds;
    
        public TileBase connectionPathTile;
        public TileBase connectionWallTile;
        public int connectionPathWidth = 1;
    }
}
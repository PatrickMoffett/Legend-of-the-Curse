
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class PrefabPlacer : MonoBehaviour
{
    public GameObject prefabToPlace;
    public Vector3Int _position;

    private Dictionary<string, Tilemap> _tilemaps = new Dictionary<string, Tilemap>();

    // Start is called before the first frame update
    void Start()
    {
        Tilemap[] tilemapsInScene = FindObjectsOfType<Tilemap>();
        foreach (Tilemap tilemap in tilemapsInScene)
        {
            _tilemaps.Add(tilemap.name,tilemap);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestDraw()
    {
        Tilemap[] tilemapsInPrefab = prefabToPlace.GetComponentsInChildren<Tilemap>();
        int xMin = int.MaxValue;
        int xMax = int.MinValue;
        int yMin = int.MaxValue;
        int yMax = int.MinValue;
        int zMin = int.MaxValue;
        int zMax = int.MinValue;
        foreach (var tilemap in tilemapsInPrefab)
        {
            xMax = xMax > tilemap.cellBounds.xMax ? xMax : tilemap.cellBounds.xMax;
            yMax = yMax > tilemap.cellBounds.yMax ? yMax : tilemap.cellBounds.yMax;
            zMax = zMax > tilemap.cellBounds.zMax ? zMax : tilemap.cellBounds.zMax;
            
            xMin = xMin < tilemap.cellBounds.xMin ? xMin : tilemap.cellBounds.xMin;
            yMin = yMin < tilemap.cellBounds.yMin ? yMin : tilemap.cellBounds.yMin;
            zMin = zMin < tilemap.cellBounds.zMin ? zMin : tilemap.cellBounds.zMin;
            
        }

        int sizeX = xMax - xMin;
        int sizeY = yMax - yMin;
        int sizeZ = zMax - zMin;
        BoundsInt bounds = new BoundsInt(xMin, yMin, zMin, sizeX, sizeY, sizeZ);

        foreach (var tilemap in tilemapsInPrefab)
        {
            if (_tilemaps.ContainsKey(tilemap.name))
            {
                DrawTilesAtLocation(tilemap, bounds,_position, _tilemaps[tilemap.name]);
            }
            else
            {
                Debug.LogWarning("Prefab contained unknown tilemap layer");
            }
        }
        
    }

    private void DrawTilesAtLocation(Tilemap tilemapToDraw, BoundsInt bounds, Vector3Int position, Tilemap tilemapToAddTo)
    {
        TileBase[] tiles = new TileBase[(bounds.size.x * bounds.size.y * bounds.size.z)+1];
        Vector3Int[] positions = new Vector3Int[(bounds.size.x * bounds.size.y * bounds.size.z)+1];
        int xMin = bounds.xMin;
        int yMin = bounds.yMin;
        int zMin = bounds.zMin;
        int index = 0;
        for (int x = 0; x < tilemapToDraw.cellBounds.xMax - xMin; x++)
        {
            for (int y = 0; y < tilemapToDraw.cellBounds.yMax - yMin; y++)
            {
                for (int z = 0; z < tilemapToDraw.cellBounds.zMax - zMin; z++)
                {
                    index++;
                    positions[index] = new Vector3Int(x + position.x, y + position.y, z + position.z);
                    TileBase tileBase =tilemapToDraw.GetTile<TileBase>(new Vector3Int(xMin + x, yMin + y, zMin + z));
                    tiles[index] = tileBase;
                }
            }
        }
        tilemapToAddTo.SetTiles(positions,tiles);
    }

    public void DrawTilesAtLocation(Tilemap tilemapToDraw,Vector3Int position,Tilemap tilemapToAddTo)
    {
        TileBase[] tiles = new TileBase[(tilemapToDraw.size.x * tilemapToDraw.size.y * tilemapToDraw.size.z)+1];
        Vector3Int[] positions = new Vector3Int[(tilemapToDraw.size.x * tilemapToDraw.size.y * tilemapToDraw.size.z)+1];
        int xMin = tilemapToDraw.cellBounds.xMin;
        int yMin = tilemapToDraw.cellBounds.yMin;
        int zMin = tilemapToDraw.cellBounds.zMin;
        int index = 0;
        for (int x = 0; x < tilemapToDraw.cellBounds.xMax - xMin; x++)
        {
            for (int y = 0; y < tilemapToDraw.cellBounds.yMax - yMin; y++)
            {
                for (int z = 0; z < tilemapToDraw.cellBounds.zMax - zMin; z++)
                {
                    index++;
                    positions[index] = new Vector3Int(x + position.x, y + position.y, z + position.z);
                    TileBase tileBase =tilemapToDraw.GetTile<TileBase>(new Vector3Int(xMin + x, yMin + y, zMin + z));
                    tiles[index] = tileBase;
                }
            }
        }
        tilemapToAddTo.SetTiles(positions,tiles);
    }
}





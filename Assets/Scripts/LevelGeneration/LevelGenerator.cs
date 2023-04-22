using System;
using System.Collections.Generic;
using LevelGeneration;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnedRoom
{
    public string name = String.Empty;
    public BoundsInt bounds = new BoundsInt();
    public List<Vector3Int> _connectionPositions = new List<Vector3Int>();
}

public class LevelGenerator : MonoBehaviour
{
    public GameObject testPrefabToSpawn;
    public Vector3Int testSpawnPosition;
    public Vector3Int startTestPath;
    public Vector3Int finishTestPath;

    public LevelGeneratorSettings levelGeneratorSettings;

    private readonly Dictionary<string, Tilemap> _tilemaps = new Dictionary<string, Tilemap>();
    private GameObject _roomsBucket;
    private List<SpawnedRoom> _rooms = new List<SpawnedRoom>();

    // Start is called before the first frame update
    void Start()
    {
        Tilemap[] tilemapsInScene = FindObjectsOfType<Tilemap>();
        foreach (Tilemap tilemap in tilemapsInScene)
        {
            _tilemaps.Add(tilemap.name,tilemap);
        }
        _tilemaps["Connections"].gameObject.SetActive(false);
        _roomsBucket = new GameObject("Rooms");
    }

    public void TestDraw()
    {
        //add room
        SpawnedRoom newRoom = LevelGeneratorUtils.SpawnRoom(testPrefabToSpawn, testSpawnPosition,_tilemaps,_roomsBucket);
        _rooms.Add(newRoom);
        
        //add path connecting rooms
        if (_rooms.Count > 1)
        {
            //get closest connection positions
            LevelGeneratorUtils.GetClosestConnectionPositions(_rooms[^2], _rooms[^1],
                out var startPosition, out var finishPosition);

            //get path initial direction for start and finish
            Vector3Int startDirection = GetInitialPathDirection(startPosition);
            bool startHorizontal = Math.Abs(startDirection.x) != 0;
            
            Vector3Int finishDirection = GetInitialPathDirection(finishPosition);
            bool finishHorizontal = Math.Abs(finishDirection.x) != 0;

            //get additional starting positions to widen path
            int pathWidth = levelGeneratorSettings.connectionPathWidth;

            //get path
            List<Vector3Int> pathBuffer = new List<Vector3Int>();
            if (startHorizontal && finishHorizontal)
            {
                int halfXDistance = (Math.Abs(startPosition.x - finishPosition.x) + 1) / 2;
                pathBuffer.AddRange(LevelGeneratorUtils.GetHorizontalZPath(startPosition, finishPosition, pathWidth,halfXDistance));
            }
            else if(!startHorizontal && !finishHorizontal)
            {
                int halfYDistance = (Math.Abs(startPosition.x - finishPosition.x) + 1) / 2;
                pathBuffer.AddRange(LevelGeneratorUtils.GetVerticalZPath(startPosition, finishPosition, pathWidth,halfYDistance));
            }else if (startHorizontal)
            {
                pathBuffer.AddRange(LevelGeneratorUtils.GetVerticalZPath(startPosition, finishPosition, pathWidth,0));
            }
            else
            {
                pathBuffer.AddRange(LevelGeneratorUtils.GetHorizontalZPath(startPosition, finishPosition, pathWidth,0));
            }

            //check all path positions for empty adjacent tiles and add a wall
            foreach (var pos in pathBuffer)
            {
                ClearPosition(pos);
                _tilemaps["Floors"].SetTile(pos, levelGeneratorSettings.connectionPathTile);
            }
            //check all path positions for empty adjacent tiles and add a wall
            foreach (var pos in pathBuffer)
            {
                var neighbors= GetNeighbors(pos);
                foreach (var emptyPos in neighbors)
                {
                    _tilemaps["Walls"].SetTile(emptyPos,levelGeneratorSettings.connectionWallTile);
                }
            }
            
        }
    }

    private Vector3Int GetInitialPathDirection(Vector3Int startPosition)
    {
        //determine if start is horizontal or vertical wall
        var neighbors = GetNeighbors(startPosition);
        if (neighbors.Count is 0 or > 1)
        {
            Debug.LogError("Path start position has an unexpected amount of empty neighbors");
            return Vector3Int.zero;
        }
        return startPosition - neighbors[0];
        
    }

    public void TestDrawPath()
    {
        DrawShortestPath(startTestPath,finishTestPath);
    }

    public void ClearPosition(Vector3Int position)
    {
        foreach (var tilemap in _tilemaps)
        {
            tilemap.Value.SetTile(position,null);
        }
    }
    private List<Vector3Int> DrawShortestPath(Vector3Int start, Vector3Int finish)
    {
        List<Vector3Int> path = TilemapUtils.Astar(start, finish,GetNeighbors);
        if (path == null)
        {
            Debug.LogWarning("No Path Found");
            return null;
        }
        foreach (var pos in path)
        {
            _tilemaps["Floors"].SetTile(pos,levelGeneratorSettings.connectionPathTile);
        }
        return path;
    }
    private List<Vector3Int> GetNeighbors(Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        if (!levelGeneratorSettings.levelSizeBounds.Contains(position)) return neighbors;
        for (int i = 0; i < 4; i++)
        {
            Vector3Int neighbor = position;
            switch (i)
            {
                case 0:
                    neighbor.x += 1;
                    break;
                case 1:
                    neighbor.x -= 1;
                    break;
                case 2:
                    neighbor.y += 1;
                    break;
                case 3:
                    neighbor.y -= 1;
                    break;
            }

            if (_tilemaps["Floors"].GetTile<TileBase>(neighbor) == null)
            {
                if(_tilemaps["Walls"].GetTile<TileBase>(neighbor) == null)
                {
                    neighbors.Add(neighbor);
                }
            }
        }
        return neighbors;
    }
}





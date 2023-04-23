using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


[CreateAssetMenu(menuName="LevelGenerator/BasicLinear")]
public class LinearLevelGenerator : LevelGenerator
{

    public BoundsInt levelSizeBounds = new BoundsInt(-1000,-1000,0,2000,2000,1);
    
    public int numberOfRoomsToSpawn = 5;
    public int distanceBetweenRooms = 5;
    
    public GameObject initialRoom;
    public List<GameObject> possibleRooms;
    
    public TileBase connectionPathTile;
    public TileBase connectionWallTile;
    public int connectionPathWidth = 1;
    
    private readonly Dictionary<string, Tilemap> _tilemaps = new Dictionary<string, Tilemap>();
    private GameObject _roomsBucket;
    private List<SpawnedRoom> _rooms = new List<SpawnedRoom>();
    
    public override void SpawnLevel()
    {
        Setup();
        SpawnRooms();
        SpawnPaths();
    }
    void Setup()
    {
        Tilemap[] tilemapsInScene = FindObjectsOfType<Tilemap>();
        _tilemaps.Clear();
        _rooms.Clear();
        foreach (Tilemap tilemap in tilemapsInScene)
        {
            _tilemaps.Add(tilemap.name,tilemap);
        }
        //Hide the connection tilemap, it's not meant to be visible
        _tilemaps["Connections"].gameObject.SetActive(false);
        
        //Create a bucket to put spawned rooms under
        _roomsBucket = new GameObject("=====ROOMS=====");
    }
    protected virtual void SpawnRooms()
    {
        Vector3Int currentPosition = Vector3Int.zero;
        var newRoom = LevelGeneratorUtils.SpawnRoom(initialRoom, currentPosition,_tilemaps,_roomsBucket);
        _rooms.Add(newRoom);
        
        for (int i = 1; i < numberOfRoomsToSpawn; ++i)
        {
            //get a random index
            int randomIndex = Random.Range(0, possibleRooms.Count);
            
            //get the bounds of the room to spawn
            Tilemap[] tilemapsInPrefab = possibleRooms[randomIndex].GetComponentsInChildren<Tilemap>();
            var bounds = TilemapUtils.GetBoundsOfAllTilemaps(tilemapsInPrefab);

            //move position
            currentPosition.y += distanceBetweenRooms + bounds.size.y / 2 + newRoom.bounds.size.y / 2;
            
            //spawn room
            newRoom= LevelGeneratorUtils.SpawnRoom(possibleRooms[randomIndex], currentPosition,_tilemaps,_roomsBucket);
            
            _rooms.Add(newRoom);
        }
    }
    protected virtual void SpawnPaths()
    {
        //add path connecting rooms in order spawned
        for(int i = 1; i < _rooms.Count; i++)
        {
            SpawnedRoom startRoom = _rooms[i-1];
            SpawnedRoom finishRoom = _rooms[i];
            
            //get additional starting positions to widen path
            int pathWidth = connectionPathWidth;
            
            var pathBuffer = GetPathConnectingRooms(startRoom, finishRoom, pathWidth);

            //check all path positions for empty adjacent tiles and add a wall
            foreach (var pos in pathBuffer)
            {
                ClearPosition(pos);
                _tilemaps["Floors"].SetTile(pos, connectionPathTile);
            }
            //check all path positions for empty adjacent tiles and add a wall
            foreach (var pos in pathBuffer)
            {
                var neighbors= GetNeighbors(pos);
                foreach (var emptyPos in neighbors)
                {
                    _tilemaps["Walls"].SetTile(emptyPos,connectionWallTile);
                }
            }
        }
    }

    private List<Vector3Int> GetPathConnectingRooms(SpawnedRoom startRoom, SpawnedRoom finishRoom, int pathWidth)
    {
        //get closest connection positions
        LevelGeneratorUtils.GetClosestConnectionPositions(startRoom, finishRoom,
            out var startPosition, out var finishPosition);

        //get path initial direction for start and finish
        Vector3Int startDirection = GetInitialPathDirection(startPosition);
        bool startHorizontal = Math.Abs(startDirection.x) != 0;

        Vector3Int finishDirection = GetInitialPathDirection(finishPosition);
        bool finishHorizontal = Math.Abs(finishDirection.x) != 0;

        //get path
        List<Vector3Int> pathBuffer = new List<Vector3Int>();
        if (startHorizontal && finishHorizontal)
        {
            int halfXDistance = (Math.Abs(startPosition.x - finishPosition.x) + 1) / 2;
            pathBuffer.AddRange(
                LevelGeneratorUtils.GetHorizontalZPath(startPosition, finishPosition, pathWidth, halfXDistance));
        }
        else if (!startHorizontal && !finishHorizontal)
        {
            int halfYDistance = (Math.Abs(startPosition.x - finishPosition.x) + 1) / 2;
            pathBuffer.AddRange(
                LevelGeneratorUtils.GetVerticalZPath(startPosition, finishPosition, pathWidth, halfYDistance));
        }
        else if (startHorizontal)
        {
            pathBuffer.AddRange(LevelGeneratorUtils.GetVerticalZPath(startPosition, finishPosition, pathWidth, 0));
        }
        else
        {
            pathBuffer.AddRange(LevelGeneratorUtils.GetHorizontalZPath(startPosition, finishPosition, pathWidth, 0));
        }

        return pathBuffer;
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

    public void ClearPosition(Vector3Int position)
    {
        foreach (var tilemap in _tilemaps)
        {
            tilemap.Value.SetTile(position,null);
        }
    }
    private List<Vector3Int> GetNeighbors(Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        if (!levelSizeBounds.Contains(position)) return neighbors;
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

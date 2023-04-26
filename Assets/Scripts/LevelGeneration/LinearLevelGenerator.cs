using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


[CreateAssetMenu(menuName="LevelGenerator/BasicLinear")]
public class LinearLevelGenerator : LevelGenerator
{

    public BoundsInt levelSizeBounds = new BoundsInt(-1000,-1000,0,2000,2000,1);
    
    public Vector3Int linearDirection = new Vector3Int(0,1,0);
    
    public int numberOfRoomsToSpawn = 5;
    public int distanceBetweenRooms = 5;
    
    public GameObject initialRoom;
    public List<GameObject> possibleRooms;

    public TileBase connectionPathTile;
    public TileBase connectionWallTile;
    public int connectionPathWidth = 1;

    public TileBase testTile;
    public Vector3Int testPosition;
    
    private readonly Dictionary<string, Tilemap> _tilemaps = new Dictionary<string, Tilemap>();
    private readonly List<Tilemap> _collidableTileMaps = new List<Tilemap>();
    private GameObject _roomsBucket;
    private readonly List<SpawnedRoom> _rooms = new List<SpawnedRoom>();
    private readonly List<Vector3Int> _pathBuffer = new List<Vector3Int>();
    
    public override void SpawnLevel()
    {
        Setup();
        SpawnRooms(linearDirection);
        SpawnPaths();
    }
    void Setup()
    {
        _tilemaps.Clear();
        _rooms.Clear();
        _collidableTileMaps.Clear();

        Tilemap[] tilemapsInScene = FindObjectsOfType<Tilemap>();
        foreach (Tilemap tilemap in tilemapsInScene)
        {
            _tilemaps.Add(tilemap.name,tilemap);
        }
        
        //Hide the connection tilemap, it's not meant to be visible
        _tilemaps["Connections"].gameObject.SetActive(false);
        
        //Create a bucket to put spawned rooms under
        _roomsBucket = new GameObject("=====ROOMS=====");
    }
    protected virtual void SpawnRooms(Vector3Int direction)
    {
        Vector3Int currentPosition = TilemapUtils.GetBoundsOfAllTilemaps(initialRoom.GetComponentsInChildren<Tilemap>()).size/-2;
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
            if (direction.x > 0)
            {
                currentPosition.x += newRoom.bounds.size.x;
            }else if (direction.x < 0)
            {
                currentPosition.x -= bounds.size.x;
            }

            if (direction.y > 0)
            {
                currentPosition.y += newRoom.bounds.size.y;
            }else if (direction.y < 0)
            {
                currentPosition.y -= bounds.size.y;
            }
            currentPosition += new Vector3Int(distanceBetweenRooms, distanceBetweenRooms, distanceBetweenRooms) * direction;
            
            //spawn room
            newRoom= LevelGeneratorUtils.SpawnRoom(possibleRooms[randomIndex], currentPosition,_tilemaps,_roomsBucket);
            
            _rooms.Add(newRoom);
            Debug.Log("Spawned New Room:");
            newRoom.LogInfo();
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
            
            var pathBuffer = LevelGeneratorUtils.GetPathConnectingRooms(startRoom, finishRoom, pathWidth,levelSizeBounds,_collidableTileMaps);

            //draw paths
            foreach (var pos in pathBuffer)
            {
                LevelGeneratorUtils.ClearPosition(pos,_tilemaps);
                _tilemaps["Floors"].SetTile(pos, connectionPathTile);
            }
            //check all path positions for empty adjacent tiles and add a wall
            foreach (var pos in pathBuffer)
            {
                List<Tilemap> collidableLayers = new List<Tilemap>();
                collidableLayers.Add(_tilemaps["Walls"]);
                collidableLayers.Add(_tilemaps["Floors"]);
                var neighbors = LevelGeneratorUtils.GetOpenSquares4D(pos,levelSizeBounds,collidableLayers);
                foreach (var emptyPos in neighbors)
                {
                    _tilemaps["Walls"].SetTile(emptyPos,connectionWallTile);
                }
            }
        }
    }

}

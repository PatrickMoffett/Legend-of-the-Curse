using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


[CreateAssetMenu(menuName="LevelGenerator/LinearLevelWithBranches")]
public class LinearLevelWithBranches : LevelGenerator
{
    public BoundsInt levelSizeBounds = new BoundsInt(-1000,-1000,0,2000,2000,1);
    public Vector3Int linearDirection = new Vector3Int(0,1,0);
    
    public int numberOfLinearRoomsToSpawn = 5;
    public int distanceBetweenRooms = 5;
    public int numberOfBranchesToSpawn = 2;
    public int numberOfRoomsPerBranch = 2;
    
    
    public GameObject initialRoom;
    public List<GameObject> possibleRooms;
    
    public TileBase connectionPathTile;
    public TileBase connectionWallTile;
    public int connectionPathWidth = 1;

    public bool useTestSeed = false;
    public int testSeed;
    
    private readonly Dictionary<string, Tilemap> _tilemaps = new Dictionary<string, Tilemap>();
    private readonly List<Tilemap> _collidableTileMaps = new List<Tilemap>();
    private GameObject _roomsBucket;
    private readonly List<SpawnedRoom> _rooms = new List<SpawnedRoom>();
    private readonly List<Vector3Int> _pathBuffer = new List<Vector3Int>();

    public override void SpawnLevel()
    {
        Setup();
        SpawnStartingRoom();
        SpawnRooms(_rooms[0],linearDirection,numberOfLinearRoomsToSpawn);
        AddLinearPathToBuffer();
        SpawnBranches();
        DrawPathBuffer();
        EmptyPathBuffer();
    }

    private void Setup()
    {
        //clear arrays
        _tilemaps.Clear();
        _rooms.Clear();
        _collidableTileMaps.Clear();
        EmptyPathBuffer();
        

        //get all tilemaps
        Tilemap[] tilemapsInScene = FindObjectsOfType<Tilemap>();
        
        foreach (Tilemap tilemap in tilemapsInScene)
        {
            _tilemaps.Add(tilemap.name,tilemap);
        }
        
        //Hide the connection tilemap, it's not meant to be visible
        _tilemaps["Connections"].gameObject.SetActive(false);
        
        //create collidable tilemaps the are checked for collision
        _collidableTileMaps.Add(_tilemaps["Walls"]);
        _collidableTileMaps.Add(_tilemaps["Floors"]);
        
        //Create a bucket to put spawned rooms under
        _roomsBucket = new GameObject("=====ROOMS=====");

        //create and set a seed for testing
        if (useTestSeed)
        {
            Random.InitState(testSeed);
        }
    }
    private void SpawnStartingRoom()
    {
        Vector3Int startPos = TilemapUtils.GetBoundsOfAllTilemaps(initialRoom.GetComponentsInChildren<Tilemap>()).size / -2;
        var newRoom = LevelGeneratorUtils.SpawnRoom(initialRoom, startPos, _tilemaps,_roomsBucket);
        _rooms.Add(newRoom);
        Debug.Log("Spawned New Room:");
        newRoom.LogInfo();
    }
    protected virtual void SpawnRooms(SpawnedRoom startingRoom,Vector3Int direction,int numberOfRoomsToSpawn)
    {
        
        Vector3Int currentPosition = startingRoom.bounds.position;
        SpawnedRoom newRoom = startingRoom;
        
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
    protected virtual void AddLinearPathToBuffer()
    {
        //add path connecting rooms in order spawned
        for(int i = 1; i < _rooms.Count; i++)
        {
            SpawnedRoom startRoom = _rooms[i-1];
            SpawnedRoom finishRoom = _rooms[i];

            _pathBuffer.AddRange(LevelGeneratorUtils.GetPathConnectingRooms(startRoom, finishRoom, connectionPathWidth, levelSizeBounds,_collidableTileMaps));
        }
    }
    private void SpawnBranches()
    {
        //Get indexes
        List<int> index = new List<int>();
        for (int i = 0; i < _rooms.Count; i++)
        {
            index.Add(i);
        }
        
        //chose unique random indexes
        List<int> locationsToBranch = new List<int>();
        for (int i = 0; i < numberOfBranchesToSpawn; ++i)
        {
            int random = Random.Range(0, index.Count);
            locationsToBranch.Add(index[random]);
            index.RemoveAt(random);
        }

        //spawn rooms
        foreach (var roomIndex in locationsToBranch)
        {
            SpawnedRoom roomToBranchFrom = _rooms[roomIndex];
            
            //Get branch Direction
            int randomDir =Random.Range(0, 2) * 2 - 1;  // == -1 or 1
            Vector3Int branchDirection;
            if (linearDirection.x != 0)
            {
                branchDirection = new Vector3Int(0, randomDir, 0);
            }
            else
            {
                branchDirection = new Vector3Int(randomDir, 0, 0);
            }

            //get index of next room to spawn
            int startingIndex = _rooms.Count;
            //spawn rooms
            SpawnRooms(roomToBranchFrom,branchDirection,numberOfRoomsPerBranch);
            //connect first room in branch
            _pathBuffer.AddRange(LevelGeneratorUtils.GetPathConnectingRooms(
                _rooms[roomIndex], 
                _rooms[startingIndex], 
                connectionPathWidth,
                levelSizeBounds,
                _collidableTileMaps));
            //connect remaining rooms
            for (int i = startingIndex+1; i < _rooms.Count; i++)
            {
                _pathBuffer.AddRange(LevelGeneratorUtils.GetPathConnectingRooms(
                    _rooms[i-1], 
                    _rooms[i], 
                    connectionPathWidth,
                    levelSizeBounds, 
                    _collidableTileMaps));
            }
        }
        
    }
    private void DrawPathBuffer()
    {
        //check all path positions for empty adjacent tiles and add a wall
        foreach (var pos in _pathBuffer)
        {
            LevelGeneratorUtils.ClearPosition(pos,_tilemaps);
            _tilemaps["Floors"].SetTile(pos, connectionPathTile);
        }
        //check all path positions for empty adjacent tiles and add a wall
        foreach (var pos in _pathBuffer)
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
    private void EmptyPathBuffer()
    {
        _pathBuffer.Clear();
    }
    
}
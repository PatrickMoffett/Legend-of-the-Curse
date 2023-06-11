using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

[Serializable]
public class SpawnedRoom
{
    public string name = String.Empty;
    public BoundsInt bounds = new BoundsInt();
    public List<Vector3Int> connectionPositions = new List<Vector3Int>();

    public void LogInfo()
    {
        string connections = "| Connection Positions:";
        for (int i = 0; i < connectionPositions.Count; i++)
        {
            connections += "\t |Pos " + i + ": "  + connectionPositions[i] + "|";
        }
        Debug.LogFormat("|RoomName: {0}| \t | Bounds: {1} |",name,bounds + "\n \t" + connections);
    }
}

public static class LevelGeneratorUtils
{
    public static SpawnedRoom SpawnRoom(GameObject roomToSpawn, Vector3Int positionToSpawn,
        Dictionary<string, Tilemap> tilemapLayers,GameObject roomBucket = null)
    {
        //COPY TILEMAPS
        Tilemap[] tilemapsInPrefab = roomToSpawn.GetComponentsInChildren<Tilemap>();
        var bounds = TilemapUtils.GetBoundsOfAllTilemaps(tilemapsInPrefab);

        GameObject roomObject = new GameObject(roomToSpawn.name)
        {
            transform =
            {
                position = positionToSpawn,
                parent = roomBucket.transform
            }
        };
        List<Vector3Int> connectionPositions = null;
        foreach (var tilemap in tilemapsInPrefab)
        {
            if (tilemap.name == "Objects")
            {
                CopyChildObjects(tilemap, bounds, positionToSpawn, roomObject);
                CopyTilemapAtPosition(tilemap, bounds, positionToSpawn, tilemapLayers[tilemap.name]);
            }
            else if (tilemap.name == "Connections")
            {
                connectionPositions = GetConnectionPositions(tilemap, bounds, positionToSpawn);
            }else if (tilemapLayers.ContainsKey(tilemap.name))
            {
                CopyTilemapAtPosition(tilemap, bounds, positionToSpawn, tilemapLayers[tilemap.name]);
            }
            else
            {
                Debug.LogWarning("Prefab contained unknown tilemap layer: " + tilemap.name);
            }
        }

        SpawnedRoom newRoom = new SpawnedRoom
        {
            name = roomToSpawn.name,
            bounds = new BoundsInt(positionToSpawn, bounds.size),
            connectionPositions = connectionPositions,
        };
        return newRoom;
    }
    private static void CopyChildObjects(Tilemap tilemap, BoundsInt bounds, Vector3Int position, GameObject roomObject)
    {
        for (int i = 0; i < tilemap.gameObject.transform.childCount; i++)
        {
            GameObject go = tilemap.gameObject.transform.GetChild(i).gameObject;
            go = Object.Instantiate(go, go.transform.position + position - bounds.position, go.transform.rotation);
            go.transform.parent = roomObject.transform;
        }
    }
    
    private static void CopyTilemapAtPosition(Tilemap tilemapToCopy, BoundsInt bounds, Vector3Int position, Tilemap tilemapToAddTo)
    {
        TileBase[] tiles = new TileBase[(bounds.size.x * bounds.size.y * bounds.size.z)];
        Vector3Int[] positions = new Vector3Int[(bounds.size.x * bounds.size.y * bounds.size.z)];
        
        int xMin = bounds.xMin;
        int yMin = bounds.yMin;
        int zMin = bounds.zMin;
        int index = 0;
        for (int x = 0; x < tilemapToCopy.cellBounds.xMax - xMin; x++)
        {
            for (int y = 0; y < tilemapToCopy.cellBounds.yMax - yMin; y++)
            {
                for (int z = 0; z < tilemapToCopy.cellBounds.zMax - zMin; z++)
                {
                    positions[index] = new Vector3Int(x + position.x,y + position.y, z + position.z);
                    TileBase tileBase =tilemapToCopy.GetTile<TileBase>(new Vector3Int(xMin + x, yMin + y, zMin + z));
                    tiles[index] = tileBase;
                    index++;
                }
            }
        }
        tilemapToAddTo.SetTiles(positions,tiles);
    }

    private static List<Vector3Int> GetConnectionPositions(Tilemap tilemap,BoundsInt bounds, Vector3Int position)
    {
        List<Vector3Int> connectionPositions = new List<Vector3Int>();
        int xMin = bounds.xMin;
        int yMin = bounds.yMin;
        int zMin = bounds.zMin;
        for (int x = 0; x < bounds.xMax - xMin; x++)
        {
            for (int y = 0; y < bounds.yMax - yMin; y++)
            {
                for (int z = 0; z < bounds.zMax - zMin; z++)
                {
                    var tile = tilemap.GetTile(new Vector3Int(xMin + x, yMin + y, zMin + z));
                    if (tile != null)
                    {
                        connectionPositions.Add(new Vector3Int(x + position.x,y + position.y, z + position.z));
                    }
                }
            }
        }
        return connectionPositions;
    }
    public  static void GetClosestConnectionPositions(SpawnedRoom startRoom, SpawnedRoom finishRoom, 
        out Vector3Int startRoomPosition, out Vector3Int finishRoomPosition)
    {
        startRoomPosition = default;
        finishRoomPosition = default;
        float minSqrMagnitude = float.PositiveInfinity;

        foreach (var startPos in startRoom.connectionPositions)
        {
            foreach (var finishPos in finishRoom.connectionPositions)
            {
                if ((finishPos - startPos).sqrMagnitude < minSqrMagnitude)
                {
                    startRoomPosition = startPos;
                    finishRoomPosition = finishPos;
                    minSqrMagnitude = (finishPos - startPos).sqrMagnitude;
                }
            }
        }
    }
    public static List<Vector3Int> GetHorizontalZPath(Vector3Int startPosition, Vector3Int finishPosition, int pathWidth, int turnDistance)
    {
        Debug.LogFormat("Spawning Horizontal Z Path: | StartPosition: {0} | FinishPosition: {1} | Path Width: {2} | TurnDistance: {3}", 
                                                        startPosition,  finishPosition,  pathWidth,  turnDistance);
        List<Vector3Int> pathBuffer = new List<Vector3Int>();
        //determine which position is the leftmost
        Vector3Int leftPosition = startPosition.x < finishPosition.x ? startPosition : finishPosition;
        Vector3Int rightPosition = startPosition.x < finishPosition.x ? finishPosition : startPosition;

        //adjust position for path Width
        leftPosition -= new Vector3Int(0, pathWidth / 2, 0);
        rightPosition -= new Vector3Int(0, pathWidth / 2, 0);

        //determine bottom and top positions (After width adjustment)
        Vector3Int bottomPosition = leftPosition.y < rightPosition.y ? leftPosition : rightPosition;
        Vector3Int topPosition = leftPosition.y < rightPosition.y ? rightPosition : leftPosition;

        //adjust top position for path width
        topPosition.y += pathWidth;

        //get path X Distance
        int pathXDistance = Math.Abs(rightPosition.x - leftPosition.x)+1;
        int pathYDistance = Math.Abs(bottomPosition.y - topPosition.y);

        //get distance for first section of x path
        int firstPathXDistance = turnDistance;
        //get distance and start for second section of x path
        int secondPathXDistance = pathXDistance - firstPathXDistance;
        int secondPathXStart = leftPosition.x + firstPathXDistance;
        int secondPathYStart = rightPosition.y;

        //get y path start
        Vector3Int yPathStart = new Vector3Int(secondPathXStart - (pathWidth / 2), bottomPosition.y, 0);

        //create 3 bounds representing the path
        BoundsInt firstXPathBounds = new BoundsInt(leftPosition.x, leftPosition.y, 0, firstPathXDistance, pathWidth, 1);
        BoundsInt secondXPathBounds = new BoundsInt(secondPathXStart, secondPathYStart, 0, secondPathXDistance, pathWidth, 1);
        BoundsInt yPathBounds = new BoundsInt(yPathStart.x, yPathStart.y, yPathStart.z, pathWidth, pathYDistance, 1);

        foreach (var pos in firstXPathBounds.allPositionsWithin)
        {
            pathBuffer.Add(pos);
        }

        foreach (var pos in secondXPathBounds.allPositionsWithin)
        {
            pathBuffer.Add(pos);
        }

        foreach (var pos in yPathBounds.allPositionsWithin)
        {
            pathBuffer.Add(pos);
        }

        string boundsDebug = "Path Bounds: ";
        boundsDebug += "| Left X Bound: " + firstXPathBounds + " | ";
        boundsDebug += "| Y Bound: " + yPathBounds + " | ";
        boundsDebug += "| Right X Bound: " + secondXPathBounds + " | ";
        Debug.Log(boundsDebug);
        
        return pathBuffer;
    }
    
    public static List<Vector3Int> GetVerticalZPath(Vector3Int startPosition, Vector3Int finishPosition, int pathWidth, int turnDistance)
    {
        Debug.LogFormat("Spawning Vertical Z Path: | StartPosition: {0} | FinishPosition: {1} | Path Width: {2} | TurnDistance: {3}", 
            startPosition,  finishPosition,  pathWidth,  turnDistance);
        
        List<Vector3Int> pathBuffer = new List<Vector3Int>();
        
        //determine bottom and top positions 
        Vector3Int bottomPosition = startPosition.y < finishPosition.y ? startPosition : finishPosition;
        Vector3Int topPosition = startPosition.y < finishPosition.y ? finishPosition : startPosition;

        //adjust position for path Width
        bottomPosition -= new Vector3Int(pathWidth / 2, 0, 0);
        topPosition -= new Vector3Int(pathWidth / 2, 0, 0);

        //determine which position is the leftmost (After width adjustment)
        Vector3Int leftPosition = bottomPosition.x < topPosition.x ? bottomPosition : topPosition;
        Vector3Int rightPosition = bottomPosition.x < topPosition.x ? topPosition : bottomPosition;
        
        //adjust right position for path width
        rightPosition.x += pathWidth;

        //get path X Distance
        int pathXDistance = Math.Abs(rightPosition.x - leftPosition.x);
        int pathYDistance = Math.Abs(bottomPosition.y - topPosition.y)+1;

        //get distance for first section of x path
        int firstPathYDistance = turnDistance;
        //get distance and start for second section of x path
        int secondPathYDistance = pathYDistance - firstPathYDistance;
        int secondPathYStart = bottomPosition.y + firstPathYDistance;
        int secondPathXStart = topPosition.x;

        //get y path start
        Vector3Int xPathStart = new Vector3Int(leftPosition.x,secondPathYStart - (pathWidth / 2), 0);

        //create 3 bounds representing the path
        BoundsInt firstYPathBounds = new BoundsInt(bottomPosition.x, bottomPosition.y, 0, pathWidth, firstPathYDistance, 1);
        BoundsInt secondYPathBounds = new BoundsInt(secondPathXStart, secondPathYStart, 0, pathWidth, secondPathYDistance, 1);
        BoundsInt xPathBounds = new BoundsInt(xPathStart.x, xPathStart.y, xPathStart.z, pathXDistance, pathWidth, 1);

        foreach (var pos in firstYPathBounds.allPositionsWithin)
        {
            pathBuffer.Add(pos);
        }

        foreach (var pos in secondYPathBounds.allPositionsWithin)
        {
            pathBuffer.Add(pos);
        }

        foreach (var pos in xPathBounds.allPositionsWithin)
        {
            pathBuffer.Add(pos);
        }

        string boundsDebug = "Path Bounds: ";
        boundsDebug += "| Lower Y Bound: " + firstYPathBounds + " | ";
        boundsDebug += "| X Bound: " + xPathBounds + " | ";
        boundsDebug += "| Upper Y Bound: " + secondYPathBounds + " | ";
        Debug.Log(boundsDebug);
        
        return pathBuffer;
    }
    public static void ClearPosition(Vector3Int position, Dictionary<string, Tilemap> tilemaps)
    {
        foreach (var tilemap in tilemaps)
        {
            tilemap.Value.SetTile(position,null);
        }
    }
    public static List<Vector3Int> GetOpenSquares4D(Vector3Int position, BoundsInt levelBounds, List<Tilemap> collidableTilemaps)
    {
        List<Vector3Int> openSquares = new List<Vector3Int>();
        //if position is outside level, it has no neighbors
            //TODO: What if it's one square outside the level?
        if (!levelBounds.Contains(position)) return openSquares;
        
        //for tiles up down left and right
        for (int i = 0; i < 4; i++)
        {
            //start at position
            Vector3Int neighbor = position;
            //move to adjacent square
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
            
            //check for collisions
            bool collisionFound = false;
            foreach(var tilemap in collidableTilemaps)
            {
                if (tilemap.GetTile<TileBase>(neighbor) != null)
                {
                    collisionFound = true;
                }
            }
            
            //if no collisions add square as open
            if (!collisionFound)
            {
                openSquares.Add(neighbor);
            }
        }
        return openSquares;
    }
    
    public static List<Vector3Int> GetPathConnectingRooms(SpawnedRoom startRoom, SpawnedRoom finishRoom, int pathWidth, BoundsInt levelSizeBounds,List<Tilemap> collidableTilemaps)
    {
        //get closest connection positions
        LevelGeneratorUtils.GetClosestConnectionPositions(startRoom, finishRoom,
            out var startPosition, out var finishPosition);

        //get path initial direction for start and finish
        Vector3Int startDirection = GetInitialPathDirection(startPosition,levelSizeBounds,collidableTilemaps);
        bool startHorizontal = Math.Abs(startDirection.x) != 0;

        Vector3Int finishDirection = GetInitialPathDirection(finishPosition,levelSizeBounds,collidableTilemaps);
        bool finishHorizontal = Math.Abs(finishDirection.x) != 0;

        //get path
        List<Vector3Int> pathBuffer = new List<Vector3Int>();
        if (startHorizontal && finishHorizontal)
        {
            int halfXDistance = (Math.Abs(startPosition.x - finishPosition.x) + 1) / 2;
            pathBuffer.AddRange(LevelGeneratorUtils.GetHorizontalZPath(startPosition, finishPosition, pathWidth, halfXDistance));
        }
        else if (!startHorizontal && !finishHorizontal)
        {
            int halfYDistance = (Math.Abs(startPosition.y - finishPosition.y) + 1) / 2;
            pathBuffer.AddRange(LevelGeneratorUtils.GetVerticalZPath(startPosition, finishPosition, pathWidth, halfYDistance));
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
    public static Vector3Int GetInitialPathDirection(Vector3Int startPosition, BoundsInt levelSizeBounds,List<Tilemap> collidableTilemaps)
    {
        //determine if start is horizontal or vertical wall
        var neighbors = LevelGeneratorUtils.GetOpenSquares4D(startPosition,levelSizeBounds,collidableTilemaps);
        if (neighbors.Count is 0 or > 1)
        {
            Debug.LogWarning("Path start position has an unexpected amount of empty neighbors");
            return Vector3Int.zero;
        }
        return neighbors[0]-startPosition;
    }

}

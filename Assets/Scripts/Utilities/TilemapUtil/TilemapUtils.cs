
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using Object = UnityEngine.Object;

public static class TilemapUtils
{
    
    // A* finds a path from start to goal.
    // h is the heuristic function. h(n) estimates the cost to reach goal from node n.
    public static List<Vector3Int> Astar(Vector3Int start, Vector3Int finish, List<Tilemap> collidableTilemaps)
    {
        if (start == finish) return null;
        
        //TODO: MAKE THIS A PARAMETER INSTEAD OF HARDCODED? or maybe some global setting?
        BoundsInt levelBounds = new BoundsInt(new Vector3Int(-1000, -1000,0), new Vector3Int(2000, 2000,1));

        // For node n, cameFrom[n] is the node immediately preceding it on the cheapest path from the start
        // to n currently known.
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        

        // For node n, gScore[n] is the cost of the cheapest path from start to n currently known.
        //default to infinity
        Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>();
        gScore.Add(start,0f);
        
        // For node n, fScore[n] := gScore[n] + h(n). fScore[n] represents our current best guess as to
        // how cheap a path could be from start to finish if it goes through n.
        //default to infinity
        Dictionary<Vector3Int, float> fScore = new Dictionary<Vector3Int, float>();
        fScore.Add(start,CalculateHValue(start,finish));
        
        // The set of discovered nodes that may need to be (re-)expanded.
        // Initially, only the start node is known.
        // This is usually implemented as a min-heap or priority queue rather than a hash-set.
        PriorityQueue<Vector3Int,float> openSet = new PriorityQueue<Vector3Int, float>();
        HashSet<Vector3Int> openHashSet = new HashSet<Vector3Int>();
        openHashSet.Add(start);
        openSet.Enqueue(start, fScore[start]);

        
        while (openSet.Count is > 0 and < 2000)
        {
            Vector3Int current = openSet.Peek();
            if (current == finish)
            {
                return ReconstructPath(current, cameFrom);
            }

            openSet.Dequeue();
            openHashSet.Remove(current);
            List<Vector3Int> neighbors= LevelGeneratorUtils.GetOpenSquares4D(current,levelBounds,collidableTilemaps);

            foreach (var neighbor in neighbors)
            {
                // d(current,neighbor) is the weight of the edge from current to neighbor
                // tentative_gScore is the distance from start to the neighbor through current

                float tenativeGscore = gScore[current] + 1; //no diagonals so only +1 (diagonal would be sqrt(2))
                float currentGscore = gScore.ContainsKey(neighbor) ? gScore[neighbor] : float.PositiveInfinity;
                if (tenativeGscore < currentGscore)
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tenativeGscore;
                    fScore[neighbor] = tenativeGscore + CalculateHValue(neighbor, finish);
                    if (!openHashSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor,fScore[neighbor]);
                        openHashSet.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }
    // A Utility Function to calculate the 'h' heuristics.
    private static float  CalculateHValue(Vector3Int pos, Vector3Int dest)
    {
        // Return using the distance formula
        return (Mathf.Sqrt(
            (pos.x - dest.x) * (pos.x - dest.x)
            + (pos.y - dest.y) * (pos.y - dest.y)));
    }
    private static List<Vector3Int> ReconstructPath(Vector3Int finish, Dictionary<Vector3Int, Vector3Int> cameFrom)
    {
        Vector3Int current = finish;
        List<Vector3Int> path = new List<Vector3Int>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Add(current);
        path.Reverse();
        return path;
    }
    public static BoundsInt GetBoundsOfAllTilemaps(Tilemap[] tilemapsInPrefab)
    {
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
        return bounds;
    }
}

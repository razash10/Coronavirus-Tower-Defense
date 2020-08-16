using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Waypoint startWaypoint = null, endWaypoint = null;
    
    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
    Queue<Waypoint> queue = new Queue<Waypoint>();
    bool isRunning = true;
    Waypoint searchCenter;
    public Stack<Waypoint> path = new Stack<Waypoint>();

    Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    public Stack<Waypoint> GetPath()
    {
        LoadBlocks();
        BFS();
        ColorStartAndEnd();
        CreatePath();
        return path;
    }

    private void CreatePath()
    {
        path.Push(endWaypoint);
        Waypoint prev = endWaypoint.exploredFrom;
        while(prev != startWaypoint)
        {
            prev.SetTopColor(Color.yellow);
            path.Push(prev);
            prev = prev.exploredFrom;
        }
        path.Push(startWaypoint);
    }

    private void BFS()
    {
        queue.Enqueue(startWaypoint);
        while(queue.Count > 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            HaltIfEndFound();
            ExploreNeighbours();
            searchCenter.isExplored = true;
        }
    }

    private void HaltIfEndFound()
    {
        if(searchCenter == endWaypoint)
        {
            isRunning = false;
        }
    }

    private void ExploreNeighbours()
    {
        if(!isRunning) { return; }

        foreach(Vector2Int direction in directions)
        {
            Vector2Int neighbourCoordinates = searchCenter.GetGridPos() + direction;
            try
            {
                Waypoint neighbour = grid[neighbourCoordinates];
                if (!neighbour.isExplored && !queue.Contains(neighbour))
                {
                    queue.Enqueue(neighbour);
                    neighbour.exploredFrom = searchCenter;
                }
            }
            catch
            {
                // do nothing
            }
        }
    }

    private void ColorStartAndEnd()
    {
        startWaypoint.SetTopColor(Color.green);
        endWaypoint.SetTopColor(Color.red);
    }

    private void LoadBlocks()
    {
        var waypoints = FindObjectsOfType<Waypoint>();
        foreach(Waypoint waypoint in waypoints)
        {
            bool isOverLapping = grid.ContainsKey(waypoint.GetGridPos());
            if (isOverLapping)
            {
                Debug.LogWarning("Skipping overlapping block " + waypoint);
            }
            else
            {
                grid.Add(waypoint.GetGridPos(), waypoint);
            }
                
        }
    }
}


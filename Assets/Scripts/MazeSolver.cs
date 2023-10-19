using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeSolver : MonoBehaviour
{
    public class MazeSolvePoint
    {
        public int x, y, data;
        public int type;
        public bool visited;

        public MazeSolvePoint(int x, int y, int type)
        {
            this.x = x;
            this.y = y;
            data = 0;
            this.type = type;
            visited = false;
        }

        public bool isWalkable()
        {
            return type != -1;
        }
    }

    [Header("UI")]
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile pathTile, startTile, endTile;
    
    // Maze variables
    MazeSolvePoint[,] maze;
    MazeSolvePoint start;
    MazeSolvePoint end;
    int sizeX, sizeY;

    public void SolveMaze(int[,] mazeToSolve)
    {
        sizeX = mazeToSolve.GetLength(0);
        sizeY = mazeToSolve.GetLength(1);

        maze = new MazeSolvePoint[sizeX, sizeY];

        // Generate maze solve point array
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                maze[x, y] = new MazeSolvePoint(x, y, mazeToSolve[x, y]);
            }
        }

        // Pick start and end points
        start = GetRandomPoint();
        do
        {
            end = GetRandomPoint();
        }
        while (end == start);

        DFS();
        FindPath();
        DisplayMazePath();
    }

    MazeSolvePoint GetRandomPoint()
    {
        MazeSolvePoint point;
        do
        {
            point = maze[Random.Range(0, sizeX), Random.Range(0, sizeY)];
        }
        while (!point.isWalkable());

        return point;
    }

    public void DFS()
    {
        Stack<MazeSolvePoint> s = new Stack<MazeSolvePoint>(sizeX * sizeY);
        s.Push(start);
        int distance = 0;

        while (s.Count > 0)
        {
            MazeSolvePoint curr = s.Pop();

            if (curr.visited)
                continue;

            curr.visited = true;
            curr.data = distance++;

            if (curr == end)
                return;

            if (curr.x > 1) // Up
            {
                MazeSolvePoint n = maze[curr.x - 1, curr.y];
                if (n.isWalkable() && !n.visited)
                        s.Push(n);
            }
            if (curr.y < sizeY) // Right
            {
                MazeSolvePoint n = maze[curr.x, curr.y + 1];
                if (n.isWalkable() && !n.visited)
                    s.Push(n);
            }
            if (curr.x < sizeX) // Down
            {
                MazeSolvePoint n = maze[curr.x + 1, curr.y];
                if (n.isWalkable() && !n.visited)
                        s.Push(n);
            }
            if (curr.y > 1) // Left
            {
                MazeSolvePoint n = maze[curr.x, curr.y - 1];
                if (n.isWalkable() && !n.visited)
                    s.Push(n);
            }
        }
    }

    public void FindPath()
    {
        MazeSolvePoint currentPoint = end;

        while (currentPoint != start)
        {
            MazeSolvePoint[] nearPoints = new MazeSolvePoint[4];
            nearPoints[0] = maze[currentPoint.x, currentPoint.y + 1];
            nearPoints[1] = maze[currentPoint.x + 1, currentPoint.y];
            nearPoints[2] = maze[currentPoint.x, currentPoint.y - 1];
            nearPoints[3] = maze[currentPoint.x - 1, currentPoint.y];

            int minData = int.MaxValue;
            MazeSolvePoint nextPoint = null;
            for (int i = 0; i < nearPoints.Length; i++)
            {
                if (nearPoints[i] == start)
                    return;

                if (nearPoints[i].data < minData && nearPoints[i].data > 0)
                {
                    minData = nearPoints[i].data;
                    nextPoint = nearPoints[i];
                }
            }

            if (nextPoint == null)
                return;

            nextPoint.type = 1;
            currentPoint = nextPoint;
        }
    }

    void BFS()
    {
        Queue<MazeSolvePoint> s = new Queue<MazeSolvePoint>(sizeX * sizeY);
        s.Enqueue(start);
        int distance = 0;

        while (s.Count > 0)
        {
            MazeSolvePoint curr = s.Dequeue();

            if (curr.visited)
                continue;

            curr.visited = true;
            curr.data = distance++;

            if (curr == end)
                return;

            if (curr.x > 1) // Up
            {
                MazeSolvePoint n = maze[curr.x - 1, curr.y];
                if (n.isWalkable() && !n.visited)
                    s.Enqueue(n);
            }
            if (curr.y < sizeY) // Right
            {
                MazeSolvePoint n = maze[curr.x, curr.y + 1];
                if (n.isWalkable() && !n.visited)
                    s.Enqueue(n);
            }
            if (curr.x < sizeX) // Down
            {
                MazeSolvePoint n = maze[curr.x + 1, curr.y];
                if (n.isWalkable() && !n.visited)
                    s.Enqueue(n);
            }
            if (curr.y > 1) // Left
            {
                MazeSolvePoint n = maze[curr.x, curr.y - 1];
                if (n.isWalkable() && !n.visited)
                    s.Enqueue(n);
            }
        }
    }

    void DisplayMazePath()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (start == maze[x, y])
                    tilemap.SetTile(new Vector3Int(x, y, 0), startTile);
                else if (end == maze[x, y])
                    tilemap.SetTile(new Vector3Int(x, y, 0), endTile);
                else if (maze[x, y].type == 1)
                    tilemap.SetTile(new Vector3Int(x, y, 0), pathTile);
            }
        }
    }
}

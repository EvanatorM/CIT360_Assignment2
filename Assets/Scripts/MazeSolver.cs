using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        public void ResetPoint()
        {
            data = 0;
            if (type == 1)
                type = 0;
            visited = false;
        }
    }

    [Header("UI")]
    [SerializeField] Tilemap pathTilemap;
    [SerializeField] Tilemap startEndTilemap;
    [SerializeField] Tile pathTile, startTile, endTile;
    [SerializeField] TMP_Text toggleSolveTypeText;
    [SerializeField] TMP_Text dataNumberPrefab;
    [SerializeField] Transform dataNumberParent;
    [SerializeField] Gradient dataNumberGradient;
    
    // Maze variables
    MazeSolvePoint[,] maze;
    MazeSolvePoint start;
    MazeSolvePoint end;
    int sizeX, sizeY;
    bool dfsShowing = true;
    List<TMP_Text> dataNumbersLoaded = new List<TMP_Text>();

    public void SolveMaze(int[,] mazeToSolve)
    {
        // Reset tilemaps
        startEndTilemap.ClearAllTiles();
        pathTilemap.ClearAllTiles();

        // Set variables
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

        // Solve and display maze
        if (dfsShowing)
            DFS();
        else
            BFS();
        FindPath();
        DisplayMazePath();

        // Update data view
        if (dataNumbersLoaded.Count > 0)
        {
            ToggleDataView();
            ToggleDataView();
        }
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

    void BFS()
    {
        Queue<MazeSolvePoint> q = new Queue<MazeSolvePoint>(sizeX * sizeY);
        q.Enqueue(start);
        int distance = 0;

        while (q.Count > 0)
        {
            MazeSolvePoint curr = q.Dequeue();

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
                    q.Enqueue(n);
            }
            if (curr.y < sizeY) // Right
            {
                MazeSolvePoint n = maze[curr.x, curr.y + 1];
                if (n.isWalkable() && !n.visited)
                    q.Enqueue(n);
            }
            if (curr.x < sizeX) // Down
            {
                MazeSolvePoint n = maze[curr.x + 1, curr.y];
                if (n.isWalkable() && !n.visited)
                    q.Enqueue(n);
            }
            if (curr.y > 1) // Left
            {
                MazeSolvePoint n = maze[curr.x, curr.y - 1];
                if (n.isWalkable() && !n.visited)
                    q.Enqueue(n);
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

    void DisplayMazePath()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (start == maze[x, y])
                    startEndTilemap.SetTile(new Vector3Int(x, y, 0), startTile);
                else if (end == maze[x, y])
                    startEndTilemap.SetTile(new Vector3Int(x, y, 0), endTile);
                else if (maze[x, y].type == 1)
                    pathTilemap.SetTile(new Vector3Int(x, y, 0), pathTile);
            }
        }
    }

    public void TogglePath()
    {
        pathTilemap.gameObject.SetActive(!pathTilemap.gameObject.activeSelf);
    }

    public void SwitchSolveType()
    {
        // Toggle solve type
        dfsShowing = !dfsShowing;

        // Reset path
        pathTilemap.ClearAllTiles();

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                maze[x, y].ResetPoint();
            }
        }

        // Solve
        if (dfsShowing)
            DFS();
        else
            BFS();

        FindPath();
        DisplayMazePath();

        // Update button
        toggleSolveTypeText.text = "Switch to " + (dfsShowing ? "BFS" : "DFS");

        // Update data view
        if (dataNumbersLoaded.Count > 0)
        {
            ToggleDataView();
            ToggleDataView();
        }
    }

    public void ToggleDataView()
    {
        // Check if data is visible
        if (dataNumbersLoaded.Count > 0)
        {
            // Unload data numbers
            foreach (TMP_Text t in dataNumbersLoaded)
                Destroy(t.gameObject);
            dataNumbersLoaded.Clear();
        }
        else
        {
            // Create data numbers
            int highestData = GetHighestData();

            for (int x = 0; x < maze.GetLength(0); x++)
            {
                for (int y = 0; y < maze.GetLength(1); y++)
                {
                    TMP_Text newDataNum = Instantiate(dataNumberPrefab, new Vector3(maze[x, y].x + .5f, maze[x, y].y + .5f), Quaternion.identity, dataNumberParent);
                    newDataNum.text = maze[x, y].data.ToString();
                    //newDataNum.color = Color.Lerp(Color.green, Color.red, (float)maze[x, y].data / highestData);
                    if (maze[x, y].data == 0)
                        newDataNum.color = Color.black;
                    else
                        newDataNum.color = dataNumberGradient.Evaluate((float)maze[x, y].data / highestData);

                    dataNumbersLoaded.Add(newDataNum);
                }
            }
        }
    }

    public int GetHighestData()
    {
        MazeSolvePoint highestPoint = maze[0, 0];
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                if (maze[x, y].data > highestPoint.data)
                    highestPoint = maze[x, y];
            }
        }

        return highestPoint.data;
    }
}

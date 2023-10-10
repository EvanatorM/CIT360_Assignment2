using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSolver : MonoBehaviour
{
    MazeSolvePoint[,] maze = new MazeSolvePoint[10, 10];

    public void SolveMaze(Maze mazeToSolve)
    {
        for (int y = 0; y < mazeToSolve.mazeSizeY; y++)
        {
            for (int x = 0; x < mazeToSolve.mazeSizeX; x++)
            {
                if (wall)
                    maze[x, y] = new MazeSolvePoint();
                else
                    maze[x, y] = new MazeSolvePoint();

            }
        }

        // Pick start and end points
        MazeSolvePoint start = maze[rnd1][rnd2];
        MazeSolvePoint end = maze[rnd3][rnd4];
    }

    public void dfs()
    {
        Stack<MazeSolvePoint> s = new Stack<MazeSolvePoint>(row * col);
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

            if (curr.row > 1) // Up
            {
                MazeSolvePoint n = maze[curr.row - 1, curr.col];
                if (n.isWalkable() && !n.visited)
                        s.Push(n);
            }
            if (curr.col < col) // Right
            {
                MazeSolvePoint n = maze[curr.row, curr.col + 1];
                if (n.isWalkable() && !n.visited)
                    s.Push(n);
            }
            if (curr.row < row) // Down
            {
                MazeSolvePoint n = maze[curr.row + 1, curr.col];
                if (n.isWalkable() && !n.visited)
                        s.Push(n);
            }
            if (curr.col > 1) // Left
            {
                MazeSolvePoint n = maze[curr.row, curr.col - 1];
                if (n.isWalkable() && !n.visited)
                    s.Push(n);
            }
        }
    }

    void bfs()
    {

    }
}

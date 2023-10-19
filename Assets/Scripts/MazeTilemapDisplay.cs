using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeTilemapDisplay : MonoBehaviour
{
    [Header("Maze Generation")]
    [SerializeField] int mazeSizeX, mazeSizeY;

    [Header("Maze Display")]
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tile blankTile, wallTile;
    [SerializeField] int mazeNodeSize;

    Maze maze;
    MazeSolver ms;

    void Start()
    {
        maze = new Maze(mazeSizeX, mazeSizeY);
        ms = FindObjectOfType<MazeSolver>();

        DisplayMazeTilemaps();
    }

    void DisplayMazeTilemaps()
    {
        int[,] intArray = ConvertNodesToInt(mazeSizeX, mazeSizeY, mazeNodeSize, maze.nodes);
        for (int x = 0; x < intArray.GetLength(0); x++)
        {
            for (int y = 0; y < intArray.GetLength(1); y++)
            {
                if (intArray[x, y] == -1)
                    tilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                else
                    tilemap.SetTile(new Vector3Int(x, y, 0), blankTile);
            }
        }

        float mazeWorldSizeX = mazeSizeX * (mazeNodeSize + 1) + 1;
        float mazeWorldSizeY = mazeSizeY * (mazeNodeSize + 1) + 1;
        Camera.main.transform.position = new Vector3(mazeWorldSizeX / 2.0f, mazeWorldSizeY / 2.0f, -10);
        Camera.main.orthographicSize = mazeWorldSizeY * .5f + 1;

        // Solve maze
        ms.SolveMaze(intArray);
    }

    public int[,] ConvertNodesToInt(int nodeX, int nodeY, int nodeSize, Maze.MazeNode[,] nodes)
    {
        int sizeX = nodeX * (nodeSize + 1) + 1;
        int sizeY = nodeY * (nodeSize + 1) + 1;

        int[,] maze = new int[sizeX, sizeY];

        for (int ny = 0; ny < nodeY; ny++)
        {
            for (int nx = 0; nx < nodeX; nx++)
            {
                int ax = nx * (nodeSize + 1);
                int ay = ny * (nodeSize + 1);

                for (int dy = ay; dy < ay + nodeSize + 2; dy++)
                {
                    for (int dx = ax; dx < ax + nodeSize + 2; dx++)
                    {
                        if (dx == ax) // Left edge
                        {
                            // Check for corners
                            if (dy == ay || dy == ay + nodeSize + 1)
                                maze[dx, dy] = -1;

                            if (nodes[nx, ny].westWall)
                                maze[dx, dy] = -1;
                        }
                        else if (dx == ax + nodeSize + 1) // Right edge
                        {
                            // Check for corners
                            if (dy == ay || dy == ay + nodeSize + 1)
                                maze[dx, dy] = -1;

                            if (nodes[nx, ny].eastWall)
                                maze[dx, dy] = -1;
                        }
                        else if (dy == ay) // Bottom edge
                        {
                            if (nodes[nx, ny].southWall)
                                maze[dx, dy] = -1;
                        }
                        else if (dy == ay + nodeSize + 1) // Top edge
                        {
                            if (nodes[nx, ny].northWall)
                                maze[dx, dy] = -1;
                        }
                    }
                }
            }
        }

        return maze;
    }
}

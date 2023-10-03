using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDisplay : MonoBehaviour
{
    [Header("Maze Generation")]
    [SerializeField] int mazeSizeX, mazeSizeY;

    [Header("Maze Display")]
    [SerializeField] PhysicalMazeNode physicalMazeNodePrefab;
    [SerializeField] float mazeNodeSize;

    Maze maze;

    void Start()
    {
        maze = new Maze(mazeSizeX, mazeSizeY);

        DisplayMazePrefabs();
    }

    void DisplayMazePrefabs()
    {
        for (int x = 0; x < mazeSizeX; x++)
        {
            for (int y = 0; y < mazeSizeY; y++)
            {
                PhysicalMazeNode mazeNode = Instantiate(physicalMazeNodePrefab, new Vector3(x * mazeNodeSize, y * mazeNodeSize, 0), Quaternion.identity);
                mazeNode.InitPhysicalMadeNode(maze.nodes[x, y], mazeNodeSize);
            }
        }

        Camera.main.transform.position = new Vector3((mazeSizeX / 2.0f - 0.5f) * mazeNodeSize, (mazeSizeY / 2.0f - 0.5f) * mazeNodeSize, -10);
        Camera.main.orthographicSize = mazeSizeY * mazeNodeSize / 2f + 1;
    }
}

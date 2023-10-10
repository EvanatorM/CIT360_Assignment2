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
    [SerializeField] float mazeNodeSize;

    Maze maze;

    void Start()
    {
        maze = new Maze(mazeSizeX, mazeSizeY);

        DisplayMazeTilemaps();
    }

    void DisplayMazeTilemaps()
    {


        Camera.main.transform.position = new Vector3((mazeSizeX / 2.0f - 0.5f) * mazeNodeSize, (mazeSizeY / 2.0f - 0.5f) * mazeNodeSize, -10);
        Camera.main.orthographicSize = mazeSizeY * mazeNodeSize / 2f + 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalMazeNode : MonoBehaviour
{
    [SerializeField] GameObject northWall, eastWall, southWall, westWall;

    public void InitPhysicalMadeNode(Maze.MazeNode node, float scale)
    {
        transform.localScale = Vector3.one * scale;
        /*
        // Horizontal Walls
        northWall.transform.localScale = new Vector3(1.1f, 0.1f / scale);
        southWall.transform.localScale = new Vector3(1.1f, 0.1f / scale);
        // Vertical Walls
        eastWall.transform.localScale = new Vector3(0.1f / scale, 1.1f);
        westWall.transform.localScale = new Vector3(0.1f / scale, 1.1f);*/

        // Set walls
        northWall.SetActive(node.northWall);
        eastWall.SetActive(node.eastWall);
        southWall.SetActive(node.southWall);
        westWall.SetActive(node.westWall);
    }
}

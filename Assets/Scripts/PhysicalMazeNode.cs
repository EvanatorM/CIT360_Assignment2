using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalMazeNode : MonoBehaviour
{
    [SerializeField] GameObject northWall, eastWall, southWall, westWall;

    public void InitPhysicalMadeNode(Maze.MazeNode node, float scale)
    {
        transform.localScale = Vector3.one * scale;

        // Set walls
        northWall.SetActive(node.northWall);
        eastWall.SetActive(node.eastWall);
        southWall.SetActive(node.southWall);
        westWall.SetActive(node.westWall);
    }
}

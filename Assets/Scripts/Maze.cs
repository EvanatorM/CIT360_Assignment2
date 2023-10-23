using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    public class MazeNode
    {
        public bool northWall = true, 
            eastWall = true,
            southWall = true, 
            westWall = true;

        public bool available = true;
        public int posX, posY;

        public MazeNode(int posX, int posY)
        {
            this.posX = posX;
            this.posY = posY;
        }
    }

    public enum Direction
    {
        North,
        East,
        West,
        South
    }

    public MazeNode[,] nodes;
    public int mazeSizeX, mazeSizeY;
    public float density;

    // Generation
    List<MazeNode> nodeStack = new List<MazeNode>();

    public Maze(int mazeSizeX, int mazeSizeY, float density)
    {
        nodes = new MazeNode[mazeSizeX, mazeSizeY];
        for (int x = 0; x < mazeSizeX; x++)
        {
            for (int y = 0; y < mazeSizeY; y++)
            {
                nodes[x, y] = new MazeNode(x, y);
            }
        }

        this.mazeSizeX = mazeSizeX;
        this.mazeSizeY = mazeSizeY;
        this.density = density;

        GenerateMaze();
    }

    public void GenerateMaze()
    {
        // Choose first node
        nodeStack.Add(nodes[Random.Range(0, mazeSizeX), Random.Range(0, mazeSizeY)]);
        nodeStack[nodeStack.Count - 1].available = false;

        // Process node
        ProcessNextNode();
    }

    private void ProcessNextNode()
    {
        // Get top node in stack
        MazeNode topNode = nodeStack[nodeStack.Count - 1];

        // Get available nearby nodes
        List<Direction> availableDirections = GetAvailableDirections(topNode.posX, topNode.posY);

        // If there are available nearby nodes, add random available node to stack
        if (availableDirections.Count > 0)
        {
            Direction chosenDirection = availableDirections[Random.Range(0, availableDirections.Count)];
            switch (chosenDirection)
            {
                case Direction.North:
                    // Add to stack
                    nodeStack.Add(nodes[topNode.posX, topNode.posY + 1]);

                    // Turn off walls
                    nodeStack[nodeStack.Count - 2].northWall = false;
                    nodeStack[nodeStack.Count - 1].southWall = false;
                    break;
                case Direction.East:
                    // Add to stack
                    nodeStack.Add(nodes[topNode.posX + 1, topNode.posY]);

                    // Turn off walls
                    nodeStack[nodeStack.Count - 2].eastWall = false;
                    nodeStack[nodeStack.Count - 1].westWall = false;
                    break;
                case Direction.South:
                    // Add to stack
                    nodeStack.Add(nodes[topNode.posX, topNode.posY - 1]);

                    // Turn off walls
                    nodeStack[nodeStack.Count - 2].southWall = false;
                    nodeStack[nodeStack.Count - 1].northWall = false;
                    break;
                case Direction.West:
                    // Add to stack
                    nodeStack.Add(nodes[topNode.posX - 1, topNode.posY]);

                    // Turn off walls
                    nodeStack[nodeStack.Count - 2].westWall = false;
                    nodeStack[nodeStack.Count - 1].eastWall = false;
                    break;
            }

            // Remove walls based on density
            if (nodeStack[nodeStack.Count - 1].posY < mazeSizeY - 1 && Random.value < (1f - density))
                nodeStack[nodeStack.Count - 1].northWall = false;
            if (nodeStack[nodeStack.Count - 1].posX < mazeSizeX - 1 && Random.value < (1f - density))
                nodeStack[nodeStack.Count - 1].eastWall = false;
            if (nodeStack[nodeStack.Count - 1].posY > 0 && Random.value < (1f - density))
                nodeStack[nodeStack.Count - 1].southWall = false;
            if (nodeStack[nodeStack.Count - 1].posX > 0 && Random.value < (1f - density))
                nodeStack[nodeStack.Count - 1].westWall = false;

            // Set next node to unavailable
            nodeStack[nodeStack.Count - 1].available = false;
        }
        else
        {
            // If not, remove from stack
            nodeStack.RemoveAt(nodeStack.Count - 1);
        }

        // Process next node if there is one
        if (nodeStack.Count > 0)
            ProcessNextNode();
    }

    private List<Direction> GetAvailableDirections(int x, int y)
    {
        List<Direction> availableDirections = new List<Direction>();
        
        // -- North --
        // Check if border
        if (y < mazeSizeY - 1)
        {
            // Check if north node is available
            if (nodes[x, y + 1].available)
                availableDirections.Add(Direction.North);
        }
        // -- East --
        // Check if border
        if (x < mazeSizeX - 1)
        {
            // Check if north node is available
            if (nodes[x + 1, y].available)
                availableDirections.Add(Direction.East);
        }
        // -- South --
        // Check if border
        if (y > 0)
        {
            // Check if north node is available
            if (nodes[x, y - 1].available)
                availableDirections.Add(Direction.South);
        }
        // -- West --
        // Check if border
        if (x > 0)
        {
            // Check if north node is available
            if (nodes[x - 1, y].available)
                availableDirections.Add(Direction.West);
        }

        return availableDirections;
    }
}

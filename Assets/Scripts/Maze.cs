using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public static Maze Instance { private set; get; }

    private List<Node> nodes;

    private void Awake()
    {
        Instance = this;
        nodes = new List<Node>();
    }

    public void RegisterNode(Node node)
    {
        nodes.Add(node);
    }

    public Node GetNodeFromPosition(int x, int z)
    {
        foreach (Node node in nodes)
        {
            if(node.Position.x == x && node.Position.y == z)
            {
                return node;
            }
        }
        return null;
    }
}

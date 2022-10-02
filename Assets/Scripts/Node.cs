using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2 Position { private set; get; }
    public bool[] HasWall { private set; get; } // index 0 Right Wall, 1 down, 2 left, 3 up

    private void Start()
    {
        HasWall = new bool[4];
        
        /*Sets the wall status if the GameObject has the child walls */
        HasWall[0] = (transform.Find("East") != null);
        HasWall[1] = (transform.Find("South") != null);
        HasWall[2] = (transform.Find("West") != null);
        HasWall[3] = (transform.Find("North") != null);

        Position = new Vector2(transform.position.x, -transform.position.z);

        Maze.Instance.RegisterNode(this);
    }
}

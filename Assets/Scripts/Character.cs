using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    // Converts the world space of the character to a grid position
    public Vector2Int Position
    {
        get
        {
            return new Vector2Int((int)transform.position.x, -(int)transform.position.z);
        }
    }

    // List of positions the character has been in so we can undo their movements
    public List<Vector2Int> Plays { protected set; get; }

    protected virtual void Awake()
    {
        Plays = new List<Vector2Int>();
        Plays.Add(new Vector2Int(Position.x, Position.y));
    }

    protected virtual void Move(int x, int z)
    {
        transform.position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        Plays.Add(new Vector2Int(Position.x, Position.y));
    }

    protected bool CanMoveRight(Node node)
    {
        return !node.HasWall[0];
    }

    protected bool CanMoveDown(Node node)
    {
        return !node.HasWall[1];
    }

    protected bool CanMoveLeft(Node node)
    {
        return !node.HasWall[2];
    }

    protected bool CanMoveUp(Node node)
    {
        return !node.HasWall[3];
    }

    public bool[] GetPossibleMoves(Node node)
    {
        bool[] possibleMoves = { CanMoveRight(node), CanMoveDown(node), CanMoveLeft(node), CanMoveUp(node), true }; // last element is for wait, its always true since it is always possible

        return possibleMoves;
    }

    public Vector2Int SimulateMove(Vector2Int currentPosition, int x, int z)
    {
        return currentPosition += new Vector2Int(x, z);
    }

    public abstract void UndoLastMove();
    public abstract void Play();
}

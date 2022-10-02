using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theseus : Character
{
    public static Theseus Instance { private set; get; }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    private void Update()
    {
        if(GameManager.Instance.IsPlayerTurn && !GameManager.Instance.IsGameOver)
        {
            Play();
        }
    }

    protected override void Move(int x, int z)
    {
        base.Move(x, z);
        GameManager.Instance.EndPlayerTurn();
    }

    public override void Play()
    {
        Node node = Maze.Instance.GetNodeFromPosition(Position.x, Position.y);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (node != null && CanMoveRight(node))
            {
                Move(1, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (node != null && CanMoveDown(node))
            {
                Move(0, -1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (node != null && CanMoveLeft(node))
            {
                Move(-1, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (node != null && CanMoveUp(node))
            {
                Move(0, 1);
            }
        }
    }

    /// <summary>
    /// Undoes last move, removes the last element of the 'Plays' list and moves player to the previous position
    /// </summary>
    public override void UndoLastMove()
    {
        if (Plays.Count > 1)
        {
            Vector2 previousPlay = Plays[Plays.Count - 2];
            transform.position = new Vector3(previousPlay.x, transform.position.y, -previousPlay.y);

            Plays.RemoveAt(Plays.Count - 1);
        }
    }

    #region Solver
    public Vector2Int SimulatePlay(Vector2Int currentPosition, int direction)
    {
        switch (direction) // 0 right, 1 down, 2 left, 3 up
        {
            case 0:
                return SimulateMove(currentPosition, 1, 0);
            case 1:
                return SimulateMove(currentPosition, 0, 1);
            case 2:
                return SimulateMove(currentPosition, -1, 0);
            case 3:
                return SimulateMove(currentPosition, 0, -1);
        }
        return new Vector2Int(9999, 9999);
    }
    #endregion
}

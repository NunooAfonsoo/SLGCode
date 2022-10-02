using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : Character
{
    public static Minotaur Instance { private set; get; }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    /// <summary>
    /// Makes the Minotaur play
    /// First tries to move horizontaly to the player's X position and if not possible tries to do the same for the y position
    /// </summary>
    public override void Play()
    { 
        Vector2Int playerDirection = Theseus.Instance.Position - this.Position;

        for(int i = 0; i < 2; i++)
        {
            Node node = Maze.Instance.GetNodeFromPosition(Position.x, Position.y);

            if (playerDirection.x > 0 && node != null && CanMoveRight(node))
            {
                    Move(1, 0);
            }
            else if(playerDirection.x < 0 && node != null && CanMoveLeft(node))
            {
                    Move(-1, 0);
            }
            else
            {
                if (playerDirection.y > 0 && node != null && CanMoveDown(node))
                {
                    Move(0, -1);
                }
                else if (playerDirection.y < 0 && node != null && CanMoveUp(node))
                {
                    Move(0, 1);
                }
            }
            playerDirection = Theseus.Instance.Position - this.Position;
        }

        GameManager.Instance.EndMinotaurTurn();
    }

    /// <summary>
    /// Undoes last move, removes the last element of the 'Plays' list and moves minotaur to the previous position
    /// </summary>
    public override void UndoLastMove()
    {
        if (Plays.Count > 2)
        {
            for(int i = 0; i < 2; i++)
            {
                Vector2 previousPlay = Plays[Plays.Count - 2];
                transform.position = new Vector3(previousPlay.x, transform.position.y, -previousPlay.y);

                Plays.RemoveAt(Plays.Count - 1);
            }
        }
    }

    #region Solver
    public Vector2Int SimulatePlay(Vector2Int currentPosition, Vector2Int theseusPosition, List<Vector2Int> minotaurPlays)
    {
        Vector2Int newPosition = currentPosition;
        Vector2Int playerDirection = theseusPosition - newPosition;

        for (int i = 0; i < 2; i++)
        {
            Node node = Maze.Instance.GetNodeFromPosition(newPosition.x, newPosition.y);

            if (playerDirection.x > 0 && node != null && CanMoveRight(node))
            {
                newPosition = SimulateMove(newPosition, 1, 0);
            }
            else if (playerDirection.x < 0 && node != null && CanMoveLeft(node))
            {
                newPosition = SimulateMove(newPosition, - 1, 0);
            }
            else
            {
                if (playerDirection.y > 0 && node != null && CanMoveDown(node))
                {
                    newPosition = SimulateMove(newPosition, 0, 1);
                }
                else if (playerDirection.y < 0 && node != null && CanMoveUp(node))
                {
                    newPosition = SimulateMove(newPosition, 0, -1);
                }
            }
            minotaurPlays.Add(newPosition);
            playerDirection = theseusPosition - newPosition;
        }

        return newPosition;
    }
    #endregion
}

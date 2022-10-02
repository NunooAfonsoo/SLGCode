using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameSolver : MonoBehaviour
{
    public static GameSolver Instance { private set; get; }
    [SerializeField] private int SimulationDepth;

    private Theseus theseus = Theseus.Instance;
    private Minotaur minotaur = Minotaur.Instance;

    [SerializeField] private TextMeshProUGUI theseusPlaysTMPro;
    [SerializeField] private TextMeshProUGUI minotaurPlaysTMPro;

    private bool foundSolution;
    private void Awake()
    {
        Instance = this;

        theseus = Theseus.Instance;
        minotaur = Minotaur.Instance;
    }

    public void Simulate()
    {
        List<Vector2Int> theseusStartingPosition = new List<Vector2Int>();
        theseusStartingPosition.Add(theseus.Position);

        List<Vector2Int> minotaurStartingPosition = new List<Vector2Int>();
        minotaurStartingPosition.Add(minotaur.Position);

        foundSolution = false;

        SimulateMoves(0, Theseus.Instance.Position, Minotaur.Instance.Position, theseusStartingPosition, minotaurStartingPosition);
    }

    private void SimulateMoves(int depth, Vector2Int tPosition, Vector2Int mPosition, List<Vector2Int> theseusPlays, List<Vector2Int> minotaurPlays)
    {
        Node theseusNode = Maze.Instance.GetNodeFromPosition(tPosition.x, tPosition.y);

        // stoppage conditions
        if(depth > SimulationDepth || theseusNode == null || tPosition == mPosition || foundSolution)
        {
            Vector2Int firstElement = theseusPlays[0];
            theseusPlays.Clear();
            theseusPlays.Add(firstElement);

            firstElement = minotaurPlays[0];
            minotaurPlays.Clear();
            minotaurPlays.Add(firstElement);
            return;
        }
        else if(theseusNode.Position == GameManager.Instance.ExitNodePosition)
        {
            string p = "";
            for(int i = 0; i < theseusPlays.Count - 1; i++)
            {
                Vector2Int diff = theseusPlays[i + 1] - theseusPlays[i];

                 p += GetMovementDirection(diff) + "\n";
            }
            theseusPlaysTMPro.text = p;

            p = "";
            for (int i = 0; i < minotaurPlays.Count - 1; i++)
            {
                Vector2Int diff = minotaurPlays[i + 1] - minotaurPlays[i];

                p += GetMovementDirection(diff) + "\n";
            }
            minotaurPlaysTMPro.text = p;

            foundSolution = true;
            return;
        }

        // Trying new move and increasing the depth of the search
        bool[] possibleMoves = theseus.GetPossibleMoves(theseusNode);
        int PossibleMovesLength = theseus.GetPossibleMoves(theseusNode).Length; // Avoiding recalculating cycle

        for (int i = 0; i < PossibleMovesLength; i++)
        {
            if (possibleMoves[i] && CanTheseusMoveRightWithoutDying(i, tPosition, mPosition))
            {
                if(i != 4)
                {
                    Vector2Int newTPosition = theseus.SimulatePlay(tPosition, i);
                    Vector2Int newMPosition = minotaur.SimulatePlay(mPosition, newTPosition, minotaurPlays);
                    theseusPlays.Add(newTPosition);

                    SimulateMoves(depth + 1, newTPosition, newMPosition, theseusPlays, minotaurPlays);
                }
                else
                {
                    Vector2Int newMPosition = minotaur.SimulatePlay(mPosition, tPosition, minotaurPlays);
                    theseusPlays.Add(tPosition);

                    SimulateMoves(depth + 1, tPosition, newMPosition, theseusPlays, minotaurPlays);
                }
            }
        }
    }

    /// <summary>
    /// Returns the movement direction from a vector2
    /// </summary>
    /// <param name="diff"></param>
    /// <returns></returns>
    private string GetMovementDirection(Vector2Int diff)
    {
        if (diff.x > 0)
        {
            return "Right";
        }
        else if (diff.x < 0)
        {
            return "Left";
        }
        else if (diff.y > 0)
        {
            return "Down";
        }
        else if (diff.y < 0)
        {
            return "Up";
        }
        return "Didn't move";
    }

    /// <summary>
    /// Returns true if Theseus can move in direction without dying
    /// </summary>
    /// <param name="direction"> Direction to move </param>
    /// <param name="tPosition"> Theseus Position </param>
    /// <param name="mPosition"> Minotaur Position </param>
    /// <returns></returns>
    private bool CanTheseusMoveRightWithoutDying(int direction, Vector2Int tPosition, Vector2Int mPosition)
    {
        switch (direction) // 0 right, 1 down, 2 left, 3 up
        {
            case 0:
                return mPosition != tPosition + Vector2Int.right;
            case 1:
                return mPosition != tPosition + Vector2Int.up;
            case 2:
                return mPosition != tPosition + Vector2Int.left;
            case 3:
                return mPosition != tPosition + Vector2Int.down;
        }
        return false;
    }
}

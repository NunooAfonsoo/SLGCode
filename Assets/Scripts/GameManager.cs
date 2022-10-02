using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }
    public bool IsPlayerTurn { private set; get; }
    public bool IsGameOver { private set; get; }

    [SerializeField] private Transform exitNode;
    public Vector2Int ExitNodePosition { private set; get; }

    void Awake()
    {
        Instance = this;
        IsPlayerTurn = true;
        IsGameOver = false;

        ExitNodePosition = new Vector2Int((int)exitNode.position.x, -(int)exitNode.position.z);
    }

    private void Update()
    {
        if(!IsGameOver)
        {
            if(Input.GetKeyDown(KeyCode.U))
            {
                Undo();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                Wait();
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                PreviousLevel();
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                NextLevel();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                GameSolver.Instance.Simulate();
            }

            if (Theseus.Instance.Position == ExitNodePosition)
            {
                GameWon();
            }
            else if (Minotaur.Instance.Position == Theseus.Instance.Position)
            {
                GameOver();
            }
        }
    }

    public void EndPlayerTurn()
    {
        IsPlayerTurn = false;

        Minotaur.Instance.Play();
    }

    public void EndMinotaurTurn()
    {
        IsPlayerTurn = true;
    }

    /// <summary>
    /// Undoes last move
    /// </summary>
    public void Undo()
    {
        Theseus.Instance.UndoLastMove();
        Minotaur.Instance.UndoLastMove();

        IsPlayerTurn = true;
    }

    /// <summary>
    /// Restarts current scene
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Restarts current scene
    /// </summary>
    public void Wait()
    {
        Minotaur.Instance.Play();
    }

    /// <summary>
    /// Shows the gameOverCanvas when player loses
    /// </summary>
    private void GameOver()
    {
        IsGameOver = true;
        CanvasManager.Instance.ShowGameOverCanvas();
    }

    /// <summary>
    /// Shows the gameWonCanvas when player has no more levels to play
    /// </summary>
    private void GameWon()
    {
        IsGameOver = true;

        if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            CanvasManager.Instance.ShowGameWonCanvas();
        }
    }

    /// <summary>
    /// Loads next level
    /// </summary>
    private void NextLevel()
    {
        if(SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    /// <summary>
    /// Loads previous level
    /// </summary>
    private void PreviousLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}

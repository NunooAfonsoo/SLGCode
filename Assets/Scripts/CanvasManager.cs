using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { private set; get; }

    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject gameWonCanvas;
    [SerializeField] private GameObject gameInstructionsCanvas;

    private void Awake()
    {
        Instance = this;
        gameOverCanvas.SetActive(false);
        gameWonCanvas.SetActive(false);
        gameInstructionsCanvas.SetActive(true);
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver && (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space)))
        {
            GameManager.Instance.Restart();
        }
    }

    public void ShowGameOverCanvas()
    {
        gameOverCanvas.SetActive(true);
        gameInstructionsCanvas.SetActive(false);
    }

    public void ShowGameWonCanvas()
    {
        gameWonCanvas.SetActive(true);
        gameInstructionsCanvas.SetActive(false);
    }
}

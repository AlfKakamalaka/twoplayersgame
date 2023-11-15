using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Value
{
    None = 0,
    X = 1,
    O = -1
}
public class Gameplay : MonoBehaviour
{
    public static Gameplay instance;
    private int[,] board;
    public Cell[,] cells;
    private int round;
    public Value firstPlayer;
    public PlayerAI playerAI;
    private bool gameOver;
    
    public TextMeshProUGUI text;
    public Canvas canvas;
    private void Awake()
    {
        instance = this;
        cells = new Cell[3, 3];
    }
    
    private void Start()
    {
        StartGame();
    }
    
    public void StartGame()
    {
        firstPlayer = (Random.Range(0, 2) == 0) ?  Value.X : Value.O;
        board = new int[3, 3];
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                cells[i, j].x = i;
                cells[i, j].y = j;
            }
        }
        if(NextPlayer() != firstPlayer && GameService.isAI) playerAI.Choose();
    }

    public void RestartGame()
    {
        round = 0;
        board = new int[3, 3];
        foreach (var cell in cells)
        {
            cell.Show(Value.None);
        }
        gameOver = false;
        canvas.gameObject.SetActive(false);
        StartGame();
    }
    
    public int[,] GetBoard()
    {
        return board;
    }
    
    public Value NextPlayer()
    {
        if(((round % 2) == 0 && firstPlayer == Value.X) || ((round % 2) != 0 && firstPlayer == Value.O))
            return firstPlayer;
        else
            return (firstPlayer == Value.X) ? Value.O : Value.X;
    }

    public void NextRound()
    {
        round++;
        var nextPlayer = NextPlayer();
        if (nextPlayer != firstPlayer && GameService.isAI)
        {
            playerAI.Choose();
        }
    }
    public void LinkCell(Cell cell, int x, int y)
    {
        cells[x, y] = cell;
    }
    
    public void SetValue(int x, int y, Value value)
    {
        if(gameOver) return;
        board[x, y] = (int)value;
        cells[x, y].Show(value);
        CheckWinCondition();
        NextRound();
    }

    public int GetValue(int x, int y)
    {
        return board[x, y];
    }

    IEnumerator ShowPopupWithDelay()
    {
        foreach (var cell in cells)
        {
            if(cell.animator == null) continue;
            yield return new WaitForSeconds(0.2f);
            cell.Hide();
        }
        yield return new WaitForSeconds(1);
        canvas.gameObject.SetActive(true);
    }
    public void CheckWinCondition()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            int sumCollums = 0;
            int sumRow = 0;
            for (int j = 0; j < board.GetLength(1); j++)
            {
                sumRow += board[i, j];
                sumCollums += board[j, i];
            }

            if (sumRow == 3 || sumCollums == 3)
            {
                gameOver = true;
                StartCoroutine(ShowPopupWithDelay());
                text.text = "First player win!";
            }
            else if (sumCollums == -3 || sumRow == -3)
            {
                gameOver = true;
                StartCoroutine(ShowPopupWithDelay());
                text.text = "Second player win!";
            }
            else if (round == 8)
            {
                gameOver = true;
                StartCoroutine(ShowPopupWithDelay());
                text.text = "No one win!";
            }
        }
        
        int sumDiagonal_LR = 0;
        int sumDiagonal_RL = 0;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            sumDiagonal_LR += board[i, i];
            sumDiagonal_RL += board[i, board.GetLength(0) - i - 1];
            
        }

        if (sumDiagonal_RL == 3 || sumDiagonal_LR == 3)
        {
            gameOver = true;
            StartCoroutine(ShowPopupWithDelay());
            text.text = "First player win!";
        }
        else if (sumDiagonal_LR == -3 || sumDiagonal_RL == -3)
        {
            gameOver = true;
            StartCoroutine(ShowPopupWithDelay());
            text.text = "Second player win!";
        }
        else if (round == 8)
        {
            gameOver = true;
            StartCoroutine(ShowPopupWithDelay());
            text.text = "No one win!";
        }
    }
}

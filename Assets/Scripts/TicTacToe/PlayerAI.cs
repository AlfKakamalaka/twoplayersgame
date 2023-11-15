using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public int[,] weights= new int[3,3];
    

    private int AddColumnAndRow(int column,int row, int [,] board)
    {
        int value = 0;
        int valueColumn = 0;
        int valueRow = 0;
        
        for (int i = 0; i < board.GetLength(0); i++)
        {
            valueColumn += board[i, column];
            valueRow += board[row, i];
        }
        
        value += (valueColumn == 2 || valueColumn == -2)? 10 : valueColumn;
        value += (valueRow == 2 || valueRow == -2)? 10 : valueRow;
        return value;
    }
    
    private int AddDiagonal(int [,] board,int x, int y)
    {
        int value = 0;
        int valueDiagonal = 0;
        int valueAntiDiagonal = 0;
        bool isDiagonal = false;
        bool isAntiDiagonal = false;
        
        for (int i = 0; i < board.GetLength(0); i++)
        {

            valueDiagonal += board[i, i];
            var antiPoint = board.GetLength(0) - i - 1;

            valueAntiDiagonal += board[i, antiPoint];

            isDiagonal = x == i && y == i;

            isAntiDiagonal = x == i && y == antiPoint;

        }

        if(isDiagonal)
        {
            value += (valueDiagonal == 2 || valueDiagonal == -2) ? 10 : valueDiagonal;
        }
        
        if(isAntiDiagonal)
            value += (valueAntiDiagonal == 2 || valueAntiDiagonal == -2)? 10 : valueAntiDiagonal;
        return value;
    }
    
    public void FillArray()
    {
        var board = Gameplay.instance.GetBoard();
        
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if(board[i, j] != 0)
                {
                    weights[i, j] = -100;
                }
                else
                {
                    weights[i, j] = AddColumnAndRow(j,i, board) + AddDiagonal(board, i, j);
                }
            }
        }
    }
    
    public void Choose()
    {
        FillArray();
        int max = -1000;
        int x = 0;
        int y = 0;
        for (int i = 0; i < weights.GetLength(0); i++)
        {
            for (int j = 0; j < weights.GetLength(1); j++)
            {
                if (weights[i, j] > max)
                {
                    max = weights[i, j];
                    x = i;
                    y = j;
                }
            }
        }
        
        var nextPlayer = Gameplay.instance.NextPlayer();
        Gameplay.instance.SetValue(x, y, nextPlayer);
        
    }
}

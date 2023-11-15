using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class King : BaseFigure
//Сделать проверку для остальных фигур на тип фигуры король. Если король, то не добавлять в список доступных ходов.

{
    public bool isKing = false;
    public BaseFigure shahFigure;
    
    public bool firstMove = true;
    public List<int2> retraceShah = new List<int2>();
    public void RetraceShah(BaseFigure figure)
    {
        if(figure.color == color){return;}
        retraceShah.Clear();
        retraceShah.Add(figure.coords);
        switch (figure.type)
        {
            case FigureType.Rook:
                RetraceRook(figure);
                break;
            case FigureType.Bishop:
                RetraceBishop(figure);
                break;
            case FigureType.Queen:
                RetraceBishop(figure);
                RetraceRook(figure);
                break;
        }
        ChessManager.instance.KingSecurity(color);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color. blue;
        foreach (var cell in retraceShah)
        {
            Gizmos.DrawSphere(Grid.Instance.GetWorldPosition(cell.x, cell.y), 0.5f);
            Debug.Log(coords.x + " " + coords.y);
        }
    }

    public void RetraceRook(BaseFigure figure)
    {
        int x = figure.coords.x - coords.x;
        int y = figure.coords.y - coords.y;
        if (x == 0)
        {
            if (y > 0)
            {
                for (int i = 1; i < y; i++)
                {
                    retraceShah.Add(new int2(coords.x, coords.y + i));
                }
            }
            else
            {
                for (int i = 1; i < -y; i++)
                {
                    retraceShah.Add(new int2(coords.x, coords.y - i));
                }
            }
        }
        else if(y == 0)
        {
            if (x > 0)
            {
                for (int i = 1; i < x; i++)
                {
                    retraceShah.Add(new int2(coords.x + i, coords.y));
                }
            }
            else
            {
                for (int i = 1; i < -x; i++)
                {
                    retraceShah.Add(new int2(coords.x - i, coords.y));
                }
            }
        }
        
    }
    
    public void RetraceBishop(BaseFigure figure)
    {
        int x = figure.coords.x - coords.x;
        int y = figure.coords.y - coords.y;
        
        if (x > 0 && y > 0)
        {
            for (int i = 1; i < x; i++)
            {
                retraceShah.Add(new int2(coords.x + i, coords.y + i));
            }
        }
        else if (x > 0 && y < 0)
        {
            for (int i = 1; i < x; i++)
            {
                retraceShah.Add(new int2(coords.x + i, coords.y - i));
            }
        }
        else if (x < 0 && y > 0)
        {
            for (int i = 1; i < -x; i++)
            {
                retraceShah.Add(new int2(coords.x - i, coords.y + i));
            }
        }
        else if (x < 0 && y < 0)
        {
            for (int i = 1; i < -x; i++)
            {
                retraceShah.Add(new int2(coords.x - i, coords.y - i));
            }
        }
    }
    public void RetraceQueen(BaseFigure figure)
    {
        
    }
    
    public override void AnalyseMove()
    {
        var grid = Grid.Instance;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int x = coords.x + i;
                int y = coords.y + j;
                if (!grid.IsOnBoard(x, y)) { continue; }

                if (grid.AnalysisCells[x, y])
                {
                    continue;
                }
                if (grid.TypesOnBoard[x, y] == FigureType.None)
                {
                    availableMoves.TryAdd(new int2(x, y), grid.ObjFigures[x, y]);
                }
                else if (grid.ObjFigures[x, y].color != color)
                {
                    availableMoves.TryAdd(new int2(x, y), grid.ObjFigures[x, y]);
                }
                
            }
        }
    }
    
    public override bool Move(int x, int y)
    {
        var move = base.Move(x, y);
        if (move)
        {
            firstMove = false;
        }
        return move;
    }
    
}
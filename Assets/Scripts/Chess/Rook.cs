using NUnit.Framework.Internal.Filters;
using Unity.Mathematics;
using UnityEngine;

public class Rook : BaseFigure
{
    public bool firstMove = true;
    public override void AnalyseMove()
    {
        Grid grid = Grid.Instance;
        int y;
        int x;

        for (int i = 0; i < 8; i++)
        {
            y = coords.y + i;
            if(!CheckMoveCell(grid, coords.x, y))break;
        }
        for (int i = 0; i < 8; i++)
        {
            y = coords.y - i;
            if(!CheckMoveCell(grid, coords.x, y))break;
        }
        for (int i = 0; i < 8; i++)
        {
            x = coords.x + i;
            if(!CheckMoveCell(grid, x, coords.y))break;
        }
        for (int i = 0; i < 8; i++)
        {
            x = coords.x - i;
            if(!CheckMoveCell(grid, x, coords.y))break;
        }

    }

    private bool CheckMoveCell(Grid grid, int x, int y)
    {
        if(!grid.IsOnBoard(x,y)){return false;}
        if(coords.x == x && coords.y == y){return true; }
        if (grid.TypesOnBoard[x, y] == FigureType.None)
        {
            CalculateMove(grid, x, y);
            return true;
        }
        if (grid.TypesOnBoard[x, y] == FigureType.King)
        {
            King king = grid.ObjFigures[x, y] as King;
            king.RetraceShah(this);
        }
        CalculateMove(grid, x, y);
        return false;
    }
    private void CalculateMove(Grid grid, int x, int y)
    {
        if(!grid.IsOnBoard(x,y)){return;}
        if(CheckNonRetraceMove(x, y)) return;
        
        if (grid.TypesOnBoard[x, y] == FigureType.None )
        {
            availableMoves.TryAdd(new int2(x, y), grid.ObjFigures[x, y]);
        }
        else if ( grid.ObjFigures[x, y].color != color)
        {
            availableMoves.TryAdd(new int2(x, y), grid.ObjFigures[x, y]);
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
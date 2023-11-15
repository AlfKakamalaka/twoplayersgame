using Unity.Mathematics;
using UnityEngine;

public class Pawn : BaseFigure
{
    private bool firstMove = true;
    public override void AnalyseMove()
    {
        var grid = Grid.Instance;
        var y =(color == FigureColor.White) ? 1 : -1;
        y += coords.y;
        
        var diagonalX = coords.x - 1;
        CheckKillCell(grid, diagonalX, y);
        
        var antiDiagonalX = coords.x + 1;
        CheckKillCell(grid, antiDiagonalX, y);

        CheckMoveCell(grid,coords.x, y);
        if (firstMove)
        {
            y = (color == FigureColor.White) ? 2 : -2;
            y += coords.y;
            CheckMoveCell(grid,coords.x, y);
        }
        
    }

    public override void FillAnalysisGrid(bool[,] analysisGrid)
    {
        var grid = Grid.Instance;
        var y =(color == FigureColor.White) ? 1 : -1;
        y += coords.y;
        var diagonalX = coords.x - 1;
        if (CheckCanKillCell(grid, diagonalX, y))
        {
            grid.AnalysisCells[diagonalX, y] = true;
        }
        
        var antiDiagonalX = coords.x + 1;
        if (CheckCanKillCell(grid, antiDiagonalX, y))
        {
            grid.AnalysisCells[antiDiagonalX, y] = true;
        }
    }

    private void CheckMoveCell(Grid grid,int x, int y)
    {
        if(!grid.IsOnBoard(x,y)){return;}
        if (grid.TypesOnBoard[x, y] == FigureType.None)
        {
            availableMoves.TryAdd(new int2(x, y), grid.ObjFigures[x, y]);
        }
    }

    private bool CheckKillCell(Grid grid, int x, int y)
    {
        if(!grid.IsOnBoard(x,y)){return false;}
        if (grid.TypesOnBoard[x, y] != FigureType.None && grid.ObjFigures[x, y].color != color)
        {
            availableMoves.TryAdd(new int2(x, y), grid.ObjFigures[x, y]);
            return true;
        }
        return false;
    }
    private bool CheckCanKillCell(Grid grid, int x, int y)
    {
        if(!grid.IsOnBoard(x,y)){return false;}
        if(grid.TypesOnBoard[x,y] == FigureType.King)
        {
            (grid.ObjFigures[x, y] as King).RetraceShah(this);// повторть асы
            return true;
        }
        if (grid.TypesOnBoard[x, y] == FigureType.None)
        { 
            return true;
        }
        
        return false;
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
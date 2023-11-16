using System.Collections.Generic;
using Unity.Mathematics;

public class Knight : BaseFigure
{
    public override void AnalyseMove()
    {
        var grid = Grid.Instance;
        
        int y = coords.y + 2;
        int x = coords.x + 1;
        CalculateMove(grid, x, y);
        x = coords.x - 1;
        CalculateMove(grid, x, y);
        
        y = coords.y + 1;
        x = coords.x + 2;
        CalculateMove(grid, x, y);
        y = coords.y - 1;
        CalculateMove(grid, x, y);
        
        y = coords.y - 2;
        x = coords.x + 1;
        CalculateMove(grid, x, y);
        x = coords.x - 1;
        CalculateMove(grid, x, y);
        
        y = coords.y - 1;
        x = coords.x - 2;
        CalculateMove(grid, x, y);
        y = coords.y + 1;
        CalculateMove(grid, x, y);
    }

    private void CalculateMove(Grid grid, int x, int y)
    {
        if(!grid.IsOnBoard(x,y)){return;}
        if(CheckNonRetraceMove(x, y)) return;
        if (grid.TypesOnBoard[x, y] == FigureType.King)
        {
            King king = grid.ObjFigures[x, y] as King;
            king.RetraceShah(this);
        }
        if (grid.TypesOnBoard[x, y] == FigureType.None )
        {
            availableMoves.TryAdd(new int2(x, y), grid.ObjFigures[x, y]);
        }
        else if ( grid.ObjFigures[x, y].color != color)
        {
            availableMoves.TryAdd(new int2(x, y), grid.ObjFigures[x, y]);
        }
    }




    
}
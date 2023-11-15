using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public enum FigureCost
{
    Pawn = 10,
    Knight = 30,
    Bishop = 30,
    Rook = 50,
    Queen = 90,
    King = 900
}
public class Move
{
    public int2 from;
    public int2 to;
    public FigureType saveFigureTypeMove;
    public FigureType saveFigureTypeThis;
    public BaseFigure saveFigureObjMove;
    public FigureType typeTo;
    public FigureType typeFrom;
    public FigureColor colorTo;
    public FigureColor colorFrom;
    public BaseFigure saveFigureObjThis;
    public Move(int2 from, int2 to)
    {
        this.from = from;
        this.to = to;
    }
    
}
public class ChessAI : MonoBehaviour
{
    public static ChessAI instance;
    
    public int[,] pawnWeights = new int[8,8]
    {
        {0,0,0,0,0,0,0,0},
        {50,50,50,50,50,50,50,50},
        {10,10,20,30,30,20,10,10},
        {5,5,10,25,25,10,5,5},
        {0,0,0,20,20,0,0,0},
        {5,-5,-10,0,0,-10,-5,5},
        {5,10,10,-20,-20,10,10,5},
        {0,0,0,0,0,0,0,0}
    };
    public int[,] knightWeights = new int[8,8]
    {
        {-50,-40,-30,-30,-30,-30,-40,-50},
        {-40,-20,0,0,0,0,-20,-40},
        {-30,0,10,15,15,10,0,-30},
        {-30,5,15,20,20,15,5,-30},
        {-30,0,15,20,20,15,0,-30},
        {-30,5,10,15,15,10,5,-30},
        {-40,-20,0,5,5,0,-20,-40},
        {-50,-40,-30,-30,-30,-30,-40,-50}
    };
    public int[,] bishopWeights = new int[8,8]
    {
        {-20,-10,-10,-10,-10,-10,-10,-20},
        {-10,0,0,0,0,0,0,-10},
        {-10,0,5,10,10,5,0,-10},
        {-10,5,5,10,10,5,5,-10},
        {-10,0,10,10,10,10,0,-10},
        {-10,10,10,10,10,10,10,-10},
        {-10,5,0,0,0,0,5,-10},
        {-20,-10,-10,-10,-10,-10,-10,-20}
    };
    
    public int[,] queenWeights = new int[8,8]
    {
        {-20,-10,-10,-5,-5,-10,-10,-20},
        {-10,0,0,0,0,0,0,-10},
        {-10,0,5,5,5,5,0,-10},
        {-5,0,5,5,5,5,0,-5},
        {0,0,5,5,5,5,0,-5},
        {-10,5,5,5,5,5,0,-10},
        {-10,0,5,0,0,0,0,-10},
        {-20,-10,-10,-5,-5,-10,-10,-20}
    };

    public int[,] rookWeights = new int[8, 8]
    {
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { 5, 10, 10, 10, 10, 10, 10, 5 },
        { -5, 0, 0, 0, 0, 0, 0, -5 },
        { -5, 0, 0, 0, 0, 0, 0, -5 },
        { -5, 0, 0, 0, 0, 0, 0, -5 },
        { -5, 0, 0, 0, 0, 0, 0, -5 },
        { -5, 0, 0, 0, 0, 0, 0, -5 },
        { 0, 0, 0, 5, 5, 0, 0, 0 }
    };
    public int[,] kingWeightsBlack = new int[8, 8]
    {
        { 20, 30, 10, 0, 0, 10, 30, 20 },
        { 20, 20, 0, 0, 0, 0, 20, 20 },
        { -10, -20, -20, -20, -20, -20, -20, -10 },
        { -20, -30, -30, -40, -40, -30, -30, -20 },
        { -30, -40, -40, -50, -50, -40, -40, -30 },
        { -30, -40, -40, -50, -50, -40, -40, -30 },
        { -30, -40, -40, -50, -50, -40, -40, -30 },
        { -30, -40, -40, -50, -50, -40, -40, -30 }
    };
    public int[,] kingWeightsWhite = new int[8, 8]
    {
        { -30, -40, -40, -50, -50, -40, -40, -30 },
        { -30, -40, -40, -50, -50, -40, -40, -30 },
        { -30, -40, -40, -50, -50, -40, -40, -30 },
        { -30, -40, -40, -50, -50, -40, -40, -30 },
        { -20, -30, -30, -40, -40, -30, -30, -20 },
        { -10, -20, -20, -20, -20, -20, -20, -10 },
        { 20, 20, 0, 0, 0, 0, 20, 20 },
        { 20, 30, 10, 0, 0, 10, 30, 20 }
    };
    
    
    
    public int Evaluate()
    {
        int score = 0;
        var grid = Grid.Instance;

        foreach (var item in grid.ObjFigures)
        {
            if(item == null) continue;
            int modifier = (item.color == FigureColor.Black) ? 1 : -1;
            switch (item.type)
            {
                case FigureType.None:
                    score += 0;
                    break;
                case FigureType.Pawn:
                    score += (int)FigureCost.Pawn * modifier + pawnWeights[item.coords.x,item.coords.y];
                    break;
                case FigureType.Rook:
                    score += (int)FigureCost.Rook * modifier + rookWeights[item.coords.x,item.coords.y];
                    break;
                case FigureType.Knight:
                    score += (int)FigureCost.Knight * modifier + knightWeights[item.coords.x,item.coords.y];
                    break;
                case FigureType.Bishop:
                    score += (int)FigureCost.Bishop * modifier + bishopWeights[item.coords.x,item.coords.y];
                    break;
                case FigureType.Queen:
                    score += (int)FigureCost.Queen * modifier + queenWeights[item.coords.x,item.coords.y];
                    break;
                case FigureType.King:
                    score += (int)FigureCost.King * modifier + (item.color == FigureColor.Black ? kingWeightsBlack[item.coords.x,item.coords.y] : kingWeightsWhite[item.coords.x,item.coords.y]);
                    break;
               
            }
        }

        return score;
    }
    public void PawnTransformation(int x, int y, int newX, int newY,FigureColor color)
    {
        var grid = Grid.Instance;

        var figure = grid.ObjFigures[x, y];
        grid.TypesOnBoard[x, y] = FigureType.Queen;
        figure.coords = new int2(x,y);
        figure.color = color;
        figure.type = FigureType.Queen;
    }
   
    public void SimulateMove(int x, int y, int newX, int newY)
    {
        var grid = Grid.Instance;
        if (grid.TypesOnBoard[x, y] == FigureType.Pawn)
        {
            if (newY == 7)
            {
                PawnTransformation(x, y, newX, newY, FigureColor.White);
                return;
            }
            if (newY == 0)
            {
                PawnTransformation(x, y, newX, newY, FigureColor.Black);
                return;
            }
        }
        grid.TypesOnBoard[newX, newY] = grid.TypesOnBoard[x, y];
        grid.TypesOnBoard[x, y] = FigureType.None;

      
      
        grid.ObjFigures[newX, newY] = grid.ObjFigures[x, y];
        grid.ObjFigures[newX, newY].coords = new int2(newX, newY);
        grid.ObjFigures[x, y] = null;
        
        
    }

    public void MakeMove(Move move)
    {
        var grid = Grid.Instance;
        move.saveFigureObjMove = grid.ObjFigures[move.to.x, move.to.y];
        move.saveFigureTypeMove = grid.TypesOnBoard[move.to.x, move.to.y];
        move.saveFigureTypeThis = grid.TypesOnBoard[move.from.x, move.from.y];
        move.saveFigureObjThis = grid.ObjFigures[move.from.x, move.from.y];
        if(grid.ObjFigures[move.to.x, move.to.y] != null)
        {
            
            move.typeTo = grid.ObjFigures[move.to.x, move.to.y].type;
            move.colorTo = grid.ObjFigures[move.to.x, move.to.y].color;
        }

        if (grid.ObjFigures[move.from.x, move.from.y] != null)
        {
            
            move.typeFrom = grid.ObjFigures[move.from.x, move.from.y].type;
            move.colorFrom = grid.ObjFigures[move.from.x, move.from.y].color;
            
        }
       
        
    }
    public void UnMakeMove(Move move)
    {
        var grid = Grid.Instance;
        

        grid.TypesOnBoard[move.to.x, move.to.y] = move.saveFigureTypeMove;
        grid.ObjFigures[move.to.x, move.to.y] = move.saveFigureObjMove;
        grid.TypesOnBoard[move.from.x, move.from.y] = move.saveFigureTypeThis;
        grid.ObjFigures[move.from.x, move.from.y] = move.saveFigureObjThis;
        if(grid.ObjFigures[move.to.x, move.to.y] != null)
        {        
            grid.ObjFigures[move.to.x, move.to.y].coords = move.to;
            grid.ObjFigures[move.to.x, move.to.y].type = move.typeTo;
            grid.ObjFigures[move.to.x, move.to.y].color = move.colorTo;
        }

        if (grid.ObjFigures[move.from.x, move.from.y] != null)
        {
            grid.ObjFigures[move.from.x, move.from.y].coords = move.from;
            grid.ObjFigures[move.from.x, move.from.y].type = move.typeFrom;
            grid.ObjFigures[move.from.x, move.from.y].color = move.colorFrom;
        }

    }
    public void BestMove()
    {
        var grid = Grid.Instance;

        int bestScore = -1000000;
        BaseFigure bestmoveObj = null;
        int2 bestmove = new int2(0, 0);
        
        var moves = GetAvailableMoves((ChessManager.instance.player == FigureColor.White) ? FigureColor.Black : FigureColor.White);
        foreach (var move in moves)
        {
            MakeMove(move);
            SimulateMove(move.from.x, move.from.y, move.to.x, move.to.y);
            var score = MiniMax( 4, -1000000, 1000000, (ChessManager.instance.player == FigureColor.White ));
            UnMakeMove(move);
            if (score > bestScore)
            {
                bestScore = score;
                bestmove = move.to;
                bestmoveObj = grid.ObjFigures[move.from.x, move.from.y];
            }

        }

        if (bestmoveObj != null)
        {
            bestmoveObj.Move(bestmove.x, bestmove.y);
        }
        
    }

    public List<Move> GetAvailableMoves(FigureColor color)
    {
        List<Move> moves = new List<Move>();
        var grid = Grid.Instance;

        foreach (var gridObjFigure in grid.ObjFigures)
        {
            if (gridObjFigure != null)
            {
                if (gridObjFigure.color == color)
                {
                    // Retrieve the figure from the grid
                    var figure = grid.ObjFigures[gridObjFigure.coords.x, gridObjFigure.coords.y];

                    // Clear availableMoves for the current figure
                    figure.availableMoves.Clear();

                    // Analyze available moves for the current figure
                    figure.AnalyseMove();

                    // Add moves to the list
                    foreach (var item in figure.availableMoves.Keys)
                    {
                        moves.Add(new Move(gridObjFigure.coords, item));
                    }
                }
            }
        }

        return moves;
    }

    
    public int MiniMax( int depth, int alpha, int beta, bool maximizingPlayer)
    {
        var grid = Grid.Instance;
        bool isGameOver;
        
        isGameOver= ChessManager.instance.CheckIsShahBlack();
        
        
        if(depth == 0 || isGameOver)//или конец игры
        {
            return Evaluate();
        }

        if (maximizingPlayer)
        {
            int maxEval = -1000000;
            
            var moves = GetAvailableMoves( FigureColor.White);

            foreach (var move in moves)
            {
                MakeMove(move);
                SimulateMove(move.from.x, move.from.y, move.to.x, move.to.y);
                
                var score = MiniMax( depth-1, -1000000, 1000000, false);
                UnMakeMove(move);
                maxEval = math.max(score, maxEval);
                alpha = math.max(alpha,score);
                if(beta <= alpha)
                {
                    break;
                }
            }
            return maxEval;
        }
        else
        {
            int minEval = 1000000;
            var moves = GetAvailableMoves(FigureColor.Black);

            foreach (var move in moves)
            {
                MakeMove(move);
                SimulateMove(move.from.x, move.from.y, move.to.x, move.to.y);
                
                var score = MiniMax( depth-1, -1000000, 1000000, true);
                UnMakeMove(move);
                minEval = math.min(score, minEval);
                beta = math.min(beta,score);
                if(beta <= alpha)
                {
                    break;
                }
            }
            return minEval;
        }
        
    }
    
    private void Awake()
    {
        instance = this;
    }
    
}
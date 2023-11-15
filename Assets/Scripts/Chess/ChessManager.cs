using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class ChessManager : MonoBehaviour
{
    public static ChessManager instance;
    private int round;
    public bool gameOver;
    public Canvas canvasShah;
    public TextMeshProUGUI textShah;
    public FigureColor player;
    public bool isAI;
    public bool isShahBlack;
    public bool isShahWhite;
    public TextMeshProUGUI textMat;
    public Canvas canvasMat;
    public bool KingInCheck;
    public King WhiteKing;
    public King BlackKing;
    private void Awake()
    {
        instance = this;
    }
    public void RestartGame()
    {
        round = 0;
        foreach (var item in Grid.Instance.ObjFigures)
        {
            item.Die();
        }
        Grid.Instance.Start();
        gameOver = false;
        canvasMat.gameObject.SetActive(false);
    }
    IEnumerator ShowPopupWithShah()
    {
        canvasShah.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        canvasShah.gameObject.SetActive(false);
    }

    public FigureColor NextPlayer()
    {
        if(((round % 2) == 0 && player == FigureColor.White) || ((round % 2) != 0 && player == FigureColor.Black))
            return player;
        else
            return (player == FigureColor.White) ? FigureColor.Black : FigureColor.White;
    }
    public void NextRound()
    {
        ClearAnalysis();
        RequstAnalysis();
        CameraController.instance.RotateCamera();
        if (CheckIsShahWhite())
        {
            StartCoroutine(ShowPopupWithShah());
            textShah.text = "Шах чёрным";
            isShahWhite = true;
        }
        else
        {
            isShahWhite = false;
        }

        if (CheckIsShahBlack())
        {
            
            StartCoroutine(ShowPopupWithShah());
            textShah.text = "Шах белым";
            isShahBlack = true;
        }
        else
        {
            isShahBlack = false;
        }
        CheckIsMatBlack();
        CheckIsMatWhite();
        
        round++;
        var nextPlayer = NextPlayer();
        if (nextPlayer != player && isAI)
        {
           ChessAI.instance.BestMove();
        }
        

        
    }

    public void ClearAnalysis()
    {
        Grid.Instance.AnalysisCells = new bool[8,8];
        foreach (var item in Grid.Instance.ObjFigures)
        {
            if (item != null)
            {
                item.availableMoves.Clear();
            }
        }
    }


    public void RequstAnalysis()
    {
        foreach (var item in Grid.Instance.ObjFigures)
        {
            if (item != null)
            {
                if (NextPlayer() == item.color && item.type == FigureType.Pawn)
                {
                    item.FillAnalysisGrid(Grid.Instance.AnalysisCells); 
                }
                item.AnalyseMove();
                if (NextPlayer() == item.color)
                {
                    item.FillAnalysisGrid(Grid.Instance.AnalysisCells); 
                }
                
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (Grid.Instance == null) return;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j <8; j++)
            {
                if (Grid.Instance.AnalysisCells[i, j])
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(Grid.Instance.GetWorldPosition(i,j), Vector3.one * 0.5f);
                }
            }
        }
    }

    public void KingSecurity(FigureColor color)
    {
        foreach (var figure in Grid.Instance.ObjFigures)
        {
            if(figure == null) continue;
            List<int2> avaMoves = new List<int2>();
            avaMoves.AddRange(figure.availableMoves.Keys);
            foreach (var figureAvailableMove in avaMoves)
            {
                if (color == FigureColor.White)
                {
                    if (!WhiteKing.retraceShah.Contains(figureAvailableMove))
                    {
                        figure.availableMoves.Remove(figureAvailableMove);
                    }
                }
                else
                {
                    if (!BlackKing.retraceShah.Contains(figureAvailableMove))
                    {
                        figure.availableMoves.Remove(figureAvailableMove);
                    }
                }
            }
        }
    }

    public bool CheckIsShahBlack()
    {
        
            foreach (var item in Grid.Instance.ObjFigures)
            {
                if (item != null)
                {
                    if (item.color == FigureColor.White && item.type == FigureType.King)
                    {
                        if (Grid.Instance.AnalysisCells[item.coords.x, item.coords.y])
                        {
                            if (item.availableMoves.Count > 0)
                            {
                                return true;
                            }
                            else
                            {
                                // gameOver = true;
                                // canvasMat.gameObject.SetActive(true);
                                // textMat.text = "Шах и мат";
                            } 
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return false;
    }

    public bool CheckIsShahWhite()
    {

        foreach (var item in Grid.Instance.ObjFigures)
        {
            if (item != null)
            {
                if (item.color == FigureColor.Black && item.type == FigureType.King)
                {
                    if (Grid.Instance.AnalysisCells[item.coords.x, item.coords.y])
                    {
                        if (item.availableMoves.Count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            // gameOver = true;
                            // canvasMat.gameObject.SetActive(true);
                            // textMat.text = "шах и мат";

                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        
        return false;
}

    public void CheckIsMatBlack()
    {
        foreach (var item in Grid.Instance.ObjFigures)
        {
            if (item != null)
            {
                if (item.color == FigureColor.Black && item.type == FigureType.King)
                {
                    if (Grid.Instance.AnalysisCells[item.coords.x, item.coords.y])
                    {
                        if (!(item.availableMoves.Count > 0))
                        {
                            gameOver = true;
                            canvasMat.gameObject.SetActive(true);
                            textMat.text = "шах и мат";
                        }

                    }
                }
            }
        }
    }
    public void CheckIsMatWhite()
    {
        foreach (var item in Grid.Instance.ObjFigures)
        {
            if (item != null)
            {
                if (item.color == FigureColor.White && item.type == FigureType.King)
                {
                    if (Grid.Instance.AnalysisCells[item.coords.x, item.coords.y])
                    {
                        if (!(item.availableMoves.Count > 0))
                        {
                            gameOver = true;
                            canvasMat.gameObject.SetActive(true);
                            textMat.text = "шах и мат";
                        }

                    }
                }
            }
        }
    }

    public class PlayerHelper
    {
        List<(int2, int2)> availableMoves = new List<(int2, int2)>();

        public List<(int2, int2)> CheckNextMove(FigureColor color)
        {
            if (color == FigureColor.White)
            {
                MiniMax(2, true, color);
            }
            else
            {
                MiniMax(2, false, color);
            }

            ChessAI.instance.GetAvailableMoves(color);
            return availableMoves;
        }

        public bool MiniMax(int depth, bool maximizingPlayer, FigureColor color)
        {
            var grid = Grid.Instance;
            var ai = ChessAI.instance;

            if (depth == 0)
            {
                if (maximizingPlayer)
                {
                    return ChessManager.instance.CheckIsShahBlack();
                }
                else
                {
                    return ChessManager.instance.CheckIsShahWhite();
                }
            }

            List<(int2, int2)> movesToAvoidCheckmate = new List<(int2, int2)>();

            if (maximizingPlayer)
            {
                var moves = ai.GetAvailableMoves(FigureColor.White);

                foreach (var move in moves)
                {
                    ai.MakeMove(move);
                    ai.SimulateMove(move.from.x, move.from.y, move.to.x, move.to.y);
                    var score = MiniMax(depth - 1, false, color);
                    ai.UnMakeMove(move);

                    if (!score)
                    {
                        if (color == grid.ObjFigures[move.from.x, move.from.y].color)
                            movesToAvoidCheckmate.Add((move.from, move.to));
                    }
                }

                availableMoves.AddRange(movesToAvoidCheckmate);

            }
            else
            {
                var moves = ai.GetAvailableMoves(FigureColor.Black);

                foreach (var move in moves)
                {
                    ai.MakeMove(move);
                    ai.SimulateMove(move.from.x, move.from.y, move.to.x, move.to.y);
                    var score = MiniMax(depth - 1, true, color);
                    ai.UnMakeMove(move);


                }

            }

            return false;
        }
    }
}

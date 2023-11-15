using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
//Рокировка
//Первым переставляется король по направлению к ладье через одно поле, а ладья переставляется через короля в другую сторону и ставится рядом.
//Ход начинается с короля.
//Нельзя делать рокировку, если королю шах. Сначала нужно защититься от шаха, потом делать что-то еще.
public class InputSystem : MonoBehaviour
{
    public Grid grid;
    private BaseFigure selectedFigure;
    public LayerMask figureLayer;
    public LayerMask cellLayer;
    public ChessManager chessManager;
    
    //ToDo написать метод, который будет обрабатывать нажатие на фигуру и выделять ее
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            chessManager = ChessManager.instance;
            var nextPlayer = chessManager.NextPlayer();
            if (chessManager.player != nextPlayer && chessManager.isAI) { return; };
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit, 1000 ,cellLayer))
            {
                OnHitCell(hit);
            }
        }
    }

    public void Castling(int kingX, int kingY, int rookX, int rookY)
    {
        var king = grid.ObjFigures[kingX, kingY] as King;
        var rook = grid.ObjFigures[rookX, rookY] as Rook;
        if (!king.firstMove || !rook.firstMove) { return;}
        Debug.Log("isFirstMove");
        if (king.color == FigureColor.White && chessManager.isShahWhite )
        {
            return;
        }
        if (king.color == FigureColor.Black && chessManager.isShahBlack )
        {
            return;
        }
        Debug.Log("isNotShah");
        if (kingX > rookX)//левая рокировка
        {
            Debug.Log("isLeft");
            for (int i = kingX - 1; i > rookX + 1; i--) //проверка на пустые клетки
            {
                if (grid.TypesOnBoard[i, kingY] != FigureType.None) { return;}
            }
            Debug.Log("NotFigureOnWay");
            if (grid.AnalysisCells[kingX - 1, kingY] || grid.AnalysisCells[kingX - 2, kingY])//проверка на шах
            {
                return;
            }
            king.availableMoves.TryAdd(new int2( kingX - 2, kingY) , null);
            rook.availableMoves.TryAdd(new int2( rookX + 3, rookY) , null);
            Debug.Log("isNotShahonWay");
            king.Move(kingX - 2, kingY);
            rook.Move(rookX + 3, rookY);
            ChessManager.instance.NextRound();
        }
        else//правая рокировка
        {
            Debug.Log("isRight");
            for (int i = kingX + 1; i < rookX - 1; i++)//проверка на пустые клетки
            {
                if (grid.TypesOnBoard[i, kingY] != FigureType.None) { return;}
            }
            Debug.Log("NotFigureOnWay");
            if (grid.AnalysisCells[kingX + 1, kingY] || grid.AnalysisCells[kingX + 2, kingY])//проверка на шах
            {
                return;
            }
            Debug.Log("isNotShahonWay");
            king.availableMoves.TryAdd(new int2( kingX + 2, kingY) , null);
            rook.availableMoves.TryAdd(new int2( rookX - 2, rookY) , null);
            king.Move(kingX + 2, kingY);
            rook.Move(rookX - 2, rookY);
            ChessManager.instance.NextRound();
        }
    }
    
    private void OnHitCell(RaycastHit hit)
    {
        var cell = hit.collider.GetComponent<CellCoord>();
        if (grid.TypesOnBoard[cell.x, cell.y] != FigureType.None)
        {
            var figure = grid.ObjFigures[cell.x, cell.y];
            if (figure.color == chessManager.NextPlayer())
            {
                if (selectedFigure?.type == FigureType.King && grid.TypesOnBoard[cell.x, cell.y] == FigureType.Rook)
                {
                    Castling(selectedFigure.coords.x, selectedFigure.coords.y, cell.x, cell.y);
                    selectedFigure = null;
                }
                else if (figure != selectedFigure)
                {
                    
                    selectedFigure?.HideHints();
                    figure.Select();
                    selectedFigure = figure;
                }
                
            }
            else if (selectedFigure != null)
            {
                if (grid.TypesOnBoard[cell.x,cell.y] != FigureType.King)
                {
                    if (selectedFigure.Move(cell.x, cell.y))
                    {
                        selectedFigure.HideHints();
                        selectedFigure = null;
                    }
                }
            }
        }
        else if (selectedFigure != null)
        {
            if (grid.TypesOnBoard[cell.x,cell.y] != FigureType.King)
            {
                if (selectedFigure.Move(cell.x, cell.y))
                {
                    selectedFigure.HideHints();
                    selectedFigure = null;
                }
            }
        }

    }
    //ToDo написать метод, который будет обрабатывать нажатие на клетку и перемещать фигуру на нее
    
    //ToDo написать метод, который будет обрабатывать нажатие на клетку и атаковать фигуру на ней
}

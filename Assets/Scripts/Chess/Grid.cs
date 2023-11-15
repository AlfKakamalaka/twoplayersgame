using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

//Есть путаницца из-за большого количевства массивов и списков
//Из-за того, что есть путанница, мне сложно взаимодействовать с ними, не могу определиться, какой из них использовать, чтобы реализовать методы
//Например тоже передвижения, я не могу сформулировать у себя в голове код из-за того, что я путаюсь

//Путаницца из-за математики.

//Путаницца из-за большого количевства if-ов в методе InputSystem

//Вообщем это впервые я работаю с таким кодом, где много циклов, массивов, списков еще и взаимодействия с абстрактным классом и т.д.

public class Grid : MonoBehaviour// Класс, который отвечает за работу сетки
{
   public FigureType[,] TypesOnBoard;// для проверок, хранит в себе только типы фигур
   
   public BaseFigure[,] ObjFigures;// для перемещения фигур, хранит в себе сами фигуры
   
   public CellCoord[,] Cells;
   
   public bool[,] AnalysisCells;// для анализа клеток
   
   public Transform figuresParent;//начало сетки
   public static Grid Instance;
   public float cellSize;
   
   
   public List<FigureType> allFigures;// для растановки фигур в коде
   public BaseFigure[] figurePrefabs;// для растановки фигур на сцене
   public CellCoord cellPrefab;

   private void Awake()
   {
      Instance = this;
   }

   public void Start()
   {
      
      TypesOnBoard = new FigureType[8,8];
      ObjFigures = new BaseFigure[8,8];
      AnalysisCells = new bool[8,8];
      Cells = new CellCoord[8,8];
      SetFigure();
      CreateCells();
   }
   public bool IsOnBoard(int x, int y)
   {
      if (x < 0 || x > 7 || y < 0 || y > 7)
      {
         return false;
      }
      return true;
   }
   public void CreateCells()
   {
      for (int i = 0; i < 8; i++)
      {
         for (int j = 0; j < 8; j++)
         {
            Vector3 spawnPos = new Vector3(i * cellSize, 0.011f, j * cellSize);// создаем отступы между клетками
            spawnPos += new Vector3(cellSize, 0, cellSize) / 2f;// создаем отступы от границы
            
            var cell = Instantiate(cellPrefab,spawnPos + figuresParent.position,Quaternion.identity,figuresParent);
            cell.x = i;
            cell.y = j;
            Cells[i, j] = cell;
         }
      }
   }
   //ToDo написать метод, который будет раставлять фигуры на доске в правильном стартовом порядке и выставит их на сцене
   public void SetFigure()
   {
      for (int i = 0; i < 8; i++)
      {
         TypesOnBoard[i, 0] = allFigures[i];
         SpawnFigure(allFigures[i],FigureColor.White,i,0);//первый ряд белых фигур
         TypesOnBoard[i, 1] = FigureType.Pawn;
         SpawnFigure(FigureType.Pawn,FigureColor.White,i,1);
         
         TypesOnBoard[i, 6] = FigureType.Pawn;
         SpawnFigure(FigureType.Pawn,FigureColor.Black,i,6);
         TypesOnBoard[i, 7] = allFigures[i];
         SpawnFigure(allFigures[i],FigureColor.Black,i,7);
      }
      ChessManager.instance.RequstAnalysis();
   }

   public void SpawnFigure(FigureType type,FigureColor color, int x,int y)
   {
      var figure = Instantiate(figurePrefabs[(int)type - 1],GetWorldPosition(x,y),Quaternion.identity,figuresParent );
      figure.coords = new int2(x,y);
      figure.color = color;
      figure.type = type;
      ObjFigures[x, y] = figure;
      TypesOnBoard[x, y] = type;
      figure.Init();
      if (type == FigureType.King)
      {
         if (color == FigureColor.White)
         {
            ChessManager.instance.WhiteKing = figure as King;
         }
         else
         {
            ChessManager.instance.BlackKing = figure as King;
         }
      }
   }
   public Vector3 GetWorldPosition(int x, int y)
   {
      Vector3 spawnPos = new Vector3(x * cellSize, 0, y * cellSize);
      spawnPos += new Vector3(cellSize, 0, cellSize) / 2f;
      return spawnPos + figuresParent.position;
   }
   public void MoveFigure(int x, int y, int newX, int newY)
   {
      if (TypesOnBoard[x, y] == FigureType.Pawn)
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

      if (ObjFigures[newX, newY] != null)
      {
         ObjFigures[newX, newY].Die();
      }
      TypesOnBoard[newX, newY] = TypesOnBoard[x, y];
      TypesOnBoard[x, y] = FigureType.None;

      
      
      ObjFigures[newX, newY] = ObjFigures[x, y];
      ObjFigures[x, y] = null;
      ObjFigures[newX, newY].coords = new int2(newX, newY);
      ChessManager.instance.NextRound();

   }

   public void PawnTransformation(int x, int y, int newX, int newY,FigureColor color)
   {
      if (ObjFigures[newX, newY] != null)
      {
         ObjFigures[newX, newY].Die();
      }

      ObjFigures[x, y].Die();
      ObjFigures[x, y] = null;
      TypesOnBoard[x, y] = FigureType.None;
      ChooseFigurePopup.instance.ShowPopup(newX, newY, color);
      ChessManager.instance.NextRound();
   }
}

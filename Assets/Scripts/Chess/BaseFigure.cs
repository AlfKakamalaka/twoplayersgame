using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
public enum FigureType
{
    None,
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King
}
public enum FigureColor
{
    White,
    Black
}
public abstract class BaseFigure : MonoBehaviour
{
    public int2 coords;
    public FigureType type;
    public FigureColor color;
    public Dictionary<int2,BaseFigure> availableMoves = new Dictionary<int2, BaseFigure>();
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material whiteMaterial;
    [SerializeField] private Material blackMaterial;
    public abstract void AnalyseMove();
    
    public virtual void FillAnalysisGrid(bool[,] analysisGrid)
    {
        foreach (var move in availableMoves)
        {
            analysisGrid[move.Key.x, move.Key.y] = true;
        }
    }
    
    public bool CheckNonRetraceMove(int x, int y)
    {

        var chessManager = ChessManager.instance;
        if (color == FigureColor.White)
        {
            if (chessManager.isShahWhite)
            {
                bool any = true;
                foreach (var ret in chessManager.WhiteKing.retraceShah)
                {
                    if (ret.x == x && ret.y == y)
                    {
                        Debug.Log("Retrace" + x + " " + y);
                        any= false;
                    }
                    
                }

                return any;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (chessManager.isShahBlack)
            {
                bool any = true;
                foreach (var ret in chessManager.BlackKing.retraceShah)
                {
                    if (ret.x == x && ret.y == y)
                    {
                        Debug.Log("Retrace" + x + " " + y);
                        any = false;
                    }
                    
                }

                return any;
            }
            else
            {
                return false;
            }
        }
        Debug.Log("Nothing");
        return false;
        
    }
    
    public void Init()
    {
        if(color == FigureColor.Black)
        {
            transform.Rotate(0,180,0);
            meshRenderer.material = blackMaterial;
        }
        else
        {
            meshRenderer.material = whiteMaterial;
        }
    }
    public virtual void Select()
    {
        ShowHints();
    }

    public virtual bool Move(int x, int y)
    {
        
        if(availableMoves.ContainsKey(new int2(x,y)))
        {
            transform.DOJump(Grid.Instance.GetWorldPosition(x,y),1,1,0.5f)
                .OnComplete( ()=> {
                    Grid.Instance.MoveFigure(coords.x, coords.y, x, y);
                });
           
            
            return true;
        }
        Debug.Log("No-Move");
        return false;
    }

    public void ShowHints()
    {
        foreach (var move in availableMoves)
        {
            Grid.Instance.Cells[move.Key.x, move.Key.y].Show();
        }
    }
    public void HideHints()
    {
        foreach (var move in availableMoves)
        {
            Grid.Instance.Cells[move.Key.x, move.Key.y].Hide();
        }
    }
    public virtual void Die()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOJump(transform.position,5,1,0.5f).SetEase(Ease.InSine));
        sequence.Insert(0.25f,transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InSine));
        sequence.OnComplete(() => {
            Destroy(gameObject);
        });
    }
}

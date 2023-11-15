using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFigurePopup : MonoBehaviour
{
    public static ChooseFigurePopup instance;
    public int x;
    public int y;
    private FigureColor color;
    public GameObject popup;

    private void Awake()
    {
        instance = this;
    }

    public void ShowPopup(int x, int y, FigureColor color)
    {
        this.x = x;
        this.y = y;
        this.color = color;
        popup.SetActive(true);
    }
    
    public void ChooseFigure(int type)
    {
        Debug.Log("not working");
        Grid.Instance.SpawnFigure((FigureType)(type + 1),color, x, y);
        popup.SetActive(false);
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform pivot;
    private void Awake()
    {
        instance = this;
    }

    public void RotateCamera()
    {
        if (FigureColor.Black == ChessManager.instance.NextPlayer())
        {
            pivot.DORotate(new Vector3(0,0,0), 0.5f);
        }
        else
        {
            pivot.DORotate(new Vector3(0,180,0), 0.5f);
        }
    }
}


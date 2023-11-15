using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCoord : MonoBehaviour
{
    public int x;
    public int y;
    public MeshRenderer renderer;

    public void Show()
    {
        renderer.gameObject.SetActive(true);
    }
    public void Hide()
    {
        renderer.gameObject.SetActive(false);

    }
}

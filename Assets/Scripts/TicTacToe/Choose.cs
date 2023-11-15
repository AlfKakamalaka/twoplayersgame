using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choose : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var nextPlayer = Gameplay.instance.NextPlayer();
            if (Gameplay.instance.firstPlayer != nextPlayer && GameService.isAI) { return; }
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Cell")))
            {
                if(hit.transform.TryGetComponent<Cell>(out var cell))
                {
                    if (Gameplay.instance.GetValue(cell.x,cell.y ) != (int)Value.None) { return; }
                    
                    Gameplay.instance.SetValue(cell.x, cell.y, nextPlayer);
                    
                }
            }
        }
    }
}

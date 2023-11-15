using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x;
    public int y;
    public Animator animator;
    private Value currentValue;
    [SerializeField]private GameObject prefabX;
    [SerializeField]private GameObject prefabO;
    
    private void Start()
    {
        Gameplay.instance.LinkCell(this, x, y);
    }

    public void Hide()
    {
        if (animator)
        {
            animator.SetTrigger("Hide");    
        }
        
    }
    public void Show(Value value)
    {
        if(value == Value.X && currentValue != Value.X)
        {
            animator = Instantiate(prefabX, transform).GetComponent<Animator>();
            
        }
        else if (value == Value.O && currentValue != Value.O)
        {
            animator = Instantiate(prefabO, transform).GetComponent<Animator>();
        }
        else if(value == Value.None && currentValue != Value.None)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        currentValue = value;
    }
    
}

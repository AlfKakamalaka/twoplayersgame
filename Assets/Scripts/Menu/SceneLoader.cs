using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void isAI(bool isAI)
    {
        GameService.isAI = isAI;
    }
    public void LoadScane(string sceneName)
    {
        
        Application.LoadLevel(sceneName);
    }
}

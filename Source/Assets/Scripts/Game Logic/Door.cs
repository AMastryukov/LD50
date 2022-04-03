using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
    public string SceneName;
    public bool IsUnlocked = false;

    public void GoToNextScene()
    {
        if (SceneName.Equals("")) { Debug.LogError("This door is not bound to any scene"); }
        
        if (IsUnlocked)
        {
            SceneLoader.Instance.ChangeScene(SceneName);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
    public static Action OnDoorOpened;

    public string SceneName;
    public bool IsUnlocked = false;

    public void SetExitSceneName(string sceneName)
    {
        SceneName = sceneName;
    }

    public override void Interact()
    {
        if (SceneName.Equals("")) { Debug.LogError("This door is not bound to any scene"); }
        
        if (IsUnlocked)
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}

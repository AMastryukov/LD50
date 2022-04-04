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
        if (!IsUnlocked) 
        { 
            Debug.Log("Door is locked"); 
            return; 
        }

        if (SceneName.Equals("")) 
        { 
            Debug.LogError("This door is not bound to any scene"); 
        }

        SceneLoader.Instance.ChangeScene(SceneName);
    }

    public void OpenDoorPhysically()
    {
        if (!IsUnlocked)
        {
            Debug.Log("Door is locked");
            return;
        }

        Debug.Log("Door has been opened, you are dead");

        // Animate the door opening here
    }
}

using DG.Tweening;
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
    private bool Opened = false;

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


        if (!Opened)
        {
            OnDoorOpened?.Invoke();

            Debug.Log("Door has been opened, you are dead");

            transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 120, 0), 3);
            GetComponent<Collider>().enabled = false;
            Opened = true;
        }
        
        
    }
}

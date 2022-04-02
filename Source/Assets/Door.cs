using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private int changeToSceneIndex;
    private SceneLoader sceneLoader;

    
    private void Awake()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }


    public void OnDoorInteract()
    {
        if (sceneLoader != null)
        {
            sceneLoader.ChangeScene(changeToSceneIndex);
        }
        else
            Debug.LogWarning("No scene loader on scene");
        
    }
    
}

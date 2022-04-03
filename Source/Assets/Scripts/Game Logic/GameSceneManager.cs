using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Non-persistant game scene manager that fires events telling the persistent game manager to change state
/// </summary>
public class GameSceneManager : MonoBehaviour
{
    private Door Door;

    void Start()
    {
       Door = FindObjectOfType<Door>();

        if (!Door)
        {
            Debug.LogWarning("This scene does not have a door");
        }

        Door.DoorOpened += LeaveScene;
    }

    public void LeaveScene()
    {

    }

    public void EvidenceFound(EvidenceData evidenceData)
    {

    }
}

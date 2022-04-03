using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public delegate void OpenDoorEventHandler();

    public event OpenDoorEventHandler Leave;

    public void OnDoorInteract()
    {
        Leave?.Invoke();
        
    }
    
}

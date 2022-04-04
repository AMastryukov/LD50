using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum PlayerStates
    {
        Move,
        Inspect,
        Interrogate,
        Wait
    }
    
    public PlayerStates CurrentState = PlayerStates.Move;
}

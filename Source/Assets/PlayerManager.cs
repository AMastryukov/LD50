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
    }
    
    public PlayerStates playerState = PlayerStates.Move;
    
    private void Start()
    {
        playerState = PlayerStates.Move;
    }
}

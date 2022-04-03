using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static Action<PlayerStates> PlayerStateChanged;

    public enum PlayerStates
    {
        Move,
        Inspect,
        Interrogate
    }

    private PlayerStates _playerState;

    public PlayerStates playerState { 
        get { 
            return _playerState; 
        } 
        set {
            _playerState = value;
            PlayerStateChanged?.Invoke(PlayerStates.Move);
        } 
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static Action<PlayerStates> OnPlayerStateChanged;

    public enum PlayerStates
    {
        Move,
        Inspect,
        Interrogate,
        Wait
    }

    public PlayerStates CurrentState = PlayerStates.Move;
}

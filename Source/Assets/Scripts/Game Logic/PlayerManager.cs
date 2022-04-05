using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    public static Action<PlayerStates> OnPlayerStateChanged;

    [SerializeField] private CanvasGroup deathOverlay;

    public enum PlayerStates
    {
        Move,
        Inspect,
        Interrogate,
        Wait
    }

    public PlayerStates _currentState = PlayerStates.Move;


    public PlayerStates CurrentState
    {
        get
        {
            return _currentState;
        }

        set
        {
            _currentState = value;
            OnPlayerStateChanged?.Invoke(value);
        }
    }

    public void FadeInDeathOverlay()
    {
        deathOverlay.DOFade(1f, 15f).SetEase(Ease.InQuad);
    }
}
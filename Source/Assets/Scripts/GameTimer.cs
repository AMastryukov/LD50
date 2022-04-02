using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityCode;


public class GameTimer : UnitySingletonPersistent<GameTimer>
{
    [SerializeField] private float mainTimer = 5f;
    [SerializeField] private float standardIncrement = 5f;
    private bool isRunning = false;

    #region EventAssignment

    private void AssignDelegates()
    {
        GameEventSystem.Instance.OnTimerStart += StartGameTimer;
    }

    private void UnAssignDelegates()
    {
        if (GameEventSystem.Quitting)
            return;
        GameEventSystem.Instance.OnTimerStart -= StartGameTimer;
    }

    #endregion

    #region UnityEventFunctions

    public override void Awake()
    {
        base.Awake();
        AssignDelegates();
    }
    
    private void OnDestroy()
    {
        UnAssignDelegates();
    }
    
    void Update()
    {
        if (!isRunning) return;
        mainTimer -= 1*Time.deltaTime;
        if (mainTimer <= 0)
        {
            isRunning = false;
            Debug.Log("GameOver!");
            GameEventSystem.Instance.OnTimerEnd?.Invoke();
        }
    }

    #endregion

    #region Methods

    private void StartGameTimer()
    {
        isRunning = true;
    }

    public float GetRemainingTime()
    {
        return mainTimer;
    }

    public void IncreaseGameTimer()
    {
        mainTimer += standardIncrement;
    }
    public void IncreaseGameTimer(float customIncrement)
    {
        mainTimer += customIncrement;
    }
    
    public bool HasTimerStarted()
    {
        return isRunning;
    }

    #endregion
    
}

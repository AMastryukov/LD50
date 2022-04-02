using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHud : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    
    #region EventAssignment

    private void AssignDelegates()
    {
        GameEventSystem.Instance.OnTimerStart += ShowTimer;
    }

    private void UnAssignDelegates()
    {
        if (GameEventSystem.Quitting)
            return;
        GameEventSystem.Instance.OnTimerStart -= ShowTimer;
    }
    
    #endregion

    #region UnityEventFunctions
    void Awake()
    {
        AssignDelegates();
    }

    void OnDestroy()
    {
        UnAssignDelegates();
    }

    void Start()
    {
        //InitializeTimerValue
        UpdateTimer();
    }
    
    void Update()
    {
        if(GameTimer.Instance.HasTimerStarted())
            UpdateTimer();
    }
    

    #endregion
   

    private void UpdateTimer()
    {
        timerText.text = String.Format(GameConstants.HudConstants.TimerText,
            GameTimer.Instance.GetRemainingTime().ToString());
    }

    private void ShowTimer()
    {
        timerText.gameObject.SetActive(true);
    }
}

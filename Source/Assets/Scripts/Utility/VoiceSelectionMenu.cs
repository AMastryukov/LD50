using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceSelectionMenu : MonoBehaviour
{
    public static Action OnVoiceSelected;

    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void Show()
    {
        canvas.enabled = true;
    }

    public void SelectMaleVoice()
    {
        PlayerPrefs.SetString("PlayerVoice", "MALE");
        OnVoiceSelected?.Invoke();

        canvas.enabled = false;
    }

    public void SelectFemaleVoice()
    {
        PlayerPrefs.SetString("PlayerVoice", "FEMALE");
        OnVoiceSelected?.Invoke();

        canvas.enabled = false;
    }
}

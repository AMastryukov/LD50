using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EndCredits : MonoBehaviour
{
    [SerializeField] private List<CanvasGroup> creditPanels;
    private int currentIndex = -1;

    private void Start()
    {
        RollCredits();
    }

    private void RollCredits()
    {
        currentIndex++;
        if (currentIndex >= creditPanels.Count)
        {
            //Main Menu?
            return;
        }

        creditPanels[currentIndex].DOFade(1, 4f).onComplete = () =>
        {
            creditPanels[currentIndex].DOFade(0, 4f).onComplete = RollCredits;
        };
    }
}

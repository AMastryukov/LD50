using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EndCredits : MonoBehaviour
{
    [SerializeField] private List<CanvasGroup> creditPanels;

    public IEnumerator RollCredits()
    {
        foreach(var panel in creditPanels)
        {
            panel.alpha = 1f;
            panel.transform.DOScale(Vector2.one * 1.15f, 5f);

            yield return new WaitForSeconds(2f);

            panel.DOFade(0f, 1f).SetEase(Ease.OutQuad);

            yield return new WaitForSeconds(1f);
        }
    }
}

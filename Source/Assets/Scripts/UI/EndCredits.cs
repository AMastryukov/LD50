using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EndCredits : MonoBehaviour
{
    [SerializeField] private List<CanvasGroup> creditPanels;

    private void Start()
    {
        StartCoroutine(RollCredits());
    }

    public IEnumerator RollCredits()
    {
        foreach(var panel in creditPanels)
        {
            panel.alpha = 1f;
            panel.transform.DOScale(Vector2.one * 1.15f, 5f);

            // Stop on the last panel
            if (creditPanels[creditPanels.Count - 1] == panel)
            {
                yield break;
            }

            yield return new WaitForSeconds(3f);

            panel.DOFade(0f, 0.25f).SetEase(Ease.OutQuad);

            yield return new WaitForSeconds(0.25f);
        }
    }
}

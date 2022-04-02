using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NotebookTab : MonoBehaviour
{
    [SerializeField] private CanvasGroup content;
    private const float ScaleFactor = 1.1f;
    private readonly Vector3 originalScale = new Vector3(1f,1f,1f);
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Highlight()
    {
        gameObject.GetComponent<RectTransform>().DOScale(ScaleFactor, 0.25f);
        button.interactable = false;
        Show();
    }

    public void UnHighlight()
    {
        gameObject.GetComponent<RectTransform>().DOScale(originalScale, 0.25f);
        button.interactable = true;
        Hide();
    }
    
    protected void Show()
    {
        content.alpha = 1f;
        content.interactable = true;
        content.blocksRaycasts = true;
    }

    protected void Hide()
    {
        content.alpha = 0;
        content.interactable = false;
        content.blocksRaycasts = false;
    }
}

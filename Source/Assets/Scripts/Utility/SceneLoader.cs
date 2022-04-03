using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void FadeIn(Action onComplete)
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        canvasGroup.DOFade(1f, 0.5f)
            .SetEase(Ease.OutCirc)
            .OnComplete(() =>
            {
                onComplete.Invoke();
            });
    }

    public void FadeOut()
    {
        canvasGroup.DOFade(0f, 3f)
                .SetEase(Ease.InCirc)
                .OnComplete(() =>
                {
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                });
    }

    /// <summary>
    /// Change scene with a smooth fading transition
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public void ChangeScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            FadeIn(delegate () { SceneManager.LoadScene(sceneName); });
        }
    }
}

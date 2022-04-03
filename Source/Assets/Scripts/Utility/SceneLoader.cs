using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class SceneLoader : MonoBehaviour
{
    public static Action<string> OnSceneLoaded;
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

        SceneManager.sceneLoaded += SceneLoadEvent;
        SceneManager.sceneUnloaded += SceneUnloadEvent;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoadEvent;
        SceneManager.sceneUnloaded -= SceneUnloadEvent;
    }

    /// <summary>
    /// Add any references ou want to find in the scene here since scenes will change throughout the game
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void SceneLoadEvent(Scene scene, LoadSceneMode mode)
    {
        OnSceneLoaded?.Invoke(scene.name);
    }

    /// <summary>
    /// Good idea to unreference anything set in the SceneLoadEvent
    /// </summary>
    /// <param name="scene"></param>
    private void SceneUnloadEvent(Scene scene)
    {

    }

    public void FadeOut(Action onComplete = null)
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        canvasGroup.DOFade(1f, 0.5f)
            .SetEase(Ease.OutCirc)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    public void FadeIn(Action onComplete = null)
    {
        canvasGroup.DOFade(0f, 2f)
                .SetEase(Ease.InCirc)
                .OnComplete(() =>
                {
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;

                    onComplete?.Invoke();
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
            FadeOut(delegate () { SceneManager.LoadScene(sceneName); });
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UtilityCode;

/// <summary>
/// An instance of this class is meant to exist independant of what level is loaded
/// The manager will grab the necessary referenced on sceneLoaded.
/// </summary>
public class GameManager : UnitySingletonPersistent<GameManager>
{
    private SceneLoader sceneLoader;
    private CrimeSceneManager gameSceneManager;

    public override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);

        sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader == null)
        {
            Debug.LogError("Scene loader missing from scene");
        }

        SceneManager.sceneLoaded += SceneLoadEvent;
        SceneManager.sceneUnloaded += SceneUnloadEvent;

    }

    /// <summary>
    /// Add any references ou want to find in the scene here since scenes will change throughout the game
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void SceneLoadEvent(Scene scene, LoadSceneMode mode)
    {
        gameSceneManager = FindObjectOfType<CrimeSceneManager>();

        if (gameSceneManager == null)
        {
            Debug.LogWarning("This scene is missing a GameSceneManager");
        }
    }

    /// <summary>
    /// Good idea to unreference anything set in the SceneLoadEvent
    /// </summary>
    /// <param name="scene"></param>
    private void SceneUnloadEvent(Scene scene)
    {

    }

    private void Start()
    {
        StartCoroutine(GameSequence());
    }

    private IEnumerator GameSequence()
    {
        yield return null;
    }

    private IEnumerator IntroSequence()
    {
        yield return null;
    }

    private IEnumerator Sequence1()
    {
        yield return null;
    }

    private IEnumerator CrimeScene1()
    {
        yield return null;
    }

    private IEnumerator Interrogation1()
    {
        yield return null;
    }

    private IEnumerator GameWinSequence()
    {
        yield return null;
    }
}

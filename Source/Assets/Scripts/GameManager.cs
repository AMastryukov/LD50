using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// An instance of this class is meant to exist independant of what level is loaded
/// 
/// The manager will grab the necessary referenced on sceneLoaded.
/// </summary>
public class GameManager : MonoBehaviour
{
    private SceneLoader SL;
    private Door Door;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        SL = FindObjectOfType<SceneLoader>();
        if (SL == null)
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
        this.Door = FindObjectOfType<Door>();

        if (!this.Door)
        {
            Debug.LogWarning("This scene does not have a door");
        }

    }

    /// <summary>
    /// Good idea to unreference anything set in the SceneLoadEvent
    /// </summary>
    /// <param name="scene"></param>
    private void SceneUnloadEvent(Scene scene)
    {
        this.Door = null;
    }

    private void Start()
    {
        StartCoroutine(GameSequence());
    }

    private IEnumerator GameSequence()
    {
        Debug.Log("GameSequence");
        yield return IntroSequence();

        yield return Sequence1();
        //yield return Sequence2();
        //yield return Sequence3();

        yield return GameWinSequence();

        yield return null;
    }

    private IEnumerator IntroSequence()
    {
        Debug.Log("IntroSequence");
        yield return null;
    }

    private IEnumerator Sequence1()
    {
        HashSet<EvidenceKey> evidenceFound = new HashSet<EvidenceKey>();
        Debug.Log("Sequence1");
        //TODO: Add event delegate for evidence being found

        while (evidenceFound.Count < 3)
        {
            yield return CrimeScene1();
            yield return Interrogation1();
        }

        yield return null;
    }

    private IEnumerator CrimeScene1()
    {
        yield return SL.ChangeScene("CrimeScene1");

        Debug.Log("Loaded CrimeScene1");

        bool leaveScene = false;

        if (!Door)
        {
            Debug.LogError("Door not found in scene");
        }

        Door.Leave += () => leaveScene = true;

        while (!leaveScene)
        {
            yield return null;
        }

        yield return null;
    }

    private IEnumerator Interrogation1()
    {
        yield return SL.ChangeScene("Interrogation1");
        Debug.Log("Interrogation1");
        //SceneManager.LoadScene("Interrogation1");

        bool leaveInterrogation = false;
        //TODO: Add event delegate for leave event

        while (!leaveInterrogation)
        {
            yield return null;
        }

        yield return null;
    }

    private IEnumerator GameWinSequence()
    {
        yield return null;
    }


}

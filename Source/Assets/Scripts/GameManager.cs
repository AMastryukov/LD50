using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UtilityCode;

/// <summary>
/// An instance of this class is meant to exist independant of what level is loaded
/// 
/// The manager will grab the necessary referenced on sceneLoaded.
/// </summary>
public class GameManager : UnitySingletonPersistent<GameManager>
{
    private SceneLoader SL;
    private GameSceneManager Manager;

    public override void Awake()
    {
        base.Awake();

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
        
        this.Manager = FindObjectOfType<GameSceneManager>();

        if (this.Manager == null)
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
            yield return Interrogation1(evidenceFound);
        }

        yield return null;
    }

    private IEnumerator CrimeScene1()
    {
        // Wait for scene to load
        yield return SL.ChangeScene("CrimeScene1");
        Debug.Log("Loaded CrimeScene1");

        bool sceneLeft = false;
        Manager.SceneLeft += () => sceneLeft = true;

        while (!sceneLeft)
        {
            yield return null;
        }

        yield return null;
    }

    private IEnumerator Interrogation1(HashSet<EvidenceKey> evidenceFound)
    {
        // Wait for scene to load
        yield return SL.ChangeScene("Interrogation1");
        Debug.Log("Interrogation1");

        bool sceneLeft = false;
        Manager.SceneLeft += () => sceneLeft = true;
        Manager.FoundEvidence += (EvidenceKey evidence) => evidenceFound.Add(evidence);

        while (!sceneLeft)
        {
            yield return null;
        }

        yield return null;
    }

    private IEnumerator GameWinSequence()
    {
        Debug.Log("GameWinSequence");
        yield return null;
    }


}

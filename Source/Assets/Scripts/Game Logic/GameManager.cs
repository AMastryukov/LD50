using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UtilityCode;
using System;

/// <summary>
/// An instance of this class is meant to exist independant of what level is loaded
/// The manager will grab the necessary referenced on sceneLoaded.
/// </summary>
public class GameManager : UnitySingletonPersistent<GameManager>
{
    private SceneLoader sceneLoader;
    private CrimeSceneManager crimeSceneManager;

    public override void Awake()
    {
        base.Awake();

        sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader == null)
        {
            Debug.LogError("SceneLoader missing from scene");
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
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return IntroSequence();
        yield return AlleywayCrimeSequence();
        yield return InterrogationUptonSequence();
        yield return PreGarageSequence();
        yield return GarageSequence();
        yield return InterrogationLucaSequence();
        yield return PreApartmentSequence();
        yield return ApartmentSearchSequence();
        yield return AlleywayBennySequence();
        yield return ApartmentFinalSequence();
        yield return GameEndSequence();
        yield return CreditsSequence();
        yield return PostCreditSequence();
    }

    private IEnumerator IntroSequence()
    {
        bool talking = false;

        // Subscribe to events

        while (talking)
        {

            // Unsubscribe from events
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        sceneLoader.FadeOut();
    }

    private IEnumerator AlleywayCrimeSequence()
    {
        bool collectedAllEvidence = false;

        // Subscribe to events
        Action onCollectAllEvidence = delegate () { collectedAllEvidence = true; };
        CrimeSceneManager.OnAllEvidenceFound += onCollectAllEvidence;

        while (!collectedAllEvidence)
        {
            yield return null;
        }

        // Unsubscribe from events
        CrimeSceneManager.OnAllEvidenceFound -= onCollectAllEvidence;

        FindObjectOfType<Door>().SceneName = "Interrogation Room";
        FindObjectOfType<Door>().IsUnlocked = true;

        Debug.Log("Door unlocked! Interrogate Upton.");
    }

    private IEnumerator InterrogationUptonSequence()
    {
        yield return null;
    }

    private IEnumerator PreGarageSequence()
    {
        yield return null;
    }

    private IEnumerator GarageSequence()
    {
        yield return null;
    }

    private IEnumerator InterrogationLucaSequence()
    {
        yield return null;
    }

    private IEnumerator PreApartmentSequence()
    {
        yield return null;
    }

    private IEnumerator ApartmentSearchSequence()
    {
        yield return null;
    }

    private IEnumerator AlleywayBennySequence()
    {
        yield return null;
    }

    private IEnumerator ApartmentFinalSequence()
    {
        yield return null;
    }

    private IEnumerator GameEndSequence()
    {
        yield return null;
    }

    private IEnumerator CreditsSequence()
    {
        yield return null;
    }

    private IEnumerator PostCreditSequence()
    {
        yield return null;
    }
}

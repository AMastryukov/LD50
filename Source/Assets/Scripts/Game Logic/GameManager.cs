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
    private InterrogationManager interrogationManager;
    private PlayerManager playerManager;
    private Door door;

    public override void Awake()
    {
        base.Awake();

        sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader == null)
        {
            Debug.LogError("SceneLoader missing from scene");
        }
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
        Debug.Log("[SCENE] Intro sequence");

        playerManager = FindObjectOfType<PlayerManager>();
        playerManager.CurrentState = PlayerManager.PlayerStates.Wait;

        //yield return new WaitForSeconds(1f);
        //sceneLoader.FadeIn();

        bool talking = false;

        while (talking)
        {
            yield return null;
        }

        //sceneLoader.FadeOut();
    }

    private IEnumerator AlleywayCrimeSequence()
    {
        Debug.Log("[SCENE] Alleyway 1 sequence");

        FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;

        door = FindObjectOfType<Door>();
        door.IsUnlocked = false;
        sceneLoader.FadeIn(() => { FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move; });

        #region Wait for Evidence Collection
        bool collectedAllEvidence = false;
        Action onCollectAllEvidence = delegate () { collectedAllEvidence = true; };

        CrimeSceneManager.OnAllEvidenceFound += onCollectAllEvidence;

        while (!collectedAllEvidence)
        {
            yield return null;
        }

        CrimeSceneManager.OnAllEvidenceFound -= onCollectAllEvidence;
        #endregion

        #region Unlock Door
        door.SceneName = "Interrogation Room";
        door.IsUnlocked = true;
        #endregion

        #region Wait for Leaving Scene
        bool hasLeftScene = false;
        Action<string> onLeftScene = delegate (string scene) { hasLeftScene = true;  };

        SceneLoader.OnSceneLoaded += onLeftScene;

        while (!hasLeftScene)
        {
            yield return null;
        }

        SceneLoader.OnSceneLoaded -= onLeftScene;
        #endregion
    }

    private IEnumerator InterrogationUptonSequence()
    {
        Debug.Log("[SCENE] Upton interrogation sequence");

        door = FindObjectOfType<Door>();
        door.IsUnlocked = false;
        sceneLoader.FadeIn(() => { FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move; });

        interrogationManager = FindObjectOfType<InterrogationManager>();
        interrogationManager.SetCurrentSuspect(DataManager.Instance.GetSuspectDataFromKey("Upton O'Goode"));

        #region Wait for Confession
        bool hasConfessed = false;
        Action<Suspect> onConfessed = delegate (Suspect suspect) 
        { 
            if (suspect.Data.Name.Equals("Upton O'Goode"))
            {
                hasConfessed = true;
            }
        };

        Suspect.OnConfess += onConfessed;

        while (!hasConfessed)
        {
            yield return null;
        }

        Suspect.OnConfess -= onConfessed;
        #endregion

        #region Unlock Door
        door.SceneName = "Garage";
        door.IsUnlocked = true;
        #endregion

        #region Wait for Leaving Scene
        bool hasLeftScene = false;
        Action<string> onLeftScene = delegate (string scene) { hasLeftScene = true; };

        SceneLoader.OnSceneLoaded += onLeftScene;

        while (!hasLeftScene)
        {
            yield return null;
        }

        SceneLoader.OnSceneLoaded -= onLeftScene;
        #endregion
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

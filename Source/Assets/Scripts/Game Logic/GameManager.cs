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
        yield return ChooseVoiceSequence();
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

    private IEnumerator ChooseVoiceSequence()
    {
        FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;

        #region Lock Door
        door = FindObjectOfType<Door>();
        door.IsUnlocked = false;
        #endregion

        yield return new WaitForSeconds(1f);

        FindObjectOfType<VoiceSelectionMenu>().Show();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        #region Wait for Voice Selection
        bool playerChoseVoice = false;
        Action onPlayerChoseVoice = delegate () { playerChoseVoice = true; };

        VoiceSelectionMenu.OnVoiceSelected += onPlayerChoseVoice;

        while (!playerChoseVoice)
        {
            yield return null;
        }

        VoiceSelectionMenu.OnVoiceSelected -= onPlayerChoseVoice;
        #endregion

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private IEnumerator IntroSequence()
    {
        Debug.Log("[SCENE] Intro sequence");

        DataManager.Instance.NotebookEvidence.Clear();
        FindObjectOfType<Notebook>().AddSuspect(DataManager.Instance.GetSuspectDataFromKey("Rico Shade"));

        yield return new WaitForSeconds(2f);

        var playerVoice = FindObjectOfType<PlayerVoice>();
        //yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-pickup"));
        //yield return playerVoice.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("CHIEF_PHONE_INTRO_CRIMESCENE"));
        //yield return playerVoice.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("PLAYER_CHIEF_PHONE_INTRO_RESPONSE"));
        //yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-hangup"));

        yield return null;
    }

    private IEnumerator AlleywayCrimeSequence()
    {
        Debug.Log("[SCENE] Alleyway 1 sequence");

        #region Lock Door & Fade In
        sceneLoader.FadeIn(() => { FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move; });
        #endregion

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

        yield return new WaitForSeconds(2f);

        var playerVoice = FindObjectOfType<PlayerVoice>(); 
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-pickup"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("CHIEF_PHONE_DETAIN_UPTON"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-hangup"));

        FindObjectOfType<Notebook>().AddSuspect(DataManager.Instance.GetSuspectDataFromKey("Upton O'Goode"));

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

        interrogationManager = FindObjectOfType<InterrogationManager>();

        interrogationManager.SetCurrentSuspect("Upton O'Goode");

        #region Lock Door & Fade In
        door = FindObjectOfType<Door>();
        door.IsUnlocked = false;
        sceneLoader.FadeIn(() => { FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move; });
        #endregion

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
        Debug.Log("[SCENE] Pre Garage sequence");

        // TODO: Clear Notebook.Evidence

        FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;

        #region Lock Door 
        door = FindObjectOfType<Door>();
        door.IsUnlocked = false;
        #endregion

        yield return new WaitForSeconds(2f);

        var playerVoice = FindObjectOfType<PlayerVoice>();
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-pickup"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("CHIEF_PHONE_PRE_GARAGE_WARNING"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-hangup"));
    }

    private IEnumerator GarageSequence()
    {
        Debug.Log("[SCENE] Garage sequence");

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

        yield return new WaitForSeconds(2f);

        var playerVoice = FindObjectOfType<PlayerVoice>();
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-pickup"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("CHIEF_PHONE_DETAIN_LUCA"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-hangup"));

        FindObjectOfType<Notebook>().AddSuspect(DataManager.Instance.GetSuspectDataFromKey("Luca Verdere"));

        #region Unlock Door
        door.SceneName = "Interrogation Room";
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

    private IEnumerator InterrogationLucaSequence()
    {
        Debug.Log("[SCENE] Luca interrogation sequence");

        interrogationManager = FindObjectOfType<InterrogationManager>();
        interrogationManager.SetCurrentSuspect("Luca Verdere");

        #region Lock Door & Fade In
        door = FindObjectOfType<Door>();
        door.IsUnlocked = false;
        sceneLoader.FadeIn(() => { FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move; });
        #endregion

        #region Wait for Confession
        bool hasConfessed = false;
        Action<Suspect> onConfessed = delegate (Suspect suspect)
        {
            if (suspect.Data.Name.Equals("Luca Verdere"))
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
        door.SceneName = "Apartment 1";
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

    private IEnumerator PreApartmentSequence()
    {
        Debug.Log("[SCENE] Pre Apartment sequence");

        // TODO: Clear Notebook.Evidence

        FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;

        #region Lock Door 
        door = FindObjectOfType<Door>();
        door.IsUnlocked = false;
        #endregion

        yield return new WaitForSeconds(2f);

        var playerVoice = FindObjectOfType<PlayerVoice>();
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-pickup"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("BENNY_PHONE_FIRST_CONTACT"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-hangup"));

        yield return new WaitForSeconds(1f);

        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-pickup"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("CHIEF_PHONE_APARTMENT_KEEP_QUIET"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-hangup"));

        yield return null;
    }

    private IEnumerator ApartmentSearchSequence()
    {
        Debug.Log("[SCENE] Apartment 1 sequence");

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
        door.SceneName = "Alleyway 2";
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

    private IEnumerator AlleywayBennySequence()
    {
        Debug.Log("[SCENE] Benny interrogation sequence");

        interrogationManager = FindObjectOfType<InterrogationManager>();
        interrogationManager.SetCurrentSuspect("Benny Factor");

        #region Lock Door & Fade In
        door = FindObjectOfType<Door>();
        door.IsUnlocked = false;
        sceneLoader.FadeIn(() => { FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move; });
        #endregion

        #region Wait for Confession
        bool hasConfessed = false;
        Action<Suspect> onConfessed = delegate (Suspect suspect)
        {
            if (suspect.Data.Name.Equals("Benny Factor"))
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

        FindObjectOfType<Notebook>().AddSuspect(DataManager.Instance.GetSuspectDataFromKey("Benny Factor"));

        #region Unlock Door
        door.SceneName = "Apartment 2";
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

    private IEnumerator ApartmentFinalSequence()
    {
        // TODO: Clear Notebook.Evidence

        sceneLoader.FadeIn(() => { FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move; });

        #region Wait for Photo Inspection
        bool hasInspectedPhoto = false;
        Action<Evidence> onInspected = delegate (Evidence evidence)
        {
            if (evidence.evidenceData.Name.Equals("Photo of Benny, Luca, Rico and Badge"))
            {
                hasInspectedPhoto = true;
            }
        };

        Evidence.OnInspect += onInspected;

        while (false)
        {
            yield return null;
        }

        Evidence.OnInspect -= onInspected;
        #endregion

        yield return new WaitForSeconds(2f);

        var playerVoice = FindObjectOfType<PlayerVoice>();
        yield return playerVoice.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey($"PLAYER_REALIZATION"));

        yield return new WaitForSeconds(1f);

        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-pickup"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("CHIEF_PHONE_BAIT"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-hangup"));

        #region Unlock Door
        door = FindObjectOfType<Door>();
        door.IsUnlocked = true;
        #endregion

        #region Wait for Open Door
        bool isPlayerDead = false;
        Action onDoorOpened = delegate () { isPlayerDead = true; };

        Door.OnDoorOpened += onDoorOpened;

        while (!isPlayerDead)
        {
            yield return null;
        }

        Door.OnDoorOpened -= onDoorOpened;
        #endregion
    }

    private IEnumerator GameEndSequence()
    {
        FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;

        // Animate the camera, etc.

        yield return null;
    }

    private IEnumerator CreditsSequence()
    {
        // Yield credits

        yield return null;
    }

    private IEnumerator PostCreditSequence()
    {
        // Position player in a specific way
        // sceneLoader.FadeIn();

        // wait 5 seconds

        yield return null;

        sceneLoader.ChangeScene("MainMenu");
    }
}

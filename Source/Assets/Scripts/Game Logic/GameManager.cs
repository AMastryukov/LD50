using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UtilityCode;
using System;
using DG.Tweening;

/// <summary>
/// An instance of this class is meant to exist independant of what level is loaded
/// The manager will grab the necessary referenced on sceneLoaded.
/// </summary>
public class GameManager : UnitySingletonPersistent<GameManager>
{
    private SceneLoader sceneLoader;
    private InterrogationManager interrogationManager;
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

        FindObjectOfType<Notebook>().ClearEvidence();
        DataManager.Instance.NotebookEvidence.Clear();

        FindObjectOfType<Notebook>().AddSuspect(DataManager.Instance.GetSuspectDataFromKey("Rico Shade"));

        yield return new WaitForSeconds(2f);

        var playerVoice = FindObjectOfType<PlayerVoice>();
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-pickup"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("CHIEF_PHONE_INTRO_CRIMESCENE"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("PLAYER_CHIEF_PHONE_INTRO_RESPONSE"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-hangup"));
    }

    private IEnumerator AlleywayCrimeSequence()
    {
        Debug.Log("[SCENE] Alleyway 1 sequence");

        #region Lock Door & Fade In
        FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;
        sceneLoader.FadeIn(() => { FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move; });
        AudioManager.Instance.FadeInMusic(AudioManager.Instance.alleyway1Theme);
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
        door = FindObjectOfType<Door>();
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

        AudioManager.Instance.FadeOutMusic();
    }

    private IEnumerator InterrogationUptonSequence()
    {
        Debug.Log("[SCENE] Upton interrogation sequence");

        interrogationManager = FindObjectOfType<InterrogationManager>();

        interrogationManager.SetCurrentSuspect("Upton O'Goode");

        #region Lock Door & Fade In
        door = FindObjectOfType<Door>();
        door.IsUnlocked = false;
        FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;
        sceneLoader.FadeIn(() => { FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move; });
        AudioManager.Instance.FadeInMusic(AudioManager.Instance.interrogationRoomTheme);
        #endregion 
        
        yield return interrogationManager.CurrentSuspect.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("UPTON_INTRODUCTION"));

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

        AudioManager.Instance.FadeOutMusic();
    }

    private IEnumerator PreGarageSequence()
    {
        Debug.Log("[SCENE] Pre Garage sequence");

        FindObjectOfType<Notebook>().ClearEvidence();
        DataManager.Instance.NotebookEvidence.Clear();

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

        FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;
        sceneLoader.FadeIn(() => { FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move; });
        AudioManager.Instance.FadeInMusic(AudioManager.Instance.chopShopTheme);

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
        door = FindObjectOfType<Door>();
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

        AudioManager.Instance.FadeOutMusic();
    }

    private IEnumerator InterrogationLucaSequence()
    {
        Debug.Log("[SCENE] Luca interrogation sequence");

        interrogationManager = FindObjectOfType<InterrogationManager>();
        interrogationManager.SetCurrentSuspect("Luca Verdere");

        #region Lock Door & Fade In
        door = FindObjectOfType<Door>();
        door.IsUnlocked = false;
        FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;
        sceneLoader.FadeIn(() => { FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move; });
        AudioManager.Instance.FadeInMusic(AudioManager.Instance.interrogationRoomTheme);
        #endregion

        yield return interrogationManager.CurrentSuspect.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("LUCA_INTRODUCTION"));

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

        AudioManager.Instance.FadeOutMusic();
    }

    private IEnumerator PreApartmentSequence()
    {
        Debug.Log("[SCENE] Pre Apartment sequence");

        FindObjectOfType<Notebook>().ClearEvidence();
        DataManager.Instance.NotebookEvidence.Clear();

        FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;

        #region Lock Door 
        door = FindObjectOfType<Door>();
        door.IsUnlocked = false;
        #endregion

        yield return new WaitForSeconds(2f);

        var playerVoice = FindObjectOfType<PlayerVoice>();
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-pickup"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("CHIEF_PHONE_APARTMENT_KEEP_QUIET"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-hangup"));

        yield return new WaitForSeconds(1f);

        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-pickup"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("BENNY_PHONE_FIRST_CONTACT"));
        yield return playerVoice.PlayAudio(DataManager.Instance.GetSoundEffect("phone-hangup"));
    }

    private IEnumerator ApartmentSearchSequence()
    {
        Debug.Log("[SCENE] Apartment 1 sequence");

        FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;
        sceneLoader.FadeIn(() => { FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move; });
        AudioManager.Instance.FadeInMusic(AudioManager.Instance.victimApartment1Theme);

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
        door = FindObjectOfType<Door>();
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

        AudioManager.Instance.FadeOutMusic();
    }

    private IEnumerator AlleywayBennySequence()
    {
        Debug.Log("[SCENE] Benny interrogation sequence");

        interrogationManager = FindObjectOfType<InterrogationManager>();
        interrogationManager.SetCurrentSuspect("Benny Factor");

        #region Lock Door & Fade In
        door = FindObjectOfType<Door>();
        door.IsUnlocked = false;
        FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;
        sceneLoader.FadeIn(() => { FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move; });
        AudioManager.Instance.FadeInMusic(AudioManager.Instance.alleyway2Theme);
        #endregion

        yield return new WaitForSeconds(2f);
        yield return interrogationManager.CurrentSuspect.PlayAudio(DataManager.Instance.GetVoiceLineDataFromKey("BENNY_INTRODUCTION"));

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

        AudioManager.Instance.FadeOutMusic();
    }

    private IEnumerator ApartmentFinalSequence()
    {
        FindObjectOfType<Notebook>().ClearEvidence();
        DataManager.Instance.NotebookEvidence.Clear();

        FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;
        sceneLoader.FadeIn(() => { FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move; });
        AudioManager.Instance.FadeInMusic(AudioManager.Instance.betrayalTheme);

        #region Wait for Photo Inspection
        bool hasInspectedPhoto = false;
        Action<Evidence> onInspected = delegate (Evidence evidence)
        {
            if (evidence.evidenceData.Name.Equals("Group Photo"))
            {
                hasInspectedPhoto = true;
            }
        };

        Evidence.OnInspect += onInspected;

        while (!hasInspectedPhoto)
        {
            yield return null;
        }

        Evidence.OnInspect -= onInspected;
        #endregion

        yield return new WaitForSeconds(1f);

        AudioManager.Instance.FadeInMusic(AudioManager.Instance.victimApartment2Theme);

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
        bool hasPlayedOpenedDoor = false;
        Action onDoorOpened = delegate () { hasPlayedOpenedDoor = true; };

        Door.OnDoorOpened += onDoorOpened;

        while (!hasPlayedOpenedDoor)
        {
            yield return null;
        }

        Door.OnDoorOpened -= onDoorOpened;
        #endregion
    }

    private IEnumerator GameEndSequence()
    {
        AudioManager.Instance.FadeOutMusic();

        // Rotate the Virtual camera into position
        var vCamController = FindObjectOfType<PlayerVCamController>();
        vCamController.transform.DORotate(new Vector3(0, 90, 0), 1f).SetEase(Ease.OutQuad);
        vCamController.enabled = false;

        GameObject.FindGameObjectWithTag("deathCam").transform.DORotate(new Vector3(0, 90, 0), 1f).SetEase(Ease.OutQuad);

        // Disable the [E] on screen
        FindObjectOfType<PlayerInteractor>().ResetInteractionUI();

        var playerMovementController = FindObjectOfType<PlayerMovementController>();
        playerMovementController.transform.DOMove(new Vector3(-0.5f, 1, -1), 1f).SetEase(Ease.OutQuad);
        playerMovementController.transform.DORotate(new Vector3(0, 90, 0), 1f).SetEase(Ease.OutQuad);

        FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;

        yield return new WaitForSeconds(0.35f);

        yield return FindObjectOfType<CopShooting>().RaiseWeapon();

        yield return new WaitForSeconds(1.5f);

        yield return FindObjectOfType<CopShooting>().ShootGun();

        yield return new WaitForSeconds(2);

        AudioManager.Instance.FadeInMusic(AudioManager.Instance.endTheme);
        FindObjectOfType<PlayerManager>().FadeInDeathOverlay();

        // Kind of dumb but this is perfect timing for the song
        yield return new WaitForSeconds(18.5f);

        #region Wait for Leaving Scene
        bool hasLeftScene = false;
        Action<string> onLeftScene = delegate (string scene) { hasLeftScene = true; };

        SceneLoader.OnSceneLoaded += onLeftScene;

        sceneLoader.ChangeScene("EndCredits");

        while (!hasLeftScene)
        {
            yield return null;
        }

        SceneLoader.OnSceneLoaded -= onLeftScene;
        #endregion
    }

    private IEnumerator CreditsSequence()
    {
        AudioManager.Instance.SetMusicLoop(false);

        FindObjectOfType<SceneLoader>().FadeInInstant();
        yield return FindObjectOfType<EndCredits>().RollCredits();
    }
}

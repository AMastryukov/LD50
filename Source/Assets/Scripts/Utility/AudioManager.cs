using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UtilityCode;
using Random = UnityEngine.Random;

public class AudioManager : UnitySingletonPersistent<AudioManager>
{
    private static string musicVolumeKey = "MusicVolume";
    private static string sfxVolumeKey = "SFXVolume";

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [SerializeField] private AudioSource[] musicSources;
    [SerializeField] private AudioSource globalSFXSource;

    [SerializeField] private AudioClip[] NotebookScribbleSFX;
    [SerializeField] private AudioClip[] NotebookPageFlipSFX;

    [Header("Music")]
    [SerializeField] public AudioClip alleyway1Theme; 
    [SerializeField] public AudioClip alleyway2Theme; 
    [SerializeField] public AudioClip victimApartment1Theme;
    [SerializeField] public AudioClip victimApartment2Theme;
    [SerializeField] public AudioClip interrogationRoomTheme;
    [SerializeField] public AudioClip chopShopTheme;
    [SerializeField] public AudioClip betrayalTheme;
    [SerializeField] public AudioClip endTheme;
    
    private List<AudioSource> sfxSources = new List<AudioSource>();
    private List<AudioSource> voiceSources = new List<AudioSource>();

    private float defaultMusicVolume = 0.1f;

    //private float musicVolume = 1f;
    //private float sfxVolume = 1f;

    public override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;

        defaultMusicVolume = musicSources[0].volume;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        #region Organize Audio Sources
        sfxSources.Clear();
        voiceSources.Clear();

        var audioSources = FindObjectsOfType<AudioSource>().ToList();

        foreach(var source in audioSources)
        {
            if (source.CompareTag(GameConstants.TagConstants.SFXAudioSource))
            {
                sfxSources.Add(source);
            }
            else if (source.CompareTag(GameConstants.TagConstants.SFXAudioSource))
            {
                voiceSources.Add(source);
            }
        }
        #endregion
    }

    public void PlayNotebookFlipSound()
    {
        AudioClip clipToPlay = NotebookPageFlipSFX[Random.Range(0, NotebookPageFlipSFX.Length)];
        globalSFXSource?.PlayOneShot(clipToPlay);
    }
    
    public void PlayNotebookScribble()
    {
        AudioClip clipToPlay = NotebookScribbleSFX[Random.Range(0, NotebookScribbleSFX.Length)];
        globalSFXSource?.PlayOneShot(clipToPlay);
    }

    public void PlayMusicDirectly(AudioClip music)
    {
        FadeOutMusic();
        foreach (AudioSource source in musicSources)
        {
            if (!source.isPlaying)
            {
                source.clip = music;
                source.Play();
                return;
            }
        }
    }

    public void FadeOutMusic()
    {
        foreach (AudioSource source in musicSources)
        {
            if (source.isPlaying)
            {
                DOTween.To(() => source.volume, x => source.volume = x, 0, 1f).onComplete = () =>
                {
                    source.Stop();
                };
            }
        }
    }
    
    public void FadeInMusic(AudioClip music,float fadeTime = 1f)
    {
        AudioSource fadeInSource = null;
        foreach (AudioSource source in musicSources)
        {
            if (source.isPlaying)
            {
                DOTween.To(() => source.volume, x => source.volume = x, 0, fadeTime).onComplete = () =>
                {
                    source.Stop();
                };
            }
            else
            {
                fadeInSource = source;
            }
        }

        if (fadeInSource != null)
        {
            fadeInSource.clip = music;
            fadeInSource.volume = 0;
            fadeInSource.Play();
            DOTween.To(() => fadeInSource.volume, x => fadeInSource.volume = x, defaultMusicVolume, fadeTime);
        }
        else
        {
            Debug.Log("[DEBUG] No free AudioSource to perform a fade in....Using direct play");
            PlayMusicDirectly(music);
        }
       
    }

    public void SetMusicLoop(bool shouldLoop)
    {
        foreach (AudioSource source in musicSources)
        {
            source.loop = shouldLoop;
        }
    }
}

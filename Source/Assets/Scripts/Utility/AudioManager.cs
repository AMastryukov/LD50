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

    [SerializeField] private AudioSource voiceSource;
    [SerializeField] private SubtitleAudio[] voiceLines;
    [SerializeField] private TextMeshProUGUI subtitleBox;

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
    
    [SerializeField] private List<AudioSource> musicSources = new List<AudioSource>();
    [SerializeField] private List<AudioSource> sfxSources = new List<AudioSource>();

    private float defaultMusicVolume = 0.1f;

    //private float musicVolume = 1f;
    //private float sfxVolume = 1f;

    public override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;

        defaultMusicVolume = musicSources[0].volume;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        #region Organize Audio Sources
        var audioSources = FindObjectsOfType<AudioSource>().ToList();

        foreach(var source in audioSources)
        {
            if (source.CompareTag(GameConstants.TagConstants.SFXAudioSource))
            {
                sfxSources.Add(source);
            }
        }
        #endregion
    }

    public void OnNoteBookPageFlip()
    {
        AudioClip clipToPlay = NotebookPageFlipSFX[Random.Range(0, NotebookPageFlipSFX.Length)];
        sfxSources[0]?.PlayOneShot(clipToPlay);
    }
    
    public void OnNoteBookScribble()
    {
        AudioClip clipToPlay = NotebookScribbleSFX[Random.Range(0, NotebookScribbleSFX.Length)];
        sfxSources[0]?.PlayOneShot(clipToPlay);
    }

    public IEnumerator WaitForVoiceline(int ID)
    {
        if (ID >= voiceLines.Length) { yield break; }

        voiceSource.clip = voiceLines[ID].clip;
        voiceSource.Play();

        if (subtitleBox != null)
        {
            subtitleBox.text = voiceLines[ID].subtitle;
        }

        yield return new WaitForSeconds(voiceSource.clip.length);

        if (subtitleBox != null)
        {
            subtitleBox.text = "";
        }
    }

    public void PlayMusicDirectly(AudioClip music)
    {
        StopAllMusic();
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

    public void StopAllMusic()
    {
        foreach (AudioSource source in musicSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
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
}

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
    [SerializeField] public AudioClip alleywayTheme; 
    [SerializeField] public AudioClip victimApartmentTheme;
    [SerializeField] public AudioClip interrogationRoomTheme;
    [SerializeField] public AudioClip chopShopTheme;
    
    private List<AudioSource> musicSources;
    private List<AudioSource> sfxSources;

    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    public override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        #region Organize Audio Sources
        musicSources = new List<AudioSource>();
        sfxSources = new List<AudioSource>();

        var audioSources = FindObjectsOfType<AudioSource>().ToList();

        foreach(var source in audioSources)
        {
            if (source.CompareTag(GameConstants.TagConstants.MusicAudioSource))
            {
                musicSources.Add(source);
            }
            else if (source.CompareTag(GameConstants.TagConstants.SFXAudioSource))
            {
                sfxSources.Add(source);
            }
        }
        #endregion

        musicVolume = PlayerPrefs.GetFloat(musicVolumeKey, 1f);
        sfxVolume = PlayerPrefs.GetFloat(sfxVolumeKey, 1f);

        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
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

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat(musicVolumeKey, volume);

        foreach (var source in musicSources)
        {
            source.volume = musicVolume;
        }

        //Uncomment Later
       //musicVolumeSlider.value = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat(sfxVolumeKey, volume);

        foreach (var source in sfxSources)
        {
            source.volume = sfxVolume;
        }
        //Uncomment Later
        //sfxVolumeSlider.value = sfxVolume;
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
            DOTween.To(() => fadeInSource.volume, x => fadeInSource.volume = x, 1f, fadeTime);
        }
        else
        {
            Debug.Log("[DEBUG] No free AudioSource to perform a fade in....Using direct play");
            PlayMusicDirectly(music);
        }
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is what plays the voicelines that the character hears "in their head" - phone calls, them saying stuff, etc
/// </summary>
public class PlayerVoice : MonoBehaviour
{
    private VoicelineSubtitles voicelineSubtitles;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        voicelineSubtitles = FindObjectOfType<VoicelineSubtitles>();
    }

    public IEnumerator PlayAudio(VoiceLineData voicelineData)
    {
        if (!GameManager.Instance.EnableVoicelines) { yield break; }

        StartCoroutine(voicelineSubtitles.ShowSubtitle(voicelineData));

        audioSource.clip = voicelineData.AudioClip;
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator PlayAudio(AudioClip clip)
    {
        if (!GameManager.Instance.EnableVoicelines) { yield break; }

        audioSource.clip = clip;
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);
        yield return new WaitForSeconds(0.5f);
    }
}

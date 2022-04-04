using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is what plays the voicelines that the character hears "in their head" - phone calls, them saying stuff, etc
/// </summary>
public class PlayerVoice : MonoBehaviour
{
    private AudioSource audioSource;

    public void PlayAudio(VoiceLineData voicelineData)
    {
        audioSource.clip = voicelineData.AudioClip;
        audioSource.Play();
    }
}

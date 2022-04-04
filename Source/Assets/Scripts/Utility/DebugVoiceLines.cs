using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugVoiceLines : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomVoiceline()
    {
        audioSource.clip = DataManager.Instance.GetRandomGenericPlayerEvidenceVoiceline().AudioClip;
        audioSource.Play();
    }

    public void SetPlayerMale()
    {
        PlayerPrefs.SetString("PlayerVoice", "MALE");
    }

    public void SetPlayerFemale()
    {
        PlayerPrefs.SetString("PlayerVoice", "FEMALE");
    }
}

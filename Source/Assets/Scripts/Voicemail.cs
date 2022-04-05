using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voicemail : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomVoiceline()
    {
        audioSource.clip = DataManager.Instance.GetVoiceMail();
        audioSource.Play();
    }
}

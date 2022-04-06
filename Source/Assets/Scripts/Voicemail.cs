using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voicemail : MonoBehaviour
{
    private AudioSource audioSource;
    private bool played = false;

    [SerializeField] private AudioClip warranty;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomVoiceline()
    {
        if (played) return;

        audioSource.clip = warranty;
        audioSource.Play();

        played = true;
    }
}

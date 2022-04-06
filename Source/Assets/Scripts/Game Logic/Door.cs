using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class Door : Interactable
{
    public static Action OnDoorOpened;

    public string SceneName;
    public bool IsUnlocked = false;

    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorLocked;

    private AudioSource audioSource;
    private bool Opened = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void GoToNextScene()
    {
        if (!IsUnlocked) 
        { 
            Debug.Log("Door is locked"); 
            audioSource.PlayOneShot(doorLocked);
            return; 
        }

        if (SceneName.Equals("")) 
        { 
            Debug.LogError("This door is not bound to any scene"); 
        }

        audioSource.PlayOneShot(doorOpen);

        SceneLoader.Instance.ChangeScene(SceneName);

        IsUnlocked = false;
    }

    public void OpenDoorPhysically()
    {
        if (!IsUnlocked)
        {
            Debug.Log("Door is locked");
            audioSource.PlayOneShot(doorLocked);
            return;
        }


        if (!Opened)
        {
            OnDoorOpened?.Invoke();

            transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 120, 0), 2f).SetEase(Ease.OutCirc);
            GetComponent<Collider>().enabled = false;
            Opened = true;

            audioSource.PlayOneShot(doorOpen);
        }
    }
}

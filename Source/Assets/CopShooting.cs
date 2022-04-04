using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class CopShooting : MonoBehaviour
{
    [SerializeField] private GameObject muzzleFlash; 
    [SerializeField] private AudioClip shootSoundEffect;
    
    
    private AudioSource CopAudioSource;
    private GameObject deathCamera;
    private float flashTimer = 0.08f;
    private PlayerManager manager;
    
    
    private void Start()
    {
        muzzleFlash.SetActive(false);
        
        CopAudioSource = gameObject.GetComponent<AudioSource>();
        manager = FindObjectOfType<PlayerManager>();
        deathCamera = GameObject.FindWithTag("deathCam");
        
        if(deathCamera == null)
            Debug.LogWarning("Death camera not found!");
    }

    
    //Call this to shoot the gun
    public void CopShootGun()
    {
        //Animation 
        CopAudioSource.PlayOneShot(shootSoundEffect);
        muzzleFlash.SetActive(true);

        if (deathCamera != null)
        {
            if (deathCamera.GetComponent<CinemachineVirtualCamera>() != null)
            {
                if (deathCamera.GetComponent<Animator>())
                {
                    deathCamera.GetComponent<CinemachineVirtualCamera>().Priority = 15;
                    deathCamera.GetComponent<Animator>().SetTrigger("Die");
                    
                    if(manager != null)
                        manager._currentState = PlayerManager.PlayerStates.Wait;
                }
            }
        }
    }


    private void Update()
    {
        if (muzzleFlash.activeSelf == true)
        {
            flashTimer -= Time.deltaTime;

            if (flashTimer <= 0f)
            {
                muzzleFlash.SetActive(false);
                flashTimer = 0.08f;
            }
        }
    }
}

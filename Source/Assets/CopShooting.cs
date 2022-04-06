using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class CopShooting : MonoBehaviour
{
    [SerializeField] private GameObject muzzleFlash; 
    [SerializeField] private AudioSource shootSource;
    [SerializeField] private AudioClip shootSoundEffect;
    [SerializeField] private Transform shootingArmTransform;
    [SerializeField] private Transform bloodPool;
    
    private GameObject deathCamera;
    private float flashTimer = 0.15f;
    private PlayerManager manager;

    private float originalZArmPosition;
    
    
    private void Start()
    {
        muzzleFlash.SetActive(false);
        
        manager = FindObjectOfType<PlayerManager>();
        deathCamera = GameObject.FindWithTag("deathCam");
        
        if(deathCamera == null)
            Debug.LogWarning("Death camera not found!");

        bloodPool.gameObject.SetActive(false);
    }

    public IEnumerator RaiseWeapon()
    {
        originalZArmPosition = shootingArmTransform.localEulerAngles.z;
        shootingArmTransform.DOLocalRotate(new Vector3(shootingArmTransform.localEulerAngles.x, shootingArmTransform.localEulerAngles.y, -15f), 1f).SetEase(Ease.OutQuart);

        yield return null;
    }

    public IEnumerator ShootGun()
    {
        shootSource.PlayOneShot(shootSoundEffect);

        muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(flashTimer);

        muzzleFlash.SetActive(false);

        if (deathCamera != null)
        {
            if (deathCamera.GetComponent<CinemachineVirtualCamera>() != null)
            {
                if (deathCamera.GetComponent<Animator>())
                {
                    deathCamera.GetComponent<CinemachineVirtualCamera>().Priority = 15;
                    deathCamera.GetComponent<Animator>().SetTrigger("Die");
                    
                    if (manager != null)
                        manager._currentState = PlayerManager.PlayerStates.Wait;
                }
            }
        }

        bloodPool.gameObject.SetActive(true);
        bloodPool.DOScale(Vector3.one * 20f, 20f);

        yield return new WaitForSeconds(0.25f);

        shootingArmTransform.DOLocalRotate(new Vector3(shootingArmTransform.localEulerAngles.x, shootingArmTransform.localEulerAngles.y, originalZArmPosition), 10f).SetEase(Ease.OutQuart);

        yield return null;
    }
}

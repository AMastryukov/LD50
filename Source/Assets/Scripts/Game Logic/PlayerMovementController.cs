using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float walkSpeed = 3f;

    [Header("Footsteps")]
    [SerializeField] private List<AudioClip> footStepSounds;
    [SerializeField] private float footStepFrequency = 0.5f;
    private float footStepTimer;

    Vector3 desiredMovementVector = Vector3.zero;
    
    private PlayerManager manager; 
    private AudioSource audioSource;
    
    
    private void Awake()
    {
        manager = FindObjectOfType<PlayerManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
        footStepTimer = 0;
    }

    
    private void Update()
    {
        GetMovementInput();
        MovePlayer();
        Footstepts();
    }

    
    private void GetMovementInput()
    {
        if (manager.CurrentState != PlayerManager.PlayerStates.Move)
        {
            desiredMovementVector = Vector3.zero;
            return;
        }
        
        // Get axis input on horizontal and vertical axes
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector3 forwardMovement = transform.forward * verticalInput;
        Vector3 strafeMovement = transform.right * horizontalInput;

        desiredMovementVector = Vector3.Normalize(strafeMovement + forwardMovement);
    }

    
    private void Footstepts()
    {
        if (manager._currentState == PlayerManager.PlayerStates.Move)
        {
            if (desiredMovementVector.magnitude != 0)
            {
                footStepTimer -= Time.deltaTime;
                if (footStepTimer <= 0)
                {
                    footStepTimer = footStepFrequency;

                    audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    audioSource.PlayOneShot(footStepSounds[UnityEngine.Random.Range(0, footStepSounds.Count)]);
                }
            }
        }
    }

    
    private void MovePlayer()
    {
        characterController.SimpleMove(desiredMovementVector * walkSpeed);
    }
}

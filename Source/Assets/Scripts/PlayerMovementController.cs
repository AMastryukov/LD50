using System;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float walkSpeed = 3f;

    Vector3 desiredMovementVector = Vector3.zero;
    private PlayerManager manager; 

    private void Awake()
    {
        manager = FindObjectOfType<PlayerManager>();

    }

    private void Update()
    {
        
        GetMovementInput();
        MovePlayer();
    }

    private void GetMovementInput()
    {
        
        if (manager.playerState == PlayerManager.PlayerStates.Inspect)
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

    private void MovePlayer()
    {
        characterController.SimpleMove(desiredMovementVector * walkSpeed);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerCameraController : MonoBehaviour
{
 [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform fpsOrigin;

    // Adjustable Settings
    [Header("General Camera Settings")]
    [SerializeField] private  float cameraFOV = 90f;
    [SerializeField] private float cameraLerpSpeed = 0.5f;

    [Header("Mouse Sensitivity Settings")]
    [SerializeField] private float xMouseSensitivity = 1.0f;
    [SerializeField] private float yMouseSensitivity = 1.0f;

    private float xCameraRotation;
    private float yCameraRotation;

    private Vector3 targetCameraPosition;
    private PlayerManager playerManager;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Start()
    {
        // Disable and lock the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        cameraTransform.GetComponent<Camera>().fieldOfView = cameraFOV;

        xCameraRotation = cameraTransform.rotation.eulerAngles.x;
        yCameraRotation = cameraTransform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        // If the game is paused, don't do anything
        if (Time.timeScale == 0f || 
            playerManager.CurrentState == PlayerManager.PlayerStates.Inspect || 
            playerManager.CurrentState == PlayerManager.PlayerStates.Wait) 
        { 
            return; 
        }

        xCameraRotation -= Input.GetAxisRaw("Mouse Y") * xMouseSensitivity;
        yCameraRotation += Input.GetAxisRaw("Mouse X") * yMouseSensitivity;

        // Clamp rotation
        if (xCameraRotation < -90f)
        {
            xCameraRotation = -90f;
        }
        else if (xCameraRotation > 90f)
        {
            xCameraRotation = 90f;
        }

        // Set camera and character controller rotation
        characterController.transform.rotation = Quaternion.Euler(0f, yCameraRotation, 0f);
        cameraTransform.rotation = Quaternion.Euler(xCameraRotation, yCameraRotation, 0f);

        targetCameraPosition = fpsOrigin.transform.position;

        // Smoothly lerp camera on the y-axis
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetCameraPosition, cameraLerpSpeed * Time.deltaTime);
    }
}
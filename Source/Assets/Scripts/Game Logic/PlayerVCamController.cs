using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerVCamController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform fpsOrigin;
    private CinemachineVirtualCamera interrogationCamera;
    private CinemachineVirtualCamera currentVirtualCamera;
    
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

    private PlayerManager manager;

    
    private void Awake()
    {
        manager = FindObjectOfType<PlayerManager>();
    }

    private void Start()
    {

        //Interrogation Camera
        if (GameObject.FindGameObjectWithTag("benchCam") != null)
            interrogationCamera = GameObject.FindGameObjectWithTag("benchCam").GetComponent<CinemachineVirtualCamera>();
        else
            Debug.LogWarning("InterrogationCamera was not found, add 'benchCam' tag for intterrogation camera");
        
        
        //Player camera
        currentVirtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        if(currentVirtualCamera == null)
            Debug.LogWarning("No virtual camera found for player");
        
        currentVirtualCamera.m_Lens.FieldOfView = cameraFOV;
        
        
        // Disable and lock the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        
        //Camera rotation initialization
        xCameraRotation = currentVirtualCamera.transform.rotation.eulerAngles.x;
        yCameraRotation = currentVirtualCamera.transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        // If the game is paused, don't do anything
        if (Time.timeScale == 0f ||
            manager.CurrentState == PlayerManager.PlayerStates.Inspect || 
            manager.CurrentState == PlayerManager.PlayerStates.Wait) 
        { 
            return; 
        }

        MoveCamera();
    }


    private void MoveCamera()
    {
        
        if (manager.CurrentState == PlayerManager.PlayerStates.Interrogate)
        {
            if (interrogationCamera != null)
            {
                interrogationCamera.Priority = 11;
                currentVirtualCamera = interrogationCamera;
                targetCameraPosition = interrogationCamera.transform.position;
            }
            else
            {
                Debug.LogWarning("No interrogation camera was found while trying to interrogate");
                manager.CurrentState = PlayerManager.PlayerStates.Move;
            }
        }
        else if(manager.CurrentState == PlayerManager.PlayerStates.Move)
        {
            
            xCameraRotation -= Input.GetAxisRaw("Mouse Y") * xMouseSensitivity;
            yCameraRotation += Input.GetAxisRaw("Mouse X") * yMouseSensitivity;

            if(interrogationCamera != null)
                interrogationCamera.Priority = 9;

            currentVirtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
            
            //Clamp
            if (xCameraRotation < -90f)
            {
                xCameraRotation = -90f;
            }
            else if (xCameraRotation > 90f)
            {
                xCameraRotation = 90f;
            }
            
            characterController.transform.rotation = Quaternion.Euler(0f, yCameraRotation, 0f);
            currentVirtualCamera.transform.rotation = Quaternion.Euler(xCameraRotation, yCameraRotation, 0f);
            
            targetCameraPosition = fpsOrigin.transform.position;
            // Smoothly lerp camera on the y-axis
            currentVirtualCamera.transform.position = Vector3.Lerp(currentVirtualCamera.transform.position, targetCameraPosition, cameraLerpSpeed * Time.deltaTime);
        }
    }

}
